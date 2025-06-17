using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rasterisation
{
    // PREVIOUSLY USED TESTS
    // Note:    These are outdated and likely don't work but I am keeping them
    //          here in the event that I want to recreate the 2D testing scenes
    internal class LegacyTests
    {
        public static void CreateTestImage()
        {
            const int width = 64;
            const int height = 64;
            float3[,] image = new float3[width, height];

            // define some points in 2d space
            float2 a = new(0.2f * width, 0.2f * height);
            float2 b = new(0.7f * width, 0.45f * height);
            float2 c = new(0.7f * width, 0.95f * height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float2 p = new(x, y);
                    //bool inside = MathUtils.PointInTriangle(a, b, c, p);
                    //if (inside) image[x, y] = new float3(x / (width - 2f), y / (height - 2f), 0);

                }
            }
            ImageWriter.WriteImageToFile(image, "art.bmp");
        }
        public static void CreateTestImages()
        {
            const int width = 1280;
            const int height = 720;
            float3[,] image = new float3[width, height];

            const int triangleCount = 100;
            float2[] points = new float2[triangleCount * 3];
            float2[] velocities = new float2[points.Length];
            float3[] triangleCols = new float3[triangleCount];

            Random rng = new Random();
            float2 half = new float2(width / 2f, height / 2f);

            // Initalize random points
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = half + (Float2Utils.RandomFloat2(rng, width, height) - half) * 0.3f;
            }

            // Initialize colour and velocities
            for (int i = 0; i < points.Length; i += 3)
            {
                triangleCols[i / 3] = Float3Utils.RandomColour(rng);

                float2 velocity = (Float2Utils.RandomFloat2(rng, width, height) - half) * 0.01f;

                velocities[i] = velocity;
                velocities[i + 1] = velocity;
                velocities[i + 2] = velocity;
            }


            // Run the simulation
            Stopwatch sw;

            const int frames = 500;
            for (int i = 0; i < frames; i++)
            {

                sw = Stopwatch.StartNew();

                // clear the screen
                Array.Clear(image);

                // update positions
                for (int j = 0; j < points.Length; j++)
                {
                    float2 curPos = points[j];
                    if (curPos.x >= width || curPos.x <= 0)
                    {
                        velocities[j].x = -velocities[j].x;
                    }
                    if (curPos.y >= height || curPos.y <= 0)
                    {
                        velocities[j].y = -velocities[j].y;
                    }
                    points[j] = curPos + velocities[j];
                }

                // draw to image
                //Render(points, triangleCols, image);

                // save file
                ImageWriter.WriteImageToFile(image, $"frame-{i}.bmp");

                Console.WriteLine($"Time Elapsed for Frame {i}: {sw.ElapsedMilliseconds} ms");
            }

            //sw.Stop();
        }

    }
}
