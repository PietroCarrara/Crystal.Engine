using System;
using Microsoft.Xna.Framework;
using Crystal.Engine.Graphics.Scalers;

namespace Crystal.Engine.Graphics
{
    public interface ICrystalScaler
    {
        /// <summary>
        /// Scales a texture to fit the screen
        /// </summary>
        /// <param name="width">The width of the texture to be scaled</param>
        /// <param name="height">Height of the texture to be rendered</param>
        /// <returns>The rectangle where the texture should be rendered</returns>
        Rectangle Scale(int width, int height);
    }

    public static class CrystalScalerFactory
    {
        public static ICrystalScaler FromStrategy(ScaleStrategy strat, CrystalGame game)
        {
            switch (strat)
            {
                case ScaleStrategy.LetterBoxing:
                    return new LetterboxingScaler(game);
            }

            throw new Exception("Unknown scaling strategy!");
        }
    }

    public enum ScaleStrategy
    {
        LetterBoxing
    }
}