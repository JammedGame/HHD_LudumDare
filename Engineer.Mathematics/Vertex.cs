using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Engineer.Mathematics
{
    public enum Axis
    {
        X, Y, Z
    }
    public struct Vertex
    {
        public float X;
        public float Y;
        public float Z;
        public Vertex(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
    }
    public class VertexBuilder
    {
        public float X;
        public float Y;
        public float Z;
        public VertexBuilder(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
            this.Z = 0;
        }
        public VertexBuilder(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        public VertexBuilder(Vertex V)
        {
            X = V.X;
            Y = V.Y;
            Z = V.Z;
        }
        public VertexBuilder(VertexBuilder V)
        {
            X = V.X;
            Y = V.Y;
            Z = V.Z;
        }
        public VertexBuilder Translate(VertexBuilder A)
        {
            this.X += A.X;
            this.Y += A.Y;
            this.Z += A.Z;
            return this;
        }
        public VertexBuilder Scale(VertexBuilder A)
        {
            this.X *= A.X;
            this.Y *= A.Y;
            this.Z *= A.Z;
            return this;
        }
        public VertexBuilder RotateX(float A)
        {
            float OY = Y;
            float OZ = Z;
            Y = (float)(Math.Cos((A / 180) * Math.PI) * OY - Math.Sin((A / 180) * Math.PI) * OZ);
            Z = (float)(Math.Cos((A / 180) * Math.PI) * OZ + Math.Sin((A / 180) * Math.PI) * OY);
            return this;
        }
        public VertexBuilder RotateY(float A)
        {
            float OX = X;
            float OZ = Z;
            X = (float)(Math.Cos((A / 180) * Math.PI) * OX + Math.Sin((A / 180) * Math.PI) * OZ);
            Z = (float)(Math.Cos((A / 180) * Math.PI) * OZ - Math.Sin((A / 180) * Math.PI) * OX);
            return this;
        }
        public VertexBuilder RotateZ(float A)
        {
            float OX = X;
            float OY = Y;
            X = (float)(Math.Cos((A / 180) * Math.PI) * OX - Math.Sin((A / 180) * Math.PI) * OY);
            Y = (float)(Math.Cos((A / 180) * Math.PI) * OY + Math.Sin((A / 180) * Math.PI) * OX);
            return this;
        }
        public static VertexBuilder operator +(VertexBuilder a, VertexBuilder b)
        {
            return new VertexBuilder(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static VertexBuilder operator +(VertexBuilder a, Vertex b)
        {
            return new VertexBuilder(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static VertexBuilder operator -(VertexBuilder a, VertexBuilder b)
        {
            return new VertexBuilder(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        public static VertexBuilder operator -(VertexBuilder a, Vertex b)
        {
            return new VertexBuilder(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        public static VertexBuilder operator *(VertexBuilder a, VertexBuilder b)
        {
            return new VertexBuilder(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }
        public static VertexBuilder operator *(VertexBuilder a, Vertex b)
        {
            return new VertexBuilder(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }
        public static VertexBuilder operator *(VertexBuilder a, float b)
        {
            return new VertexBuilder(a.X * b, a.Y * b, a.Z * b);
        }
        public Vertex ToVertex()
        {
            return new Vertex(X, Y, Z);
        }
        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }
        public VertexBuilder Normalize()
        {
            float Divider = 1.0f / this.Length();
            VertexBuilder VB = new VertexBuilder(this);
            VB.X *= Divider;
            VB.Y *= Divider;
            VB.Z *= Divider;
            return VB;
        }
        public static Vertex FromRGB(int R, int G, int B)
        {
            return new Vertex((R * 1.0f + 1) / 256, (G * 1.0f + 1) / 256, (B * 1.0f + 1) / 256);
        }
        public static VertexBuilder Cross(VertexBuilder Left, VertexBuilder Right)
        {
            return new VertexBuilder(Left.Y * Right.Z - Left.Z * Right.Y,
                                    Left.Z * Right.X - Left.X * Right.Z,
                                    Left.X * Right.Y - Left.Y * Right.X);
        }
    }
}
