using System;
using System.Linq;
using System.Reflection;

namespace Crystal.Engine.Reflection
{
    public static class ReflectionUtil
    {
        public static Type GetType(string fullType)
        {
            Exception e;

            try
            {
                return Type.GetType(fullType, true);
            }
            catch(TypeLoadException tle)
            {
                e = tle;
            }
            
            var parts = fullType.Split(',');

            var typeName = parts[0].Trim();
            var assemblyName = parts.Length > 1 ? parts[1].Trim() : "";

            // Load the type using the specified assembly
            if (assemblyName != "")
            {
                return Type.GetType($"{typeName},{assemblyName}", true);
            }
            
            // Ask for each assembly to load our type
            // TODO: Remove the current assembly from the list
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var t = assembly.GetType(fullType);

                if (t != null)
                {
                    return t;
                }
            }

            throw e;
        }
    }
}
