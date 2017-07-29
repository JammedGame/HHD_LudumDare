using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Data
{
    public class MeshContainer : FileContainer
    {
        private List<Geometry> _Geometries;
        public List<Geometry> Geometries
        {
            get
            {
                return _Geometries;
            }

            set
            {
                _Geometries = value;
            }
        }
        public MeshContainer()
        {
            this._Geometries = new List<Geometry>();
        }
        public void RecalculateNormals()
        {
            for (int i = 0; i < this._Geometries.Count; i++) this._Geometries[i].RecalculateNormals();
        }
        public void CreaseVerts()
        {
            for (int i = 0; i < this._Geometries.Count; i++) this._Geometries[i].CreaseVerts();
        }
    }
}
