using Crystal.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Crystal.Engine.Backends.MonoGame.Wrappers
{
    public class SoundEffectWrapper : MonoGameWrapper<SoundEffect>, IAudio
    {
        public SoundEffectWrapper(SoundEffect e) : base(e)
        { }

        public void Play(float volume = 1, float panning = 0)
        {
            // TODO: Panning is not working
            this.Resource.Play(volume, 0, panning);
        }

        public void Dispose()
        {
            this.Resource.Dispose();
        }
    }
}