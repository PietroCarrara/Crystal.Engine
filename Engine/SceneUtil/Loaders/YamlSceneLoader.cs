using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

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
            if (node.NodeType == YamlNodeType.Mapping)
            {
                return parseEntity(node.Map().Children);
            }

            throw new Exception("Invalid entity structure!");
        }

        private EntityModel parseEntity(IDictionary<YamlNode, YamlNode> map)
        {
            if (map.Count != 1)
            {
                throw new Exception("Invalid entity structure!");
            }

            var node = map.First();

            return new EntityModel
            {
                Name = node.Key.Val().String(),
                Components = parseComponents(node.Value)
            };
        }

        private List<ComponentModel> parseComponents(YamlNode node)
        {
            var res = new List<ComponentModel>();

            foreach (var component in node.Seq().Children)
            {
                res.Add(parseComponent(component));
            }

            return res;
        }

        private ComponentModel parseComponent(YamlNode node)
        {
            if (node.NodeType == YamlNodeType.Scalar)
            {
                return new ComponentModel
                {
                    Type = Type.GetType(node.Val().String())
                };
            }
            else if (node.NodeType == YamlNodeType.Mapping)
            {
                return parseComponent(node.Map().Children);
            }

            throw new Exception("Invalid entity structure!");
        }

        private ComponentModel parseComponent(IDictionary<YamlNode, YamlNode> map)
        {
            if (map.Count != 1)
            {
                throw new Exception("Invalid entity structure!");
            }

            var component = map.First();

            return new ComponentModel
            {
                Type = Type.GetType(component.Key.Val().String()),
                CtorArgs = parseArguments(component.Value)
            };
        }

        private object[] parseArguments(YamlNode node)
        {
            // TODO: Implement
            throw new NotImplementedException();
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
    }
}