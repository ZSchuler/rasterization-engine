using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rasterisation
{
    // Loads an object from a OBJ file
    // This is pretty inefficent but you only need to load each once
    public static class ObjLoader
    {
        public static float3[] LoadObjFile(string filename)
        {
            try
            {
                StreamReader sr = new StreamReader(filename);

                List<String> lines = new();
                String? line = sr.ReadLine();
                while (line != null)
                {
                    lines.Add(line);
                    line = sr.ReadLine();
                }
                sr.Close();

                List<float3> allPoints = new();
                List<float3> trianglePoints = new();

                foreach (String l in lines)
                {
                    if (l.StartsWith("v ")) // vertex position
                    {
                        // takes the 3 numbers from the line, converts each to float, and puts into a list
                        float[] vertex = l[2..].Split(' ').Select(float.Parse).ToArray();
                        allPoints.Add(new float3(vertex[0], vertex[1], vertex[2]));
                    }
                    else if (l.StartsWith("f ")) // face indices
                    {
                        // obj polygonal face elements are formatted vertex_index/texture_index/normal_index
                        // e.g. 1/1/1 2/4/1 4/9/1 3/7/1, so we foremost care about the first digits of each

                        string[] faceIndexes = l[2..].Split(' ');

                        for (int i = 0; i < faceIndexes.Length; i++)
                        {
                            int[] index = faceIndexes[i].Split('/').Select(int.Parse).ToArray();
                            int pointIndex = index[0] - 1; // get the first digit, subtract 1 as obj indices start at 1

                            // if a face has more than 3 points (not a triangle),
                            // make a triangle for each additional point using the VERY FIRST point
                            // and the last point of the previous triangle
                            if (i >= 3) trianglePoints.Add(trianglePoints[^(3 * i - 6)]); // 3*i - 6 last item (first from this face)
                            if (i >= 3) trianglePoints.Add(trianglePoints[^2]); // second last item
                            trianglePoints.Add(allPoints[pointIndex]);

                        }
                    }
                }

                return trianglePoints.ToArray();
            }
            catch
            {
                Console.Error.WriteLine("Error opening obj file!");
                return null;
            }
        }
    }
}
