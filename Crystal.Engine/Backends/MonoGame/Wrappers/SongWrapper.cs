using System;
using Crystal.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Crystal.Engine.Backends.MonoGame.Wrappers
{
    public class SongWrapper : MonoGameWrapper<Song>, IMusic
    {
        private float volume = 1;
        
        public float Volume
        {
            get => volume;
            set
            {
                if (MediaPlayer.Queue.ActiveSong == this.Resource)
                {
                    MediaPlayer.Volume = value;
                }

                this.volume = value;
            }
        }

        public SongWrapper(Song content) : base(content)
        { }

        public event Action<IMusic> PlaybackEnded;

        public void Play(float volume)
        {
            this.volume = volume;
            this.Play();
        }

        public void Play()
        {
            MediaPlayer.Volume = this.volume;
            MediaPlayer.Play(this.Resource);

            MediaPlayer.MediaStateChanged += this.onMediaStateChanged;
        }
        
        private void onMediaStateChanged(object sender, EventArgs e)
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.MediaStateChanged -= this.onMediaStateChanged;
                this.PlaybackEnded?.Invoke(this);
            }
        }

        public void Dispose()
        {
            this.Resource.Dispose();
        }
    }
}