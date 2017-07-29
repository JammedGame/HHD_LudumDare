using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Engineer.IO;
using Engineer.Mathematics;

namespace Engineer.Data
{
    public class OBJContainer : MeshContainer
    {
        private char[] _WhiteSpaces = { ' ', '\t' };
        private string[] CleanStringArray(string[] Array)
        {
            List<string> NewArray = new List<string>();
            for (int i = 0; i < Array.Length; i++)
            {
                if (Array[i] != "") NewArray.Add(Array[i]);
            }
            return NewArray.ToArray();
        }
        public override void Load(string FilePath, BackgroundWorker Worker)
        {
            FileStream Stream = new FileStream(FilePath, FileMode.Open);
            StreamReader Reader = new StreamReader(Stream);
            Geometry NewGeometry = null;
            while (!Reader.EndOfStream)
            {
                string Line = Reader.ReadLine();
                string[] Parts = CleanStringArray(Line.Split(_WhiteSpaces));
                if (Line.StartsWith("#")) continue;
                if (Line.StartsWith("g") || Line.StartsWith("o"))
                {
                    if (NewGeometry != null) this.Geometries.Add(NewGeometry);
                    NewGeometry = new Geometry(Parts[1]);
                    continue;
                }
                if (Line.StartsWith("vn"))
                {
                    NewGeometry.Normals.Add(new Mathematics.Vertex(Converter.ToSingle(Parts[1]), Converter.ToSingle(Parts[2]), Converter.ToSingle(Parts[3])));
                    continue;
                }
                if (Line.StartsWith("vt"))
                {
                    NewGeometry.TexCoords.Add(new Mathematics.Vertex(Converter.ToSingle(Parts[1]), Converter.ToSingle(Parts[2]), 0));
                    continue;
                }
                if (Line.StartsWith("v"))
                {
                    if (NewGeometry.Vertices == null) NewGeometry.Vertices = new List<Vertex>();
                    NewGeometry.Vertices.Add(new Mathematics.Vertex(Converter.ToSingle(Parts[1]), Converter.ToSingle(Parts[2]), Converter.ToSingle(Parts[3])));
                    continue;
                }
                if (Line.StartsWith("f"))
                {
                    if (Parts.Length == 5)
                    {
                        Mathematics.Face NewFace1 = new Mathematics.Face();
                        Mathematics.Face NewFace2 = new Mathematics.Face();
                        for (int i = 1; i < Parts.Length; i++)
                        {
                            string[] SubParts = Parts[i].Split('/');
                            if(i == 1 || i == 2 || i == 4) NewFace1.Vertices.Add(Math.Abs(Convert.ToInt32(SubParts[0])) - 1);
                            if (i == 2 || i == 3 || i == 4) NewFace2.Vertices.Add(Math.Abs(Convert.ToInt32(SubParts[0])) - 1);
                            if (SubParts.Length > 1 && SubParts[1] != "")
                            {
                                if (i == 1 || i == 2 || i == 4) NewFace1.TexCoords.Add(Math.Abs(Convert.ToInt32(SubParts[1])) - 1);
                                if (i == 2 || i == 3 || i == 4) NewFace2.TexCoords.Add(Math.Abs(Convert.ToInt32(SubParts[1])) - 1);
                            }
                            if (SubParts.Length > 2)
                            {
                                if (i == 1 || i == 2 || i == 4) NewFace1.Normals.Add(Math.Abs(Convert.ToInt32(SubParts[2])) - 1);
                                if (i == 2 || i == 3 || i == 4) NewFace2.Normals.Add(Math.Abs(Convert.ToInt32(SubParts[2])) - 1);
                            }
                        }
                        NewGeometry.Faces.Add(NewFace1);
                        NewGeometry.Faces.Add(NewFace2);
                    }
                    else
                    {
                        Mathematics.Face NewFace = new Mathematics.Face();
                        for (int i = 1; i < Parts.Length; i++)
                        {
                            string[] SubParts = Parts[i].Split('/');
                            NewFace.Vertices.Add(Math.Abs(Convert.ToInt32(SubParts[0])) - 1);
                            if (SubParts.Length > 1) NewFace.TexCoords.Add(Math.Abs(Convert.ToInt32(SubParts[1])) - 1);
                            if (SubParts.Length > 2) NewFace.Normals.Add(Math.Abs(Convert.ToInt32(SubParts[2])) - 1);
                        }
                        NewGeometry.Faces.Add(NewFace);
                    }
                    continue;
                }
            }
            this.Geometries.Add(NewGeometry);
            Reader.Close();
        }
        public override void Save(string FilePath, BackgroundWorker Worker)
        {
            FileStream Stream = new FileStream(FilePath, FileMode.Create);
            StreamWriter Writer = new StreamWriter(Stream);
            Writer.WriteLine("# WaveFront *.obj file (Generated by Engineer)");
            for(int i = 0; i < this.Geometries.Count; i++)
            {
                Writer.WriteLine("");
                Writer.WriteLine("g " + Geometries[i].Name);
                for(int j = 0; j < Geometries[i].Vertices.Count; j++)
                {
                    Writer.WriteLine("v " + Geometries[i].Vertices[j].X + " " + Geometries[i].Vertices[j].Y + " " + Geometries[i].Vertices[j].Z);
                }
                for (int j = 0; j < Geometries[i].TexCoords.Count; j++)
                {
                    Writer.WriteLine("vt " + Geometries[i].TexCoords[j].X + " " + Geometries[i].TexCoords[j].Y + " " + Geometries[i].TexCoords[j].Z);
                }
                for (int j = 0; j < Geometries[i].Normals.Count; j++)
                {
                    Writer.WriteLine("vn " + Geometries[i].Normals[j].X + " " + Geometries[i].Normals[j].Y + " " + Geometries[i].Normals[j].Z);
                }
                for (int j = 0; j < Geometries[i].Faces.Count; j++)
                {
                    string Written = "f ";
                    for(int k = 0; k < Geometries[i].Faces[j].Vertices.Count; k++)
                    {
                        if (k > 0) Written += " ";
                        Written += Geometries[i].Faces[j].Vertices[k] + "/" + Geometries[i].Faces[j].TexCoords[k] + "/" + Geometries[i].Faces[j].Normals[k];
                    }
                    Writer.WriteLine(Written);
                }
            }
            Writer.Close();
        }
        public void Repack()
        {
            for(int i = 0; i < this.Geometries.Count; i++)
            {
                List<Vertex> NewVertices = new List<Vertex>();
                List<Vertex> NewNormals = new List<Vertex>();
                List<Vertex> NewTexCoords = new List<Vertex>();
                for (int j = 0; j < this.Geometries[i].Faces.Count; j++)
                {
                    for (int k = 0; k < this.Geometries[i].Faces[j].Vertices.Count; k++)
                    {
                        NewVertices.Add(this.Geometries[i].Vertices[this.Geometries[i].Faces[j].Vertices[k]]);
                        this.Geometries[i].Faces[j].Vertices[k] = NewVertices.Count - 1;
                    }
                    for (int k = 0; k < this.Geometries[i].Faces[j].Normals.Count; k++)
                    {
                        NewNormals.Add(this.Geometries[i].Normals[this.Geometries[i].Faces[j].Normals[k]]);
                        this.Geometries[i].Faces[j].Normals[k] = NewNormals.Count - 1;
                    }
                    for (int k = 0; k < this.Geometries[i].Faces[j].TexCoords.Count; k++)
                    {
                        if (this.Geometries[i].TexCoords.Count == 0) break;
                        NewTexCoords.Add(this.Geometries[i].TexCoords[this.Geometries[i].Faces[j].TexCoords[k]]);
                        this.Geometries[i].Faces[j].TexCoords[k] = NewTexCoords.Count - 1;
                    }
                }
                this.Geometries[i].Vertices = NewVertices;
                this.Geometries[i].Normals = NewNormals;
                this.Geometries[i].TexCoords = NewTexCoords;
            }
        }
    }
}
