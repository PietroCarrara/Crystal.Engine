using System;
using Crystal.Framework;
using Crystal.Framework.Graphics;
using Crystal.Engine.SFML;
using Crystal.Engine.SDL;
using static SDL2.SDL;

namespace Crystal.Engine.Graphics
{
    public class CrystalCanvas : IResizeableRenderTarget
    {
        private Point maximumSize, size;
        private Renderer renderer;

        public Texture RenderTexture { get; private set; }
        public Point Size => size;

        public event IRenderTarget.SizeChangedEventHandler SizeChanged;

        public CrystalCanvas(int width, int height, Renderer renderer)
        {
            this.renderer = renderer;

            this.RenderTexture = this.createTexture(width, height);

            this.size = this.maximumSize = new Point(width, height);
        }

        private Texture createTexture(int w, int h)
        {
            return new Texture(SDL_CreateTexture(
                renderer,
                SDL_PIXELFORMAT_RGBA8888,
                (int)SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET,
                w,
                h
            ));
        }

        public void Clear(Framework.Graphics.Color color)
        {
            var rt = SDL_GetRenderTarget(renderer);

            SDL_SetRenderTarget(renderer, RenderTexture);
            SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
            SDL_RenderClear(renderer);

            SDL_SetRenderTarget(renderer, rt);
        }

        public void Dispose()
        {
            SDL_DestroyTexture(this.RenderTexture);
        }

        public void SetSize(Point size)
        {
            var resize = false;

            if (this.maximumSize.X < size.X)
            {
                resize = true;
                this.maximumSize.X = size.X;
            }

            if (this.maximumSize.Y < size.Y)
            {
                resize = true;
                this.maximumSize.Y = size.Y;
            }

            if (resize)
            {
                SDL_DestroyTexture(this.RenderTexture);
                this.RenderTexture = createTexture(maximumSize.X, maximumSize.Y);
            }

            this.size = size;

            SizeChanged?.Invoke(this, size);
        }
    }
}