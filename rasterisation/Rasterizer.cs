using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace rasterisation
{
    // Takes in the data from the scene (list of models), and renders it
    public class Rasterizer
    {
        public static void Render(RenderTarget target, List<Model> models, Camera camera)
        {
            // Reset the depth buffer
            for (int y = 0; y < target.Height; y++)
            {
                for (int x = 0; x < target.Width; x++)
                {
                    target.DepthBuffer[x, y] = float.PositiveInfinity;
                }
            }

            foreach (Model model in models)
            {
                
                // For every triangle
                for (int i = 0; i < model.Points.Length; i += 3)
                {
                    float3 a = MathUtils.VertexToScreen(model.Points[i], model.transform, camera, target.Size);
                    float3 b = MathUtils.VertexToScreen(model.Points[i + 1], model.transform, camera, target.Size);
                    float3 c = MathUtils.VertexToScreen(model.Points[i + 2], model.transform, camera, target.Size);

                    if (a.z <= 0 || b.z <= 0 || c.z <= 0) continue; // temporary fix


                    // Get the bounds (i.e. the "sub-square" that the triangle is within)
                    float minX = Min(Min(a.x, b.x), c.x);
                    float minY = Min(Min(a.y, b.y), c.y);
                    float maxX = Max(Max(a.x, b.x), c.x);
                    float maxY = Max(Max(a.y, b.y), c.y);

                    // Create said "sub-square"
                    int squareStartX = Clamp((int)minX, 0, target.Width - 1);
                    int squareStartY = Clamp((int)minY, 0, target.Height - 1);
                    int squareEndX = Clamp((int)Ceiling(maxX), 0, target.Width - 1);
                    int squareEndY = Clamp((int)Ceiling(maxY), 0, target.Height - 1);

                    // Draw only within the bounds of the "sub-square"
                    for (int y = squareStartY; y <= squareEndY; y++)
                    {
                        for (int x = squareStartX; x <= squareEndX; x++)
                        {
                            float2 p = new float2(x, y);
                            float3 weights;
                            
                            if (MathUtils.PointInTriangle((float2)a, (float2)b, (float2)c, p, out weights))
                            {
                                // sum the weighted depths at each vertex to get depth for current pixel
                                float3 depths = new float3(a.z, b.z, c.z);
                                float depth = Float3Utils.Dot(depths, weights); // ie, multiply depth at each corner and sum
                                if (depth > target.DepthBuffer[x, y]) continue;

                                
                                // If you wish to display the depth map
                                /*
                                float minDepth = 3.5f; // Closest a model will be
                                float maxDepth = 6.5f; // Furthest a model will be
                                float norm = (depth - minDepth) / (maxDepth - minDepth);
                                norm = Clamp(norm, 0f, 1f);
                                norm *= 255f;
                                norm = Clamp(norm, 0f, 255f);
                                float3 depthColour = new float3((int)norm, (int)norm, (int)norm);
                                target.ColourBuffer[x, y] = depthColour;
                                */

                                target.ColourBuffer[x, y] = model.TriangleColours[i / 3];
                                target.DepthBuffer[x, y] = depth;
                            }
                            
                        }
                    }
                }
            }
        }
    }
}
