using System.IO;
using Microsoft.Xna.Framework.Content;
using Crystal.Framework.Content;

namespace Crystal.Engine.Backends.MonoGame.Wrappers
{
    public class CrystalContentManager : MonoGameWrapper<ContentManager>, IContentManager
    {
        public CrystalContentManager(ContentManager content) : base(content)
        {
        }

        public T Load<T>(string path)
        {
            if (path.StartsWith("engine://"))
            {
                path = path.Replace("engine://", "");
            }
            else
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), Resource.RootDirectory, path);
            }

            return (T)MonoGameWrapper.Wrap(this.Resource.Load<object>(path));
        }
    }
}