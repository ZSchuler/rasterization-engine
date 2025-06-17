using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rasterisation
{
    public struct float2(float x, float y)
    {
        public float x = x;
        public float y = y;

        // Operator Overflows
        public static float2 operator +(float2 a, float2 b) => new float2(a.x + b.x, a.y + b.y);
        public static float2 operator -(float2 a, float2 b) => new float2(a.x - b.x, a.y - b.y);
        public static float2 operator *(float2 a, float c) => new float2(a.x * c, a.y * c);
        public static float2 operator /(float2 a, float c) => new float2(a.x / c, a.y / c);
    }

    public static class Float2Utils
    {
        public static float2 RandomFloat2(Random rng, int width, int height)
        {
            float2 p = new float2();
            p.x = rng.Next(0, width);
            p.y = rng.Next(0, height);
            return p;
        }
    }
}
