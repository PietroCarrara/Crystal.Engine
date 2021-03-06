using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using Crystal.Framework;
using Crystal.Framework.Graphics.Scalers;
using Crystal.Engine.Reflection;

namespace Crystal.Engine.Scene.Loaders
{
    public class YamlSceneLoader : ISceneLoader
    {
        public SceneInitializer FromFilePath(string path)
        {
            return this.FromText(File.ReadAllText(path));
        }

        public SceneInitializer FromText(string text)
        {
            return this.fromText(text);
        }

        private SceneInitializer fromText(string text)
        {
            var yaml = new YamlStream();
            yaml.Load(new StringReader(text));

            var root = yaml.Documents[0].RootNode.Map();

            var size = new Point(1280, 720);
            var scaler = new LetterboxingScaler();
            var systems = new List<ObjectModel>();
            var renderers = new List<ObjectModel>();
            var entities = new List<EntityModel>();
            var actions = new List<InputAction>();

            if (root.Children.ContainsKey("size"))
            {
                // TODO
            }

            if (root.Children.ContainsKey("scaler"))
            {
                // TODO
            }

            // Add systems
            if (root.Children.ContainsKey("systems"))
            {
                foreach (var system in root["systems"].Seq().Children)
                {
                    switch (system.NodeType)
                    {
                        case YamlNodeType.Scalar:
                            systems.Add(
                                new ObjectModel
                                {
                                    Type = RegisteredTypes.GetType(system.Val().String()),
                                    CtorArgs = new object[] { },
                                }
                            );
                            break;
                        case YamlNodeType.Mapping:
                            systems.Add(
                                new ObjectModel
                                {
                                    Type = RegisteredTypes.GetType(system.Map().First().Key.Val().String()),
                                    CtorArgs = parseArguments(system.Map().First().Value),
                                }
                            );
                            break;

                    }
                }
            }

            // Add renderers
            if (root.Children.ContainsKey("renderers"))
            {
                foreach (var renderer in root["renderers"].Seq().Children)
                {
                    switch (renderer.NodeType)
                    {
                        case YamlNodeType.Scalar:
                            renderers.Add(
                                new ObjectModel
                                {
                                    Type = RegisteredTypes.GetType(renderer.Val().String()),
                                    CtorArgs = new object[] { },
                                }
                            );
                            break;
                        case YamlNodeType.Mapping:
                            renderers.Add(
                                new ObjectModel
                                {
                                    Type = RegisteredTypes.GetType(renderer.Map().First().Key.Val().String()),
                                    CtorArgs = parseArguments(renderer.Map().First().Value),
                                }
                            );
                            break;
                    }
                }
            }

            // Add entities
            if (root.Children.ContainsKey("entities"))
            {
                foreach (var entity in root["entities"].Seq().Children)
                {
                    var model = parseEntity(entity);

                    entities.Add(model);
                }
            }

            // Add actions
            if (root.Children.ContainsKey("actions"))
            {
                foreach (var actionPair in root["actions"].Map())
                {
                    var name = actionPair.Key.Val().String();
                    var yamlKeys = actionPair.Value;

                    String[] keys;

                    switch (yamlKeys.NodeType)
                    {
                        // Single key
                        case YamlNodeType.Scalar:
                            keys = new string[] { yamlKeys.Val().String() };
                            break;

                        // Many keys
                        case YamlNodeType.Sequence:
                            keys = yamlKeys.Seq().Select(n => n.Val().String()).ToArray();
                            break;

                        default:
                            throw new YamlException(
                                yamlKeys.Start,
                                yamlKeys.End,
                                "Expected a sequence or value node, but got none!"
                            );
                    }

                    var buttons = keys.Select(k => (Buttons)Enum.Parse(typeof(Buttons), k));
                    actions.Add(new InputAction(name, buttons.ToArray()));
                }
            }

            return new SceneInitializer(size, scaler, systems, renderers, entities, actions);
        }

        private EntityModel parseEntity(YamlNode node)
        {
            // Named entity
            if (node.NodeType == YamlNodeType.Mapping)
            {
                return parseEntity(node.Map());
            }
            // Unnamed entity
            if (node.NodeType == YamlNodeType.Sequence)
            {
                return parseEntity(node.Seq());
            }

            throw new YamlException(node.Start, node.End, "Invalid entity structure");
        }

        private EntityModel parseEntity(YamlMappingNode map)
        {
            if (map.Children.Count != 1)
            {
                throw new YamlException(map.Start, map.End, "Invalid entity structure");
            }

            var node = map.First();

            return new EntityModel
            {
                Name = node.Key.Val().String(),
                Components = parseObjects(node.Value.Seq())
            };
        }

        private EntityModel parseEntity(YamlSequenceNode seq)
        {
            return new EntityModel
            {
                Name = null,
                Components = parseObjects(seq)
            };
        }

        private List<ObjectModel> parseObjects(YamlSequenceNode node)
        {
            var res = new List<ObjectModel>();

            foreach (var component in node.Children)
            {
                res.Add(parseObject(component));
            }

            return res;
        }

        private ObjectModel parseObject(YamlNode node)
        {
            if (node.NodeType == YamlNodeType.Scalar)
            {
                return new ObjectModel
                {
                    Type = RegisteredTypes.GetType(node.Val().String()),
                    CtorArgs = new object[] { }
                };
            }
            else if (node.NodeType == YamlNodeType.Mapping)
            {
                return parseObject(node.Map().Children);
            }

            throw new YamlException(node.Start, node.End, "Invalid object structure");
        }

        private ObjectModel parseObject(IDictionary<YamlNode, YamlNode> map)
        {
            if (map.Count != 1)
            {
                throw new YamlException(
                    map.First().Key.Start,
                    map.First().Key.End,
                    "Invalid object structure"
                );
            }

            var component = map.First();

            return new ObjectModel
            {
                Type = RegisteredTypes.GetType(component.Key.Val().String()),
                CtorArgs = parseArguments(component.Value.Seq())
            };
        }

        private object[] parseArguments(YamlNode node)
        {
            if (node.NodeType != YamlNodeType.Sequence)
            {
                throw new YamlException(node.Start, node.End, "Invalid object structure");
            }

            return parseArguments(node.Seq());
        }

        private object[] parseArguments(YamlSequenceNode array)
        {
            var res = new List<object>();

            foreach (var node in array.Children)
            {
                // Argument is a primitive
                if (node.NodeType == YamlNodeType.Scalar)
                {
                    res.Add(parseArgument(node.Val()));
                }
                // Argument is a object
                else if (node.NodeType == YamlNodeType.Mapping)
                {
                    res.Add(parseObject(node.Map()));
                }
                else
                {
                    throw new YamlException(node.Start, node.End, "Invalid argument structure");
                }
            }

            return res.ToArray();
        }

        /// <summary>
        /// Casts a scalar node to a C# object following the rules:
        /// Single-Length, single quoted strings become char
        /// Else, quoted (single or double), literal or folded values become strings
        /// Else, plain values equal to the string "null" become null
        /// Else, plain values that start with "~" become scene resources
        /// Else, plain values that can be cast to int become int
        /// Else, plain values that can be cast to float become float
        /// Else, plain values are cast to a object using the parameterless constructor
        /// (For parameterized constructors, see casting a mapping node)
        /// </summary>
        /// <param name="node"></param>
        private object parseArgument(YamlScalarNode node)
        {
            switch (node.Style)
            {
                case ScalarStyle.SingleQuoted:
                    if (node.String().Length == 1)
                        return node.String()[0];
                    else
                        return node.String();

                case ScalarStyle.DoubleQuoted:
                case ScalarStyle.Literal:
                case ScalarStyle.Folded:
                    return node.String();

                case ScalarStyle.Plain:
                    if (node.String() == "null")
                    {
                        return null;
                    }

                    if (node.String().StartsWith("~"))
                    {
                        return new ObjectModel
                        {
                            Type = typeof(Resource),
                            CtorArgs = new Object[] { node.String().Substring(1) }
                        };
                    }

                    int i;
                    float f;
                    if (node.TryInt(out i))
                        return i;
                    if (node.TryFloat(out f))
                        return f;

                    return new ObjectModel
                    {
                        Type = RegisteredTypes.GetType(node.String()),
                        CtorArgs = new object[] { }
                    };

                default:
                    throw new Exception("Trying to parse arguments from invalid node!");
            }
        }
    }
}