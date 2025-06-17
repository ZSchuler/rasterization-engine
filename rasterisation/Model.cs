using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rasterisation
{
    // Holds a model's vertex points and transform info
    public class Model(float3[] points, float3[] colours)
    {
        public readonly float3[] Points = points;
        public readonly float3[] TriangleColours = colours;
        public Transform transform = new Transform();
    }
}
