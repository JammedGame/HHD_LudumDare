using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Engineer.Mathematics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engineer.Draw.OpenGL.FixedGL
{
    public class GLRenderer : Renderer
    {
        public override void SetViewport(int Width, int Height)
        {
            GL.Viewport(0, 0, Width, Height);
        }
        public override void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
        public override void ClearColor(float[] Color)
        {
            GL.ClearColor(Color[0], Color[1], Color[2], Color[3]);
        }
        public override void SetSurface(float[] Color)
        {
            GL.Color3(Color);
        }
        public override void SetProjectionMatrix(float[] Matrix)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(Matrix);
        }
        public override void SetModelViewMatrix(float[] Matrix)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(Matrix);
        }
        public override void RenderGeometry(List<Vertex> Vertices, List<Vertex> Normals, List<Vertex> TexCoords, List<Face> Faces, bool Update)
        {
            if (Faces[0].Vertices.Count > 3) GL.Begin(PrimitiveType.Quads);
            else GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < Faces.Count; i++)
            {
                for (int j = 0; j < Faces[i].Vertices.Count; j++)
                {
                    GL.Normal3(Normals[Faces[i].Normals[j]].X, Normals[Faces[i].Normals[j]].Y, Normals[Faces[i].Normals[j]].Z);
                    GL.TexCoord3(TexCoords[Faces[i].TexCoords[j]].X, TexCoords[Faces[i].TexCoords[j]].Y, TexCoords[Faces[i].TexCoords[j]].Z);
                    GL.Vertex3(Vertices[Faces[i].Vertices[j]].X, Vertices[Faces[i].Vertices[j]].Y, Vertices[Faces[i].Vertices[j]].Z);
                }
            }
            GL.End();
        }
    }
}
