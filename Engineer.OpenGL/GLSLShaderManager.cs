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
    public class GLSLShaderManager : ShaderManager
    {
        public GLSLShaderManager()
        {

        }
        public override bool AddShader(string ID)
        {
            if (_Shader.ContainsKey(ID)) return false;
            GLSLShaderProgram NewProgram;
            NewProgram = new GLSLShaderProgram(ID);
            _Shader.Add(ID, NewProgram);
            return true;
        }
    }
}
