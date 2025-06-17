

using System.Diagnostics;
using static MainClass;
using static System.Math;
using static System.Net.Mime.MediaTypeNames;
using rasterisation;
using Raylib_cs;

class MainClass
{
    // Options
    private const int TARGET_FPS = 75;
    private const int RES_WIDTH = 480;
    private const int RES_HEIGHT = 360;
    
    // Converts ColourBuffer into a format used by raylib's texture
    public static void ToFlatByteArray(RenderTarget target, byte[] textureBytes)
    {
        int width = target.Width;
        int height = target.Height;

        // Assert correct lengths
        if (textureBytes.Length != width * height * 4)
        {
            throw new ArgumentException("textureBytes has an incorrect length!");
        }

        int byteIndex = 0;
        for (int y = height - 1; y >= 0; y--) // write bottom to top
        {
            for (int x = 0; x < width; x++)
            {
                float3 colour = target.ColourBuffer[x, y];

                textureBytes[byteIndex++] = (byte)Math.Clamp(colour.r * 255f, 0, 255);
                textureBytes[byteIndex++] = (byte)Math.Clamp(colour.g * 255f, 0, 255);
                textureBytes[byteIndex++] = (byte)Math.Clamp(colour.b * 255f, 0, 255);
                textureBytes[byteIndex++] = (byte)255; // Alpha
            }
        }
    }

    // Uses raylib to open a window and start the update loop
    public static void Run(RenderTarget target, Scene scene)
    {
        Raylib.InitWindow(target.Width, target.Height, "Rasterisation!");
        Raylib.SetTargetFPS(TARGET_FPS);
        Raylib.HideCursor();
        Texture2D texture = Raylib.LoadTextureFromImage(Raylib.GenImageColor(target.Width, target.Height, Color.Black));
        byte[] textureBytes = new byte[target.Width * target.Height * 4]; // RGBA (i.e. Color is 32 bits)

        // Render Loop
        while (!Raylib.WindowShouldClose())
        {
            // Update & Draw
            scene.Update(target, Raylib.GetFrameTime());
            Rasterizer.Render(target, scene.Data, scene.camera);

            // Write rasterizeration output to a texture and display it on the window
            ToFlatByteArray(target, textureBytes); // Makes my ColourBuffer compatable with raylib's Texture2D
            Raylib.UpdateTexture(texture, textureBytes);
            Raylib.BeginDrawing();
            Raylib.DrawTexture(texture, 0, 0, Color.White);
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }



    static void Main(string[] args)
    {

        RenderTarget target = new(RES_WIDTH, RES_HEIGHT);
        Scene scene = new Scene();

        scene.AddModel("oddfella1.obj", model =>
        {
            model.transform.Position.z = 8f;
            model.transform.Yaw = MathF.PI;
        });
        scene.AddModel("cube.obj", model =>
        {
            model.transform.Position.z = 8f;
            model.transform.Position.x = -1f;
            model.transform.Pitch = MathF.PI / 8f;
        });
        scene.AddModel("suzanne.obj", model =>
        {
            model.transform.Position.z = 12f;
            model.transform.Position.x = 0f;
            model.transform.Yaw = MathF.PI;
        });


        //Rasterizer.Render(target, scene.Data, scene.camera);

        Run(target, scene);
    }
}

