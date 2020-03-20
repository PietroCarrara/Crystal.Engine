using System;
using Microsoft.Xna.Framework;
using Crystal.Framework.Graphics;
using Crystal.Engine.Graphics.Scalers;

namespace Crystal.Engine.Graphics
{
    public static class ScalerFactory
    {
        public static IScaler FromStrategy(ScaleStrategy strat, CrystalGame game)
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