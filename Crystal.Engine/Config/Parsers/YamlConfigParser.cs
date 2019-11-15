using System;
using System.Linq;
using System.IO;
using Crystal.Engine.Input;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using Crystal.Engine.Backends.MonoGame;
using Crystal.Engine.Backends.Yaml;

namespace Crystal.Engine.Config.Parsers
{
    public class YamlConfigParser : IConfigParser
    {
        public CrystalConfig FromFile(string path)
        {
            return this.FromText(File.ReadAllText(path));
        }

        public CrystalConfig FromText(string text)
        {
            var yaml = new YamlStream();
            yaml.Load(new StringReader(text));

            var config = CrystalConfig.Default;

            if (yaml.Documents.Any())
            {
                var root = yaml.Documents[0].RootNode.Map();

                if (root.Children.ContainsKey("main"))
                {
                    config.MainScene = root["main"].Val().String();
                }

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

                        var action = CrystalInputAction.ParseAction(keys);
                        config.Actions.Add(action, name);
                    }
                }
            }

            return config;
        }
    }
}