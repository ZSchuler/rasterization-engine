using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rasterisation
{
    public static class MathUtils
    {
        // Calculates the dot product of two 2d vectors
        public static float Dot(float2 a, float2 b) => a.x * b.x + a.y * b.y;

        // Calculates the vector that is perpendicular (90 degrees clockwise from vector)
        public static float2 Perpendicular(float2 vec) => new float2(vec.y, -vec.x);

        
        // The area of any triangle ABC (clockwise = positive)
        // This is also used to determine if a point is on the right side of a line
        public static float SignedTriangleArea(float2 a, float2 b, float2 c)
        {
            float2 ac = c - a;
            float2 abPerp = Perpendicular(b - a);
            return Dot(ac, abPerp) / 2;
        }

        // Determines if a point p is within an ABC triangle
        // Non-clockwise triangles are "back-faces" and are ignored
        public static bool PointInTriangle(float2 a, float2 b, float2 c, float2 p, out float3 weights)
        {
            float areaABP = SignedTriangleArea(a, b, p);
            float areaBCP = SignedTriangleArea(b, c, p);
            float areaCAP = SignedTriangleArea(c, a, p);
            bool inTriangle = areaABP >= 0 && areaBCP >= 0 && areaCAP >= 0; // check if all clockwise

            // barycentric coordinates
            float totalArea = areaABP + areaBCP + areaCAP;
            float inverseAreaSum = 1f / totalArea; // normalizing factor, so total adds to one
            float weightA = areaBCP * inverseAreaSum;
            float weightB = areaCAP * inverseAreaSum;
            float weightC = areaABP * inverseAreaSum;

            weights = new float3(weightA, weightB, weightC);
            
            return inTriangle && totalArea > 0; // to avoid divide by zero
        }

        // Maps a 3D point onto the 2D screen
        public static float3 VertexToScreen(float3 vertex, Transform transform, Camera camera, float2 numPixels)
        {
            float3 vertexInWorld = transform.ToWorldPoint(vertex);
            float3 vertexView = camera.Transform.ToLocalPoint(vertexInWorld);

            float worldScreenHeight = MathF.Tan(camera.Fov / 2f) * 2f;
            float pixelsPerWorldUnit = numPixels.y / worldScreenHeight / vertexView.z; // "meters" to pixels
            
            float2 pixelOffset = new float2(vertexView.x, vertexView.y) * pixelsPerWorldUnit;
            float2 vertexScreen = numPixels / 2 + pixelOffset; // make 0,0 map to center
            return new float3(vertexScreen.x, vertexScreen.y, vertexView.z);
        }

        // Converts degrees to radians
        public static float ToRadians(float degrees) => degrees * MathF.PI / 180f;
    }
}
