using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rasterisation
{
    public class Transform
    {
        public float Yaw;   // rotation around y-axis
        public float Pitch; // rotation side-to-side
        public float3 Position = new float3(0f, 0f, 0f);

        // calculate the vector to transform by
        public (float3, float3, float3) GetBasisVectors()
        {
            // Yaw
            float3 ihat_yaw = new float3(MathF.Cos(Yaw), 0, MathF.Sin(Yaw));
            float3 jhat_yaw = new float3(0, 1, 0);
            float3 khat_yaw = new float3(-MathF.Sin(Yaw), 0, MathF.Cos(Yaw));

            // Pitch
            float3 ihat_pitch = new float3(1, 0, 0);
            float3 jhat_pitch = new float3(0, MathF.Cos(Pitch), -MathF.Sin(Pitch));
            float3 khat_pitch = new float3(0, MathF.Sin(Pitch), MathF.Cos(Pitch));

            // Combine Yaw + Pitch (first rotate around vertical axis, then rotation around wherever horizontal ended up)
            float3 ihat = TransformVector(ihat_yaw, jhat_yaw, khat_yaw, ihat_pitch);
            float3 jhat = TransformVector(ihat_yaw, jhat_yaw, khat_yaw, jhat_pitch);
            float3 khat = TransformVector(ihat_yaw, jhat_yaw, khat_yaw, khat_pitch);

            return (ihat, jhat, khat);
        }

        public (float3, float3, float3) GetInverseBasisVectors()
        {
            // Yaw
            float3 ihat_yaw = new float3(MathF.Cos(-Yaw), 0, MathF.Sin(-Yaw));
            float3 jhat_yaw = new float3(0, 1, 0);
            float3 khat_yaw = new float3(-MathF.Sin(-Yaw), 0, MathF.Cos(-Yaw));

            // Pitch
            float3 ihat_pitch = new float3(1, 0, 0);
            float3 jhat_pitch = new float3(0, MathF.Cos(-Pitch), -MathF.Sin(-Pitch));
            float3 khat_pitch = new float3(0, MathF.Sin(-Pitch), MathF.Cos(-Pitch));

            // Combine Yaw + Pitch (reverse order: rotate around pitch then yaw)
            float3 ihat = TransformVector(ihat_pitch, jhat_pitch, khat_pitch, ihat_yaw);
            float3 jhat = TransformVector(ihat_pitch, jhat_pitch, khat_pitch, jhat_yaw);
            float3 khat = TransformVector(ihat_pitch, jhat_pitch, khat_pitch, khat_yaw);

            return (ihat, jhat, khat);
        }

        // Move each coordinate of a vector along some basis vector
        private static float3 TransformVector(float3 ihat, float3 jhat, float3 khat, float3 v)
        {
            return ihat * v.x + jhat * v.y + khat * v.z;
        }

        public float3 ToWorldPoint(float3 localPoint)
        {
            (float3 ihat, float3 jhat, float3 khat) = GetBasisVectors();
            return TransformVector(ihat, jhat, khat, localPoint) + Position;
        }

        public float3 ToLocalPoint(float3 worldPoint)
        {
            (float3 ihat, float3 jhat, float3 khat) = GetInverseBasisVectors();
            return TransformVector(ihat, jhat, khat, worldPoint - Position);
        }
    }
}
