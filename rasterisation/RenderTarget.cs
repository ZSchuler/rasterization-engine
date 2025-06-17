using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rasterisation
{
    // Tracks the screen size, colourbuffer, and render buffer
    public class RenderTarget(int w, int h)
    {
        public readonly float3[,] ColourBuffer = new float3[w, h];
        public readonly float[,] DepthBuffer = new float[w, h];

        public readonly int Width = w;
        public readonly int Height = h;
        public readonly float2 Size = new(w, h);
    }
}
