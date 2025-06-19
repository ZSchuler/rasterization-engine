# Software Rasterizer

This project is a simple software rasterizer written in C#, using [Raylib](https://www.raylib.com/) for window management and input. It demonstrates rendering 3D models using a custom CPU-based rasterization pipeline.  

Special thanks to Sebastian Lague for his excellent [YouTube video](https://www.youtube.com/watch?v=yyJ-hdISgnw), which served as a major source of inspiration for this project.

### Features

- Triangle rasterization with barycentric coordinates
- Depth buffering (Z-buffer)
- Transformations: translation, rotation (yaw, pitch)
- Integration with Raylib for real-time display

### Example Output

![Example Output](example.gif)

### Getting Started

To run the project, you'll need to:

- Install [.NET SDK](https://dotnet.microsoft.com/)
- Add Raylib to your project via NuGet
- Add your `.obj` models in the working directory


### Usage

You can add models to the scene and manipulate their transform via the `AddModel` function:

```csharp
scene.AddModel("cube.obj", model =>
{
    model.transform.Position.z = 8f;              // Move the model forward
    model.transform.Position.x = -1f;             // Move the model left
    model.transform.Pitch = MathF.PI / 8f;        // Rotate the model slightly upward
});
```
### Controls
- Mouse Movement: Controls the camera's pitch and yaw.
- W / A / S / D: Move the camera forward, left, backward, and right.
- The mouse is locked to the center of the screen each frame to enable smooth free-look navigation.
- The camera's vertical movement is clamped to avoid flipping over.

### Entry Point

The main render loop is started via:
```
RenderTarget target = new(480, 360);
Scene scene = new Scene();

// Add models...
Run(target, scene);
```

### Future Plans

- Optimize to run at least 60 FPS
- Add better solution for negative-z triangles
- Add support for textures & simple lighting