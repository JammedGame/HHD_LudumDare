using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engineer.Mathematics;

namespace Engineer.Data
{
    public class Geometry
    {
        private string _Name;
        private List<Vertex> _Vertices;
        private List<Vertex> _Normals;
        private List<Vertex> _TexCoords;
        private List<Face> _Faces;
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
            }
        }
        public List<Vertex> Vertices
        {
            get
            {
                return _Vertices;
            }

            set
            {
                _Vertices = value;
            }
        }
        public List<Vertex> Normals
        {
            get
            {
                return _Normals;
            }

            set
            {
                _Normals = value;
            }
        }
        public List<Vertex> TexCoords
        {
            get
            {
                return _TexCoords;
            }

            set
            {
                _TexCoords = value;
            }
        }
        public List<Face> Faces
        {
            get
            {
                return _Faces;
            }

            set
            {
                _Faces = value;
            }
        }
        public Geometry()
        {
            this._Name = Guid.NewGuid().ToString();
            this._Vertices = new List<Vertex>();
            this._Normals = new List<Vertex>();
            this._TexCoords = new List<Vertex>();
            this._Faces = new List<Face>();
        }
        public Geometry(string Name)
        {
            this._Name = Name;
            this._Vertices = new List<Vertex>();
            this._Normals = new List<Vertex>();
            this._TexCoords = new List<Vertex>();
            this._Faces = new List<Face>();
        }
        public Geometry(Geometry G)
        {
            this._Name = G._Name;
            this._Vertices = new List<Vertex>();
            this._Normals = new List<Vertex>();
            this._TexCoords = new List<Vertex>();
            this._Faces = new List<Face>();
            for (int i = 0; i < G._Vertices.Count; i++) this._Vertices.Add(new Vertex(G._Vertices[i].X, G._Vertices[i].Y, G._Vertices[i].Z));
            for (int i = 0; i < G._Normals.Count; i++) this._Normals.Add(new Vertex(G._Normals[i].X, G._Normals[i].Y, G._Normals[i].Z));
            for (int i = 0; i < G._TexCoords.Count; i++) this._TexCoords.Add(new Vertex(G._TexCoords[i].X, G._TexCoords[i].Y, G._TexCoords[i].Z));
            for (int i = 0; i < G._Faces.Count; i++) this._Faces.Add(new Face(G._Faces[i]));
        }
        public void RecalculateNormals()
        {
            this._Normals = new List<Vertex>();
            List<Vertex>[] Normals = new List<Vertex>[_Vertices.Count];
            for (int i = 0; i < this._Faces.Count; i++)
            {
                List<Vertex> Vertices = new List<Vertex>();
                for (int j = 0; j < this._Faces[i].Vertices.Count; j++)
                {
                    Vertices.Add(this._Vertices[Faces[i].Vertices[j]]);
                }
                Vertex Normal = (VertexTransformer.Normalize(VertexTransformer.CalculateNormal(Vertices.ToArray()).ToVertex())).ToVertex();
                Faces[i].Normals.Clear();
                for (int j = 0; j < this._Faces[i].Vertices.Count; j++)
                {
                    if (Normals[Faces[i].Vertices[j]] == null) Normals[Faces[i].Vertices[j]] = new List<Vertex>();
                    Normals[Faces[i].Vertices[j]].Add(Normal);
                    Faces[i].Normals.Add(j);
                }
            }
            for (int i = 0; i < _Vertices.Count; i++)
            {
                if (Normals[i] == null) this._Normals.Add(new Vertex(1, 0, 0));
                else this._Normals.Add(VertexTransformer.Average(Normals[i].ToArray()).ToVertex());
            }
            for (int i = 0; i < this._Faces.Count; i++)
            {
                for(int j = 0; j < this._Faces[i].Vertices.Count; j++)
                {
                    this._Faces[i].Normals[j] = this._Faces[i].Vertices[j];
                }
            }
        }
        public void CreaseVerts()
        {
            for(int i = 0; i < this._Faces.Count; i++)
            {
                for (int j = i+1; j < this._Faces.Count; j++)
                {
                    List<int> SameVertices = new List<int>();
                    List<int> F1Position = new List<int>();
                    List<int> F2Position = new List<int>();
                    for (int k = 0; k < this.Faces[i].Vertices.Count; k++)
                    {
                        for (int l = 0; l < this.Faces[j].Vertices.Count; l++)
                        {
                            if(this.Faces[i].Vertices[k] == this.Faces[j].Vertices[l])
                            {
                                SameVertices.Add(this.Faces[i].Vertices[k]);
                                F1Position.Add(k);
                                F2Position.Add(l);
                            }
                        }
                    }
                    if(SameVertices.Count > 1)
                    {
                        List<Vertex> Vertices1 = new List<Vertex>();
                        for (int k = 0; k < this.Faces[i].Vertices.Count; k++) Vertices1.Add(this._Vertices[this.Faces[i].Vertices[k]]);
                        List<Vertex> Vertices2 = new List<Vertex>();
                        for (int l = 0; l < this.Faces[j].Vertices.Count; l++) Vertices2.Add(this._Vertices[this.Faces[j].Vertices[l]]);
                        Vertex Normal1 = VertexTransformer.Normalize(VertexTransformer.CalculateNormal(Vertices1.ToArray()).ToVertex()).ToVertex();
                        Vertex Normal2 = VertexTransformer.Normalize(VertexTransformer.CalculateNormal(Vertices2.ToArray()).ToVertex()).ToVertex();
                        float Angle = VertexTransformer.Angle(Normal1, Normal2);
                        Angle *= (float)(180 / Math.PI);
                        if(Angle > 15)
                        {
                            for (int k = 0; k < SameVertices.Count; k++)
                            {
                                int Index = this._Vertices.Count;
                                this._Vertices.Add(this._Vertices[SameVertices[k]]);
                                if (this._Normals.Count > 0 && this.Faces[j].Normals.Count > 0) this._Normals.Add(this._Normals[this._Faces[j].Normals[F2Position[k]]]);
                                this.Faces[j].Vertices[F2Position[k]] = Index;
                                if (this._Normals.Count > 0 && this.Faces[j].Normals.Count > 0) this.Faces[j].Normals[F2Position[k]] = Index;
                            }
                        }
                    }
                }
            }
        }
    }
}
