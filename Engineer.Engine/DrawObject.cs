using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engineer.Mathematics;
using System.Xml.Serialization;

namespace Engineer.Engine
{
    public enum DrawObjectType
    {
        Undefined = -1,
        Background = 0,
        Sprite = 1,
        Tile = 2,
        Actor = 3,
        Figure = 4,
        Camera = 5,
        Light = 6
    }
    public class DrawObject
    {
        private bool _Active;
        private bool _Fixed;
        private string _Name;
        private string _ID;
        private DrawObjectType _Type;
        private Vertex _Translation;
        private Vertex _Scale;
        private Vertex _Rotation;
        public bool Active
        {
            get
            {
                return _Active;
            }

            set
            {
                _Active = value;
            }
        }
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
        public string ID
        {
            get
            {
                return _ID;
            }

            set
            {
                _ID = value;
            }
        }
        public DrawObjectType Type
        {
            get
            {
                return _Type;
            }

            set
            {
                _Type = value;
            }
        }
        public Vertex Translation
        {
            get
            {
                return _Translation;
            }

            set
            {
                _Translation = value;
            }
        }
        public Vertex Scale
        {
            get
            {
                return _Scale;
            }

            set
            {
                _Scale = value;
            }
        }
        public Vertex Rotation
        {
            get
            {
                return _Rotation;
            }

            set
            {
                _Rotation = value;
            }
        }
        public bool Fixed { get => _Fixed; set => _Fixed = value; }
        public DrawObject()
        {
            this._Active = true;
            this._Name = this._ID = Guid.NewGuid().ToString();
            this._Type = DrawObjectType.Undefined;
            this._Translation = new Vertex();
            this._Scale = new Vertex(1, 1, 1);
            this._Rotation = new Vertex();
        }
        public DrawObject(DrawObject Object)
        {
            this._Active = Object.Active;
            this._ID = Guid.NewGuid().ToString();
            this._Name = Object.Name;
            this._Type = Object.Type;
            this._Translation = new Vertex(Object.Translation.X, Object.Translation.Y, Object.Translation.Z);
            this._Rotation = new Vertex(Object.Rotation.X, Object.Rotation.Y, Object.Rotation.Z);
            this._Scale = new Vertex(Object.Scale.X, Object.Scale.Y, Object.Scale.Z);
        }
    }
}
