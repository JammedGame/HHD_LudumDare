using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Engine
{
    public enum MaterialValueType
    {
        Input = 0,
        Output = 1,
        Value = 2,
        BoolValue = 3,
        IntValue = 4,
        FloatValue = 5,
        VertexValue = 6,
        ColorValue = 7,
        TextureValue = 8
    }
    public class MaterialValueHolder
    {
        private double _X;
        private double _Y;
        private double _Z;
        private double _W;
        private string _Value;
        private MaterialValueType _Type;
        public double X
        {
            get
            {
                return _X;
            }

            set
            {
                _X = value;
            }
        }
        public double Y
        {
            get
            {
                return _Y;
            }

            set
            {
                _Y = value;
            }
        }
        public double Z
        {
            get
            {
                return _Z;
            }

            set
            {
                _Z = value;
            }
        }
        public double W
        {
            get
            {
                return _W;
            }

            set
            {
                _W = value;
            }
        }
        public string Value
        {
            get
            {
                return _Value;
            }

            set
            {
                _Value = value;
            }
        }
        public MaterialValueType Type
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
        public MaterialValueHolder()
        {
            _X = 0;
            _Y = 0;
            _Z = 0;
            _W = 1;
            this._Type = MaterialValueType.VertexValue;
        }
        public MaterialValueHolder(double X, double Y, double Z)
        {
            _X = X;
            _Y = Y;
            _Z = Z;
            _W = 1;
            this._Type = MaterialValueType.VertexValue;
        }
        public MaterialValueHolder(double X, double Y, double Z, double W)
        {
            _X = X;
            _Y = Y;
            _Z = Z;
            _W = W;
            this._Type = MaterialValueType.ColorValue;
        }
        public MaterialValueHolder(int R, int G, int B, int A)
        {
            this._X = (R * 1.0 + 1) / 256;
            this._Y = (G * 1.0 + 1) / 256;
            this._Z = (B * 1.0 + 1) / 256;
            this._W = (A * 1.0 + 1) / 256;
            this._Type = MaterialValueType.ColorValue;
        }
        public MaterialValueHolder(string Value)
        {
            this._X = 0;
            this._Y = 0;
            this._Z = 0;
            this._W = 0;
            this._Value = Value;
            this._Type = MaterialValueType.TextureValue;
        }
    }
}
