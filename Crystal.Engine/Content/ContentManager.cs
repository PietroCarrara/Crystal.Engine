using System.IO;
using System;
using System.Collections.Generic;
using Crystal.Framework.Content;
using Crystal.Framework.Graphics;
using Crystal.Engine.Graphics;
using Crystal.Engine.Scene;
using SFML.Graphics;

namespace Crystal.Engine.Content
{
    public class ContentManager : IContentManager
    {
        // Default path for contents
        private readonly string rootPath;

        // Cache for the loaded items
        private Dictionary<string, IDisposable> loadedItems = new Dictionary<string, IDisposable>();

        // Loaders keyed by the typecode of the type they load
        private static readonly Dictionary<Type, Func<string, ContentManager, IDisposable>> typeLoaders =
            new Dictionary<Type, Func<string, ContentManager, IDisposable>>();

        // Loaders keyed by the extension of the file type they load
        private static readonly List<(HashSet<string> extensions, Func<string, ContentManager, IDisposable> loader)>
            extensionLoaders = new List<(HashSet<string> extensions, Func<string, ContentManager, IDisposable> loader)>();

        /// <summary>
        /// Setup loaders
        /// </summary>
        static ContentManager()
        {
            typeLoaders.Add(typeof(IDrawable), CrystalTexture.Loader);
            typeLoaders.Add(typeof(Framework.Scene), CrystalScene.Loader);

            extensionLoaders.Add((Extensions.IDrawable, CrystalTexture.Loader));
            extensionLoaders.Add((Extensions.Scene, CrystalScene.Loader));
        }

        public ContentManager(string rootPath = "res")
        {
            this.rootPath = rootPath;
        }

        public void Dispose()
        {
            foreach (var item in loadedItems)
            {
                item.Value.Dispose();
            }
        }

        public T Load<T>(string path, bool manage = true) where T : IDisposable
        {
            // TODO: Check for 'manage'
            path = adjustPath(path);

            var type = typeof(T);

            if (typeLoaders.ContainsKey(type))
            {
                if (loadedItems.ContainsKey(path))
                {
                    return (T)loadedItems[path];
                }

                var res = (T)(typeLoaders[type](path, this));

                loadedItems.Add(path, res);
                return res;
            }

            throw new Exception($"No loader found for type '{type.Name}'!");
        }

        public IDisposable Load(string path, bool manage = true)
        {
            // TODO: Check for 'manage'
            path = adjustPath(path);

            var ext = Path.GetExtension(path);

            foreach (var loader in extensionLoaders)
            {
                if (loader.extensions.Contains(ext))
                {
                    return loader.loader(path, this);
                }
            }

            throw new Exception($"No loader found for file '{path}'!");
        }

        private string adjustPath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }

            return Path.Join(Directory.GetCurrentDirectory(), rootPath, path);
        }

        private static class Extensions
        {
            public static readonly HashSet<string>
                IDrawable = new HashSet<string>
                    { ".png", ".jpg", ".jpeg", ".bmp", ".tga", ".psd", ".gif", ".hdr", ".pic", ".pnm" },
                Scene = new HashSet<string>
                    { ".yml" };
        }
    }
}