using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Mathematics
{
    public class Polygon
    {
    }
    public class Face
    {
        private List<int> _Vertices;
        private List<int> _Normals;
        private List<int> _TexCoords;
        public List<int> Vertices
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
        public List<int> Normals
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
        public List<int> TexCoords
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
        public Face()
        {
            this._Vertices = new List<int>();
            this._Normals = new List<int>();
            this._TexCoords = new List<int>();
        }
        public Face(Face F)
        {
            this._Vertices = new List<int>(F._Vertices);
            this._Normals = new List<int>(F._Normals);
            this._TexCoords = new List<int>(F._TexCoords);
        }
    }
}
