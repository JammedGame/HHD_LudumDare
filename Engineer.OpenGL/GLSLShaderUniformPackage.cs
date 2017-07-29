using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engineer.Draw.OpenGL.GLSL
{
    public class GLSLShaderUniformPackage : ShaderUniformPackage
    {
        public GLSLShaderUniformPackage() : base()
        {
        }
        public GLSLShaderUniformPackage(ShaderUniformPackage ShaderUniformPackage) : base(ShaderUniformPackage)
        {
        }
        public GLSLShaderUniformPackage(GLSLShaderUniformPackage GLSLShaderUniformPackage) : base(GLSLShaderUniformPackage)
        {
        }
        public override bool Activate(int Program_Indexer)
        {
            bool SomeFailed = false;
            int CurrentUniformLocation = -1;
            for (int i = 0; i < _ID.Count; i++)
            {
                if (_Data[i] != null)
                {
                    CurrentUniformLocation = GL.GetUniformLocation(Program_Indexer, _ID[i]);
                    if (CurrentUniformLocation != -1)
                    {
                        if (_Type[i] == "bool")
                        {
                            GL.Uniform1(CurrentUniformLocation, _Data[i][0]);
                        }
                        else if (_Type[i] == "int")
                        {
                            GL.Uniform1(CurrentUniformLocation, BitConverter.ToInt32(_Data[i], 0));
                        }
                        else if (_Type[i] == "float")
                        {
                            GL.Uniform1(CurrentUniformLocation, BitConverter.ToSingle(_Data[i], 0));
                        }
                        else if (_Type[i] == "vec2")
                        {
                            GL.Uniform2(CurrentUniformLocation, BitConverter.ToSingle(_Data[i], 0), BitConverter.ToSingle(_Data[i], 4));
                        }
                        else if (_Type[i] == "vec3")
                        {
                            GL.Uniform3(CurrentUniformLocation, BitConverter.ToSingle(_Data[i], 0), BitConverter.ToSingle(_Data[i], 4), BitConverter.ToSingle(_Data[i], 8));
                        }
                        else if (_Type[i] == "vec4")
                        {
                            GL.Uniform4(CurrentUniformLocation, BitConverter.ToSingle(_Data[i], 0), BitConverter.ToSingle(_Data[i], 4), BitConverter.ToSingle(_Data[i], 8), BitConverter.ToSingle(_Data[i], 12));
                        }
                        else if (_Type[i] == "mat4")
                        {
                            Matrix4 NewMatrix = new Matrix4();
                            for (int j = 0; j < 4; j++)
                            {
                                for (int k = 0; k < 4; k++)
                                {
                                    NewMatrix[j, k] = BitConverter.ToSingle(_Data[i], ((j * 4) + k) * 4);
                                }
                            }
                            GL.UniformMatrix4(CurrentUniformLocation, false, ref NewMatrix);
                        }
                        else SomeFailed = true;
                    }
                }
            }
            return !SomeFailed;
        }
    }
}
