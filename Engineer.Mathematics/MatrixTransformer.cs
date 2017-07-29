using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Mathematics
{
    public class MatrixTransformer
    {
        private int _MatrixMode;
        private List<float[]> _PushedProjectionMatrix;
        private List<float[]> _PushedModelViewMatrix;
        private float[] _ProjectionMatrix;
        private float[] _ModelViewMatrix;
        public float[] ProjectionMatrix
        {
            get
            {
                return _ProjectionMatrix;
            }

            set
            {
                _ProjectionMatrix = value;
            }
        }
        public float[] ModelViewMatrix
        {
            get
            {
                return _ModelViewMatrix;
            }

            set
            {
                _ModelViewMatrix = value;
            }
        }
        public MatrixTransformer()
        {
            this._MatrixMode = 0;
            this._ModelViewMatrix = new float[16];
            MTIdentity(ref this._ModelViewMatrix);
            this._ProjectionMatrix = new float[16];
            MTIdentity(ref this._ProjectionMatrix);
            this._PushedModelViewMatrix = new List<float[]>();
            this._PushedProjectionMatrix = new List<float[]>();
        }
        private void MTFrustum(ref float[] Matrix, float Left, float Right, float Bottom, float Top, float ZNear, float ZFar)
        {
            float A = (Right + Left) / (Right - Left);
            float B = (Top + Bottom) / (Top - Bottom);
            float C = -(ZFar + ZNear) / (ZFar - ZNear);
            float D = -(ZFar * ZNear * 2) / (ZFar - ZNear);
            MTIdentity(ref Matrix);
            Matrix[0 * 4 + 0] = (2 * ZNear) / (Right - Left);
            Matrix[1 * 4 + 1] = (2 * ZNear) / (Top - Bottom);
            Matrix[2 * 4 + 2] = C;
            Matrix[3 * 4 + 3] = 0;
            Matrix[2 * 4 + 0] = A;
            Matrix[2 * 4 + 1] = B;
            Matrix[3 * 4 + 2] = D;
            Matrix[2 * 4 + 3] = -1;
        }
        private void MTOrtho(ref float[] Matrix, float Left, float Right, float Bottom, float Top, float ZNear, float ZFar)
        {
            float A = 2.0f / (Right - Left);
            float B = 2.0f / (Top - Bottom);
            float C = -2.0f / (ZFar - ZNear);
            float TX = -(Right + Left) / (Right - Left);
            float TY = -(Top + Bottom) / (Top - Bottom);
            float TZ = -(ZFar + ZNear) / (ZFar - ZNear);
            MTIdentity(ref Matrix);
            Matrix[0 * 4 + 0] = A;
            Matrix[1 * 4 + 1] = B;
            Matrix[2 * 4 + 2] = C;
            Matrix[3 * 4 + 0] = TX;
            Matrix[3 * 4 + 1] = TY;
            Matrix[3 * 4 + 2] = TZ;
        }
        public static void MTIdentity(ref float[] Matrix)
        {
            Matrix = new float[16] { 1, 0, 0, 0,
                                     0, 1, 0, 0,
                                     0, 0, 1, 0,
                                     0, 0, 0, 1};
        }
        public static float[] MTTranslate(float X, float Y, float Z)
        {
            float[] Matrix = new float[16];
            MTIdentity(ref Matrix);
            Matrix[3 * 4 + 0] = X;
            Matrix[3 * 4 + 1] = Y;
            Matrix[3 * 4 + 2] = Z;
            return Matrix;
        }
        public static float[] MTScale(float X, float Y, float Z)
        {
            float[] Matrix = new float[16];
            MTIdentity(ref Matrix);
            Matrix[0 * 4 + 0] = X;
            Matrix[1 * 4 + 1] = Y;
            Matrix[2 * 4 + 2] = Z;
            return Matrix;
        }
        public static float[] MTRotate(int Axis, float Angle)
        {
            float[] Matrix = new float[16];
            float SinTheta, CosTheta;
            SinTheta = (float)Math.Sin((Angle / 180) * Math.PI);
            CosTheta = (float)Math.Cos((Angle / 180) * Math.PI);
            MTIdentity(ref Matrix);
            if (Axis == 0)
            {
                Matrix[1 * 4 + 1] = CosTheta;
                Matrix[1 * 4 + 2] = SinTheta;
                Matrix[2 * 4 + 1] = -Matrix[1 * 4 + 2];
                Matrix[2 * 4 + 2] = Matrix[1 * 4 + 1];
                Matrix[0 * 4 + 0] = 1; Matrix[0 * 4 + 1] = 0; Matrix[0 * 4 + 2] = 0; Matrix[0 * 4 + 3] = 0; Matrix[1 * 4 + 0] = 0;
                Matrix[1 * 4 + 3] = 0; Matrix[2 * 4 + 0] = 0;
                Matrix[2 * 4 + 3] = 0; Matrix[3 * 4 + 0] = 0; Matrix[3 * 4 + 1] = 0; Matrix[3 * 4 + 2] = 0; Matrix[3 * 4 + 3] = 1;
            }
            else if (Axis == 1)
            {
                Matrix[0 * 4 + 0] = CosTheta;
                Matrix[2 * 4 + 0] = SinTheta;
                Matrix[0 * 4 + 2] = -Matrix[2 * 4 + 0];
                Matrix[2 * 4 + 2] = Matrix[0 * 4 + 0];
                Matrix[0 * 4 + 1] = 0; Matrix[0 * 4 + 3] = 0; Matrix[1 * 4 + 0] = 0; Matrix[1 * 4 + 1] = 1; Matrix[1 * 4 + 2] = 0;
                Matrix[1 * 4 + 3] = 0; Matrix[2 * 4 + 1] = 0;
                Matrix[2 * 4 + 3] = 0; Matrix[3 * 4 + 0] = 0; Matrix[3 * 4 + 1] = 0; Matrix[3 * 4 + 2] = 0; Matrix[3 * 4 + 3] = 1;
            }
            else
            {
                Matrix[0 * 4 + 0] = CosTheta;
                Matrix[0 * 4 + 1] = SinTheta;
                Matrix[1 * 4 + 0] = -Matrix[0 * 4 + 1];
                Matrix[1 * 4 + 1] = Matrix[0 * 4 + 0];
                Matrix[0 * 4 + 2] = 0; Matrix[0 * 4 + 3] = 0; Matrix[1 * 4 + 2] = 0; Matrix[1 * 4 + 3] = 0;
                Matrix[2 * 4 + 0] = 0; Matrix[2 * 4 + 1] = 0; Matrix[2 * 4 + 2] = 1.0f; Matrix[2 * 4 + 3] = 0;
                Matrix[3 * 4 + 0] = 0; Matrix[3 * 4 + 1] = 0; Matrix[3 * 4 + 2] = 0; Matrix[3 * 4 + 3] = 1;
            }
            return Matrix;
        }
        public static float[] MTMultiply4x4(float[] M1, float[] M2)
        {
            float[] Result = new float[16];
            for (int i = 0; i < 4; ++i)
                for (int j = 0; j < 4; ++j) Result[i * 4 + j] = 0;
            for (int i = 0; i < 4; ++i)
                for (int j = 0; j < 4; ++j)
                    for (int k = 0; k < 4; ++k) Result[i * 4 + j] += M1[i * 4 + k] * M2[k * 4 + j];
            return Result;
        }
        public void MatrixMode(string MatrixMode)
        {
            if (MatrixMode.ToUpper() == "PROJECTION") _MatrixMode = 0;
            if (MatrixMode.ToUpper() == "MODELVIEW") _MatrixMode = 1;
        }
        public void LoadMatrix(float[] Matrix)
        {
            if (_MatrixMode == 1) ModelViewMatrix = Matrix;
            else ProjectionMatrix = Matrix;
        }
        public void LoadIdentity()
        {
            if (_MatrixMode == 1) MTIdentity(ref _ModelViewMatrix);
            else MTIdentity(ref _ProjectionMatrix);
        }
        public void PushMatrix()
        {
            float[] _PushedMatrix = new float[16];
            if (_MatrixMode == 1)
            {
                Array.Copy(ModelViewMatrix, _PushedMatrix, 16);
                this._PushedModelViewMatrix.Insert(0, _PushedMatrix);
            }
            else
            {
                Array.Copy(ProjectionMatrix, _PushedMatrix, 16);
                this._PushedProjectionMatrix.Insert(0, _PushedMatrix);
            }
        }
        public void PopMatrix()
        {
            if (_MatrixMode == 1)
            {
                if (_PushedModelViewMatrix.Count > 0)
                {
                    Array.Copy(_PushedModelViewMatrix[0], ModelViewMatrix, 16);
                    _PushedModelViewMatrix.RemoveAt(0);
                }
                else MTIdentity(ref this._ModelViewMatrix);
            }
            else
            {
                if (_PushedProjectionMatrix.Count > 0)
                {
                    Array.Copy(_PushedProjectionMatrix[0], ProjectionMatrix, 16);
                    _PushedProjectionMatrix.RemoveAt(0);
                }
                else MTIdentity(ref this._ProjectionMatrix);
            }
        }
        public void ReadMatrix()
        {
            if (_MatrixMode == 1)
            {
                if (_PushedModelViewMatrix.Count > 0)
                {
                    Array.Copy(_PushedModelViewMatrix[0], ModelViewMatrix, 16);
                }
                else MTIdentity(ref this._ModelViewMatrix);
            }
            else
            {
                if (_PushedProjectionMatrix.Count > 0)
                {
                    Array.Copy(_PushedProjectionMatrix[0], ProjectionMatrix, 16);
                }
                else MTIdentity(ref this._ProjectionMatrix);
            }
        }
        public void MultMatrix(float[] Matrix)
        {
            if (_MatrixMode == 1) ModelViewMatrix = MTMultiply4x4(ModelViewMatrix, Matrix);
            else ProjectionMatrix = MTMultiply4x4(ProjectionMatrix, Matrix);
        }
        public void Frustum(float Left, float Right, float Bottom, float Top, float Near, float Far)
        {
            if (_MatrixMode == 1) MTFrustum(ref _ModelViewMatrix, Left, Right, Bottom, Top, Near, Far);
            else MTFrustum(ref _ProjectionMatrix, Left, Right, Bottom, Top, Near, Far);
        }
        public void Ortho(float Left, float Right, float Bottom, float Top, float Near, float Far)
        {
            if (_MatrixMode == 1) MTOrtho(ref _ModelViewMatrix, Left, Right, Bottom, Top, Near, Far);
            else MTOrtho(ref _ProjectionMatrix, Left, Right, Bottom, Top, Near, Far);
        }
        public void Ortho2D(float Left, float Right, float Bottom, float Top)
        {
            if (_MatrixMode == 1) MTOrtho(ref _ModelViewMatrix, Left, Right, Bottom, Top, -1, 1);
            else MTOrtho(ref _ProjectionMatrix, Left, Right, Bottom, Top, -1, 1);
        }
        public void Translate(float X, float Y, float Z)
        {
            if (_MatrixMode == 1) _ModelViewMatrix = MTMultiply4x4(MTTranslate(X, Y, Z), _ModelViewMatrix);
            else _ProjectionMatrix = MTMultiply4x4(MTTranslate(X, Y, Z), _ProjectionMatrix);
        }
        public void Scale(float X, float Y, float Z)
        {
            if (_MatrixMode == 1) _ModelViewMatrix = MTMultiply4x4(MTScale(X, Y, Z), _ModelViewMatrix);
            else _ProjectionMatrix = MTMultiply4x4(MTScale(X, Y, Z), _ProjectionMatrix);
        }
        public void Rotate(float _Angle, float X, float Y, float Z)
        {
            if (_MatrixMode == 1)
             {
                 _ModelViewMatrix = MTMultiply4x4(MTRotate(0, X * _Angle), _ModelViewMatrix);
                 _ModelViewMatrix = MTMultiply4x4(MTRotate(1, Y * _Angle), _ModelViewMatrix);
                 _ModelViewMatrix = MTMultiply4x4(MTRotate(2, Z * _Angle), _ModelViewMatrix);
             }
             else
             {
                 _ProjectionMatrix = MTMultiply4x4(MTRotate(0, X * _Angle), _ProjectionMatrix);
                 _ProjectionMatrix = MTMultiply4x4(MTRotate(1, Y * _Angle), _ProjectionMatrix);
                 _ProjectionMatrix = MTMultiply4x4(MTRotate(2, Z * _Angle), _ProjectionMatrix);
             }
        }
        public void Perspective(float FieldOfView, float Aspect, float ZNear, float ZFar)
        {
            float Top = (float)(Math.Tan(FieldOfView / 360 * Math.PI) * ZNear);
            float Bottom = -Top;
            float Right = Top * Aspect;
            float Left = -Right;
            Frustum(Left, Right, Bottom, Top, ZNear, ZFar);
        }
        public void LookAt(float EyeX, float EyeY, float EyeZ, float TargetX, float TargetY, float TargetZ, float UpX, float UpY, float UpZ)
        {
            float[] Matrix = new float[16];
            VertexBuilder Forward = new VertexBuilder(TargetX - EyeX, TargetY - EyeY, TargetZ - EyeZ);
            VertexBuilder Up = new VertexBuilder(UpX, UpY, UpZ);
            VertexBuilder Side = new VertexBuilder(0, 0);
            Forward = Forward.Normalize();
            Side = VertexBuilder.Cross(Forward, Up);
            Side = Side.Normalize();
            Up = VertexBuilder.Cross(Side, Forward);
            MTIdentity(ref Matrix);
            Matrix[0 * 4 + 0] = Side.X;
            Matrix[1 * 4 + 0] = Side.Y;
            Matrix[2 * 4 + 0] = Side.Z;
            Matrix[0 * 4 + 1] = Up.X;
            Matrix[1 * 4 + 1] = Up.Y;
            Matrix[2 * 4 + 1] = Up.Z;
            Matrix[0 * 4 + 2] = -Forward.X;
            Matrix[1 * 4 + 2] = -Forward.Y;
            Matrix[2 * 4 + 2] = -Forward.Z;
            MultMatrix(Matrix);
            Translate(-EyeX, -EyeY, -EyeZ);
        }
        public void DefaultPerspective(int Width, int Height)
        {
            Perspective(45, Width * 1.0f / Height, 0.001f, 1000000f);
        }
        public void DefaultView(Vertex Eye, Vertex Target)
        {
            LookAt(Eye.X, Eye.Y, Eye.Z, Target.X, Target.Y, Target.Z, 0, 1, 0);
        }
        public static Vertex TransformVertex(float[] Matrix, Vertex ToTransform)
        {
            float[] NewVertex = new float[4];
            NewVertex[0] = Matrix[0 * 4 + 0] * ToTransform.X + Matrix[0 * 4 + 1] * ToTransform.Y + Matrix[0 * 4 + 2] * ToTransform.Z + Matrix[0 * 4 + 3] * 1;
            NewVertex[1] = Matrix[1 * 4 + 0] * ToTransform.X + Matrix[1 * 4 + 1] * ToTransform.Y + Matrix[1 * 4 + 2] * ToTransform.Z + Matrix[1 * 4 + 3] * 1;
            NewVertex[2] = Matrix[2 * 4 + 0] * ToTransform.X + Matrix[2 * 4 + 1] * ToTransform.Y + Matrix[2 * 4 + 2] * ToTransform.Z + Matrix[2 * 4 + 3] * 1;
            NewVertex[3] = Matrix[3 * 4 + 0] * ToTransform.X + Matrix[3 * 4 + 1] * ToTransform.Y + Matrix[3 * 4 + 2] * ToTransform.Z + Matrix[3 * 4 + 3] * 1;
            return new Vertex(NewVertex[0], NewVertex[1], NewVertex[2]);
        }
    }
}
