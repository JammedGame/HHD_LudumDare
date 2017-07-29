using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Mathematics
{
    public class VertexTransformer
    {
        public static VertexBuilder CrossProduct(Vertex V1, Vertex V2)
        {
            return new VertexBuilder(V1.Y * V2.Z - V1.Z * V2.Y, V1.Z * V2.X - V1.X * V2.Z, V1.X * V2.Y - V1.Y * V2.X);
        }
        public static VertexBuilder LaGrangeProduct(Vertex V1, Vertex V2, Vertex V3)
        {
            VertexBuilder V1B = new VertexBuilder(V1);
            VertexBuilder V2B = new VertexBuilder(V2);
            VertexBuilder V3B = new VertexBuilder(V3);
            //V2 X (V1*V3) - V3 X (V1*V2)
            return CrossProduct(V2, (V1B * V3B).ToVertex()) - CrossProduct(V3, (V1B * V2B).ToVertex());
        }
        public static VertexBuilder CalculateNormal(Vertex[] Vertices)
        {
            if (Vertices.Length == 2) return Normalize(CrossProduct(Vertices[0], Vertices[1]).ToVertex());
            //return LaGrangeProduct(Vertices[0], Vertices[1], Vertices[2]);
            VertexBuilder V0 = new VertexBuilder(Vertices[0]);
            VertexBuilder V1 = new VertexBuilder(Vertices[1]);
            VertexBuilder V2 = new VertexBuilder(Vertices[2]);
            VertexBuilder Z0 = new VertexBuilder(0,0,0);
            VertexBuilder Z1 = new VertexBuilder(0,0,0);
            V0 *= -1;
            Z0 = V1 + V0;
            Z1 = V2 + V0;
            return CrossProduct(Z0.ToVertex(), Z1.ToVertex());
        }
        public static VertexBuilder Average(Vertex[] Vertices)
        {
            VertexBuilder Average = new VertexBuilder(0, 0, 0);
            for(int i = 0; i < Vertices.Length; i++)
            {
                Average.X += Vertices[i].X;
                Average.Y += Vertices[i].Y;
                Average.Z += Vertices[i].Z;
            }
            return Average *= 1.0f/Vertices.Length;
        }
        public static VertexBuilder Normalize(Vertex V)
        {
            return new VertexBuilder(V).Normalize();
        }
        public static float Angle(Vertex V1, Vertex V2)
        {
            double V1V = Math.Sqrt(V1.X * V1.X + V1.Y * V1.Y + V1.Z * V1.Z);
            double V2V = Math.Sqrt(V2.X * V2.X + V2.Y * V2.Y + V2.Z * V2.Z);
            double UP = (V1.X * V2.X + V1.Y * V2.Y + V1.Z * V2.Z);
            double Cos = UP / (V1V + V2V);
            return (float)Math.Asin(Cos);
        }
    }
}
