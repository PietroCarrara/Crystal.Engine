using System;
using Crystal.Framework.Graphics;
using Crystal.Engine.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine
{
    public static class IRenderTargetExtensions
    {
        public static RenderTarget2D ToMonoGame(this IRenderTarget self)
        {
            if (self is CrystalCanvas crystalCanvas)
            {
                return crystalCanvas.RenderTarget;
            }

            throw new Exception("Could not cast IRenderTarget to RenderTarget2D!");
        }
    }
}