using System.Globalization;
using System;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Crystal.Engine.Backends.Yaml
{
    internal static class YamlExtensions
    {
        public static YamlMappingNode Map(this YamlNode self)
        {
            try
            {
                return (YamlMappingNode)self;
            }
            catch (InvalidCastException)
            {
                throw new YamlException(self.Start, self.End, "Expected mapping node!");
            }
        }

        public static YamlSequenceNode Seq(this YamlNode self)
        {
            try
            {
                return (YamlSequenceNode)self;
            }
            catch (InvalidCastException)
            {
                throw new YamlException(self.Start, self.End, "Expected sequence node!");
            }
        }

        public static YamlScalarNode Val(this YamlNode self)
        {
            try
            {
                return (YamlScalarNode)self;
            }
            catch (InvalidCastException)
            {
                throw new YamlException(self.Start, self.End, "Expected value node!");
            }
        }

        public static string String(this YamlScalarNode self)
        {
            return self.Value;
        }

        public static bool TryInt(this YamlScalarNode self, out int val)
        {
            return int.TryParse(self.String(), NumberStyles.Number, CultureInfo.InvariantCulture, out val);

        }

        public static bool TryFloat(this YamlScalarNode self, out float val)
        {
            return float.TryParse(self.String(), NumberStyles.Float, CultureInfo.InvariantCulture, out val);
        }
    }
}