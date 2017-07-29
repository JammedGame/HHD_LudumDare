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
    public class GLSLShaderProgram : ShaderProgram
    {
        public GLSLShaderProgram() : base()
        {
            this._Uniforms = new GLSLShaderUniformPackage();
            this._Attributes = new GLSLShaderAttributePackage();
            this._Textures = new GLSLShaderTexturePackage();
        }
        public GLSLShaderProgram(string ID) : base(ID)
        {
            this._Uniforms = new GLSLShaderUniformPackage();
            this._Attributes = new GLSLShaderAttributePackage();
            this._Textures = new GLSLShaderTexturePackage();
        }
        public GLSLShaderProgram(GLSLShaderProgram GLSLShaderProgram) : base (GLSLShaderProgram)
        {
            this._Uniforms = new GLSLShaderUniformPackage(GLSLShaderProgram._Uniforms);
            this._Attributes = new GLSLShaderAttributePackage(GLSLShaderProgram._Attributes);
            this._Textures = new GLSLShaderTexturePackage(GLSLShaderProgram._Textures);
        }
        public override bool Compile(string VertexShaderString, string FragmentShaderString, string GeometryShaderString, string TessellationControlString, string TessellationEvaluationString)
        {
            SetShaderCode(VertexShaderString, FragmentShaderString, GeometryShaderString, TessellationControlString, TessellationEvaluationString);
            int IsCompiled;
            _VertexShader_Indexer = GL.CreateShader(ShaderType.VertexShader);
            _FragmentShader_Indexer = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(_VertexShader_Indexer, VertexShaderString);
            GL.CompileShader(_VertexShader_Indexer);
            GL.GetShader(_VertexShader_Indexer, ShaderParameter.CompileStatus, out IsCompiled);
            if (IsCompiled == 0)
            {
                return false;
            }
            GL.ShaderSource(_FragmentShader_Indexer, FragmentShaderString);
            GL.CompileShader(_FragmentShader_Indexer);
            GL.GetShader(_FragmentShader_Indexer, ShaderParameter.CompileStatus, out IsCompiled);
            if (IsCompiled == 0)
            {
                return false;
            }
            if(GeometryShaderString != null)
            {
                _GeometryShader_Indexer = GL.CreateShader(ShaderType.GeometryShader);
                GL.ShaderSource(_GeometryShader_Indexer, GeometryShaderString);
                GL.CompileShader(_GeometryShader_Indexer);
                GL.GetShader(_GeometryShader_Indexer, ShaderParameter.CompileStatus, out IsCompiled);
                if (IsCompiled == 0)
                {
                    return false;
                }
            }
            if (TessellationControlString != null)
            {
                _TessellationControl_Indexer = GL.CreateShader(ShaderType.TessControlShader);
                GL.ShaderSource(_TessellationControl_Indexer, TessellationControlString);
                GL.CompileShader(_TessellationControl_Indexer);
                GL.GetShader(_TessellationControl_Indexer, ShaderParameter.CompileStatus, out IsCompiled);
                if (IsCompiled == 0) return false;
            }
            if (TessellationEvaluationString != null)
            {
                _TessellationEvaluation_Indexer = GL.CreateShader(ShaderType.TessEvaluationShader);
                GL.ShaderSource(_TessellationEvaluation_Indexer, TessellationEvaluationString);
                GL.CompileShader(_TessellationEvaluation_Indexer);
                GL.GetShader(_TessellationEvaluation_Indexer, ShaderParameter.CompileStatus, out IsCompiled);
                if (IsCompiled == 0) return false;
            }

            GL.DeleteProgram(_Program_Indexer);
            _Program_Indexer = GL.CreateProgram();
            GL.AttachShader(_Program_Indexer, _VertexShader_Indexer);
            GL.AttachShader(_Program_Indexer, _FragmentShader_Indexer);
            if (GeometryShaderString != null) GL.AttachShader(_Program_Indexer, _GeometryShader_Indexer);
            if (TessellationControlString != null) GL.AttachShader(_Program_Indexer, _TessellationControl_Indexer);
            if (TessellationEvaluationString != null) GL.AttachShader(_Program_Indexer, _TessellationEvaluation_Indexer);
            _Attributes.Bind(_Program_Indexer);
            GL.LinkProgram(_Program_Indexer);
            GL.GetProgram(_Program_Indexer, GetProgramParameterName.LinkStatus, out IsCompiled);
            if (IsCompiled == 0)
            {
                return false;
            }
            _Compiled = true;
            return true;
        }
        public override void Activate()
        {
            if (_Compiled)
            {
                GL.UseProgram(_Program_Indexer);
            }
        }
        public override void Draw(GraphicDrawMode DrawMode, int Offset)
        {
            if (!_Compiled)
            {
                this.Compile(this.VertexShader_Code, this.FragmentShader_Code, this.GeometryShader_Code, this.TessellationControl_Code, this.TessellationEvaluation_Code);
            }
            if (_Compiled)
            {
                GL.UseProgram(_Program_Indexer);
                if (!_Uniforms.Activate(_Program_Indexer)) return;
                if (!_Attributes.Activate(_Program_Indexer)) return;
                if (!_Textures.Activate()) return;
                GL.DrawArrays((PrimitiveType)DrawMode, Offset, _Attributes.BufferLines);
                GL.UseProgram(0);
            }
        }
    }
}
