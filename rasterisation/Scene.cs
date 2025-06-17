using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using Raylib_cs;

namespace rasterisation
{
    // Responsible for loading models, and moving them around each frame
    public class Scene
    {
        public readonly List<Model> Data = new List<Model>();
        public Camera camera = new Camera();

        private readonly Random rng = new(69);

        public void AddModel(string filePath, Action<Model> configure = null)
        {
            float3[] points = ObjLoader.LoadObjFile(filePath);
            float3[] triangleCols = new float3[points.Length / 3];

            for (int i = 0; i < triangleCols.Length; i++)
            {
                triangleCols[i] = Float3Utils.RandomColour(rng);
            }

            Model model = new Model(points, triangleCols);
            configure?.Invoke(model);
            Data.Add(model);
        }

        private float _angle = 0f;
        public void Update(RenderTarget target, float deltaTime)
        {
            // Work per frame
            //Console.WriteLine($"Frame: {deltaTime * 1000f:F2}ms");

            // clear the array
            Array.Clear(target.ColourBuffer);

            Data[0].transform.Yaw += 20f * MathF.PI / 18f * deltaTime;

            float radius = 2f;
            Data[1].transform.Yaw += 10f * MathF.PI / 18f * deltaTime;
            Data[1].transform.Pitch += 10f * MathF.PI / 18f * deltaTime;
            Data[1].transform.Position.x = MathF.Sin(_angle) * radius;
            Data[1].transform.Position.y = MathF.Cos(_angle) * radius;

            Data[2].transform.Position.x = MathF.Sin(_angle) * 8f;

            _angle += deltaTime * radius;


            // Mouse Movement
            const float mouseSensitivity = 5f;
            float2 mouseDelta;
            mouseDelta.x = Raylib.GetMouseDelta().X / target.Width * mouseSensitivity;
            mouseDelta.y = Raylib.GetMouseDelta().Y / target.Width * mouseSensitivity;
            camera.Transform.Pitch = Clamp(camera.Transform.Pitch - mouseDelta.y, MathUtils.ToRadians(-85), MathUtils.ToRadians(85));
            camera.Transform.Yaw -= mouseDelta.x;
            
            Raylib.SetMousePosition(target.Width / 2, target.Height / 2); // "locks" mouse to center of the screen

            // Camera Movement
            const float cameraSpeed = 5f;
            float3 moveDelta = new float3(0, 0, 0);
            (float3 cameraRight, float3 cameraUp, float3 cameraForward) = camera.Transform.GetBasisVectors();

            if (Raylib.IsKeyDown(KeyboardKey.W)) moveDelta += cameraForward;
            if (Raylib.IsKeyDown(KeyboardKey.S)) moveDelta -= cameraForward;
            if (Raylib.IsKeyDown(KeyboardKey.A)) moveDelta -= cameraRight;
            if (Raylib.IsKeyDown(KeyboardKey.D)) moveDelta += cameraRight;

            camera.Transform.Position += Float3Utils.Normalize(moveDelta) * cameraSpeed * deltaTime;
            camera.Transform.Position.y = 1;
        }
    }
}
