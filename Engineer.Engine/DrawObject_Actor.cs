using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engineer.Data;
using System.Xml.Serialization;

namespace Engineer.Engine
{
    public class Actor : DrawObject
    {
        private bool _Modified;
        private List<int> _GeometryMaterialIndices;
        private List<Material> _Materials;
        private List<Geometry> _Geometries;
        public bool Modified
        {
            get
            {
                return _Modified;
            }

            set
            {
                _Modified = value;
            }
        }
        public List<int> GeometryMaterialIndices
        {
            get
            {
                return _GeometryMaterialIndices;
            }

            set
            {
                _GeometryMaterialIndices = value;
            }
        }
        public List<Material> Materials
        {
            get
            {
                return _Materials;
            }

            set
            {
                _Materials = value;
            }
        }
        [XmlIgnore]
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
        public Actor() : base()
        {
            this._Modified = false;
            this.Name = this.ID = Guid.NewGuid().ToString();
            this.Type = DrawObjectType.Actor;
            this.GeometryMaterialIndices = new List<int>();
            this._Geometries = new List<Geometry>();
            this.Materials = new List<Material>();
            if (_UsedIDs == null) _UsedIDs = new List<string>();
            _UsedIDs.Add(ID);
        }
        public Actor(MeshContainer Mesh, string Name) : base()
        {
            this.ID = Guid.NewGuid().ToString();
            this.Name = Name;
            this._Modified = false;
            this.Type = DrawObjectType.Actor;
            this._GeometryMaterialIndices = new List<int>();
            this._Geometries = Mesh.Geometries;
            this._Materials = new List<Material>();
            this._Materials.Add(new Material(ID + "_01", Material.Default));
            for(int i = 0; i < this._Geometries.Count; i++) this.GeometryMaterialIndices.Add(0);
        }
        public Actor(Actor A) : base(A)
        {
            this._Modified = A._Modified;
            this._GeometryMaterialIndices = new List<int>(A._GeometryMaterialIndices);
            this._Materials = new List<Material>();
            for(int i = 0; i < this._Materials.Count; i++) this._Materials.Add(new Material(Guid.NewGuid().ToString(), A._Materials[i]));
            this._Geometries = new List<Geometry>();
            for (int i = 0; i < this._Geometries.Count; i++) this._Geometries.Add(new Geometry(A._Geometries[i]));
        }
        private static List<string> _UsedIDs;
    }
}
