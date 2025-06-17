using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rasterisation
{
    public class Camera
    {
        public float Fov = MathUtils.ToRadians(60f);
        public Transform Transform = new();
    }
}
