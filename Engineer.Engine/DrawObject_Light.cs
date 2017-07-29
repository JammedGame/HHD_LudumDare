using Engineer.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Engine
{
    public class Light : DrawObject
    {
        private float _Intensity;
        private Color _Color;
        private Vertex _Attenuation;
        public float Intensity
        {
            get
            {
                return _Intensity;
            }

            set
            {
                _Intensity = value;
            }
        }
        public Color Color
        {
            get
            {
                return _Color;
            }

            set
            {
                _Color = value;
            }
        }
        public Vertex Attenuation
        {
            get
            {
                return _Attenuation;
            }

            set
            {
                _Attenuation = value;
            }
        }
        public Light() : base()
        {
            this.Type = DrawObjectType.Light;
            this._Intensity = 1f;
            this._Color = Color.White;
            this._Attenuation = new Vertex(1, 0, 0);
        }
        public Light(Light L) : base (L)
        {
            this._Intensity = L._Intensity;
            this._Color = L._Color;
            this._Attenuation = new Vertex(L._Attenuation.X, L._Attenuation.Y, L._Attenuation.Z);
        }
    }
}
