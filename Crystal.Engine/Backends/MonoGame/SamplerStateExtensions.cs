using System;
using Microsoft.Xna.Framework.Graphics;
using CrystalSampler = Crystal.Framework.Graphics.SamplerState;

namespace Crystal.Engine
{
    public static class SamplerStateExtensions
    {
        public static SamplerState ToMonogame(this CrystalSampler self)
        {
            switch(self)
            {
                case CrystalSampler.AnisotropicClamp:
                    return SamplerState.AnisotropicClamp;
                case CrystalSampler.AnisotropicWrap:
                    return SamplerState.AnisotropicWrap;
                case CrystalSampler.LinearClamp:
                    return SamplerState.LinearClamp;
                case CrystalSampler.LinearWrap:
                    return SamplerState.LinearWrap;
                case CrystalSampler.PointClamp:
                    return SamplerState.PointClamp;
                case CrystalSampler.PointWrap:
                    return SamplerState.PointWrap;
            }

            throw new Exception("Unknown SamplerState!");
        }
    }
}