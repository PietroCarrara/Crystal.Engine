using System.Linq;
using System.IO;
using YamlDotNet.RepresentationModel;

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

                if (root.Children.ContainsKey("project"))
                {
                    config.Project = root["project"].Val().String();
                }
            }

            return config;
        }
    }
}