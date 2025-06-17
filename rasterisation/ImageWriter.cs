using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rasterisation
{
    // Given a ColourBuffer, will save the array into a .bmp image file
    public static class ImageWriter
    {
        public static void WriteImageToFile(float3[,] image, String filename)
        {
            using BinaryWriter writer = new(File.Open(filename, FileMode.Create));
            uint[] byteCounts = { 14, 40, (uint)image.Length * 4 }; // BMP header, DIP header, data

            // Headers
            writer.Write("BM"u8.ToArray()); // BMP header start (id field)
            writer.Write(byteCounts[0] + byteCounts[1] + byteCounts[2]); // total file size
            writer.Write((uint)0); // unused
            writer.Write(byteCounts[0] + byteCounts[1]); // data offset

            writer.Write(byteCounts[1]); // DIP header size
            writer.Write((uint)image.GetLength(0)); // image array width
            writer.Write((uint)image.GetLength(1)); // image array height
            writer.Write((ushort)1); // number of colour planes?
            writer.Write((ushort)(8 * 4)); // number of bits per pixel (1 byte per channel (rgb), 1 for aligment)
            writer.Write((uint)0); // no pixel array compression
            writer.Write(byteCounts[2]); // size of image array
            writer.Write(new byte[16]); // print resolution and palette info (ignoring for now)

            // Data
            for (int y = 0; y < image.GetLength(1); y++)
            {
                for (int x = 0; x < image.GetLength(0); x++)
                {
                    // BMP files like to do it b,g,r order for some reason
                    float3 col = image[x, y];
                    writer.Write((byte)(col.b * 255));
                    writer.Write((byte)(col.g * 255));
                    writer.Write((byte)(col.r * 255));
                    writer.Write((byte)0); // padding
                }
            }
        }
    }
}
