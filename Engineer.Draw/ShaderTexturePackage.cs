using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Engineer.Mathematics;

namespace Engineer.Draw
{
    public class ShaderTexturePackage
    {
        protected static bool _Binded = false;
        protected static uint _Index = 0;
        protected bool _Active;
        protected bool _Loaded;
        protected int _TexturesNumber;
        protected Vertex _Resolution;
        protected byte[] _Textures;
        public Vertex Resolution { get => _Resolution; set => _Resolution = value; }
        public ShaderTexturePackage()
        {
            this._Active = false;
            this._Loaded = false;
            this._TexturesNumber = 0;
            this._Textures = null;
        }
        public ShaderTexturePackage(ShaderTexturePackage Package)
        {
            this._Active = false;
            this._Loaded = false;
            this._TexturesNumber = Package._TexturesNumber;
            Array.Copy(Package._Textures, this._Textures, Package._Textures.Length);
        }
        public virtual void SetData(int TexturesNumber, byte[] Textures)
        {
            this._Active = false;
            this._Loaded = false;
            this._TexturesNumber = TexturesNumber;
            this._Textures = Textures;
        }
        public virtual void ClearData()
        {
            this._TexturesNumber = 0;
            this._Textures = null;
        }
        public virtual bool Activate()
        {
            return false;
        }
    }
}
