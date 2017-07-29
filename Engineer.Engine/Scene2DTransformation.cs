using Engineer.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Engine
{
    public class Scene2DTransformation
    {
        private Vertex _Translation;
        private Vertex _Rotation;
        private Vertex _Scale;
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
        public Scene2DTransformation()
        {
            this._Translation = new Vertex();
            this._Rotation = new Vertex();
            this._Scale = new Vertex(1, 1, 1);
        }
        public Scene2DTransformation(Scene2DTransformation S2DT)
        {
            this._Translation = new Vertex(S2DT._Translation.X, S2DT._Translation.Y, S2DT._Translation.Z);
            this._Rotation = new Vertex(S2DT._Rotation.X, S2DT._Rotation.Y, S2DT._Rotation.Z);
            this._Scale = new Vertex(S2DT._Scale.X, S2DT._Scale.Y, S2DT._Scale.Z);
        }
    }
}
