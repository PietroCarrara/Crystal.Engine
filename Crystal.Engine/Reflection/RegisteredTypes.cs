using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Crystal.Engine.Reflection
{
    public static class RegisteredTypes
    {
        private static List<Assembly> loadedAssemblies = new List<Assembly>();

        /// <summary>
        /// Given a directory, loads all assemblies inside it
        /// </summary>
        /// <param name="dir">The directory to look into</param>
        public static void LoadAllIn(string dir)
        {
            if (!Path.IsPathRooted(dir))
            {
                dir = Path.Combine(Directory.GetCurrentDirectory(), dir);
            }

            var files = Directory.GetFiles(dir);
            foreach (var file in files.Where(f => f.EndsWith(".dll")))
            {
                var fileBytes = File.ReadAllBytes(file);
                byte[] pdbBytes = null;

                var pdbPath = Path.ChangeExtension(file, ".pdb");
                if (File.Exists(pdbPath))
                {
                    pdbBytes = File.ReadAllBytes(pdbPath);
                }                
                
                loadedAssemblies.Add(Assembly.Load(fileBytes, pdbBytes));
            }
        }

        public static Type GetType(string fullType)
        {
            // Crystal.Engine type
            var t = Assembly.Load("Crystal.Engine").GetType(fullType);
            if (t != null) 
            {
                return t;
            }

            // Crystal.Framework type
            t = Assembly.Load("Crystal.Framework").GetType(fullType);
            if (t != null)
            {
                return t;
            }

            // Unknown type, probably a custom type in a loaded assemby
            foreach (var assembly in loadedAssemblies)
            {
                t = assembly.GetType(fullType);

                if (t != null)
                {
                    return t;
                }
            }

            // Okay, so not a custom type? Let's try to search EVERYWHERE (including where we've already been)
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                t = assembly.GetType(fullType);

                if (t != null)
                {
                    return t;
                }
            }            

            throw new TypeLoadException($"Type \"{fullType}\" wasn't found. " +
                                         "Maybe you are missing an assembly reference?");
        }
    }
}
