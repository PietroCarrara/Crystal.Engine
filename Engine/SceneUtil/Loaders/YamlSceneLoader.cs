using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using Microsoft.Xna.Framework;

namespace Crystal.Engine.SceneUtil.Loaders
{
    public class YamlSceneLoader : ISceneLoader
    {
        public SceneInitializer FromFilePath(string path)
        {
            return this.FromText(File.ReadAllText(path));
        }

        public SceneInitializer FromText(string text)
        {
            var yaml = new YamlStream();
            yaml.Load(new StringReader(text));

            var root = yaml.Documents[0].RootNode.Map();

            var initializer = new SceneInitializer(root["name"].Val().String());

            // Add resources
            if (root.Children.ContainsKey("resources"))
            {
                foreach (var res in root["resources"].Map().Children)
                {
                    initializer.Resources.Add(
                        res.Key.Val().String(),
                        res.Value.Val().String()
                    );
                }
            }

            // Add systems
            if (root.Children.ContainsKey("systems"))
            {
                foreach (var system in root["systems"].Seq().Children)
                {
                    initializer.Systems.Add(
                        Type.GetType(system.Val().String(), true)
                    );
                }
            }

            // Add systems
            if (root.Children.ContainsKey("renderers"))
            {
                foreach (var renderer in root["renderers"].Seq().Children)
                {
                    initializer.Renderers.Add(
                        Type.GetType(renderer.Val().String(), true)
                    );
                }
            }

            if (root.Children.ContainsKey("entities"))
            {
                foreach (var entity in root["entities"].Seq().Children)
                {
                    var model = parseEntity(entity);

                    initializer.Entities.Add(model);
                }
            }

            return initializer;
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

            Console.WriteLine(node);

            throw new Exception("Invalid entity structure!");
        }

        private EntityModel parseEntity(YamlMappingNode map)
        {
            if (map.Children.Count != 1)
            {
                throw new Exception("Invalid entity structure!");
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
                    Type = Type.GetType(node.Val().String(), true),
                    CtorArgs = new object[] { }
                };
            }
            else if (node.NodeType == YamlNodeType.Mapping)
            {
                return parseObject(node.Map().Children);
            }

            throw new Exception("Invalid entity structure!");
        }

        private ObjectModel parseObject(IDictionary<YamlNode, YamlNode> map)
        {
            if (map.Count != 1)
            {
                throw new Exception("Invalid entity structure!");
            }

            var component = map.First();

            return new ObjectModel
            {
                Type = Type.GetType(component.Key.Val().String(), true),
                CtorArgs = parseArguments(component.Value.Seq())
            };
        }

        private object[] parseArguments(YamlNode node)
        {
            if (node.NodeType != YamlNodeType.Sequence)
            {
                throw new Exception("Invalid structure for constructor arguments!");
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
                    throw new Exception("Invalid argument structure!");
                }
            }

            return res.ToArray();
        }

        /// <summary>
        /// Casts a scalar node to a C# object following the rules:
        /// Single-Length, single quoted strings become char
        /// Else, quoted (single or double), literal or folded values become strings
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
                    int i;
                    float f;
                    if (node.TryInt(out i))
                        return i;
                    if (node.TryFloat(out f))
                        return f;

                    return new ObjectModel
                    {
                        Type = Type.GetType(node.String(), true),
                        CtorArgs = new object[] { }
                    };

                default:
                    throw new Exception("Trying to parse arguments from invalid node!");
            }
        }
    }

    internal static class YamlExtensionMethods
    {
        public static YamlMappingNode Map(this YamlNode self)
        {
            return (YamlMappingNode)self;
        }

        public static YamlSequenceNode Seq(this YamlNode self)
        {
            return (YamlSequenceNode)self;
        }

        public static YamlScalarNode Val(this YamlNode self)
        {
            return (YamlScalarNode)self;
        }

        public static string String(this YamlScalarNode self)
        {
            return self.Value;
        }

        public static bool TryInt(this YamlScalarNode self, out int val)
        {
            return int.TryParse(self.String(), out val);
        }

        public static bool TryFloat(this YamlScalarNode self, out float val)
        {
            return float.TryParse(self.String(), out val);
        }
    }
}