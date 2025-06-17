using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rasterisation
{
    public struct float3(float x, float y, float z)
    {
        public float x = x;
        public float y = y;
        public float z = z;

        // maps r,g,b to x,y,z so that float3 can also be used for colours
        public float r { get => x; set => x = value; }
        public float g { get => y; set => y = value; }
        public float b { get => z; set => z = value; }

        // Operator Overloads
        public static float3 operator +(float3 a, float3 b) => new float3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static float3 operator -(float3 a, float3 b) => new float3(a.x - b.x, a.y - b.y, a.z - b.z);
        public static float3 operator *(float3 a, float c) => new float3(a.x * c, a.y * c, a.z * c);
        public static explicit operator float2(float3 p) => new float2(p.x, p.y);
    }

    public static class Float3Utils
    {
        public static float3 RandomColour(Random rng) => new float3(rng.NextSingle(), rng.NextSingle(), rng.NextSingle());
        public static float Dot(float3 a, float3 b) => (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        public static float3 Normalize(float3 a)
        {
            float3 normalized = new(0, 0, 0);
            float mag = MathF.Sqrt((a.x * a.x) + (a.y * a.y) + (a.z * a.z));

            if (mag != 0)
            {
                normalized.x = a.x / mag;
                normalized.y = a.y / mag;
                normalized.z = a.z / mag;
            }

            return normalized;
        }
    }
}
