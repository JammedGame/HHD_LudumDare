using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Draw
{
    public class ShaderProgram
    {
        protected bool _Compiled;
        protected int _Program_Indexer;
        protected int _VertexShader_Indexer;
        protected int _FragmentShader_Indexer;
        protected int _GeometryShader_Indexer;
        protected int _TessellationControl_Indexer;
        protected int _TessellationEvaluation_Indexer;
        protected string _ShaderID;
        private string _VertexShader_Code;
        private string _FragmentShader_Code;
        private string _GeometryShader_Code;
        private string _TessellationControl_Code;
        private string _TessellationEvaluation_Code;
        protected ShaderUniformPackage _Uniforms;
        protected ShaderAttributePackage _Attributes;
        protected ShaderTexturePackage _Textures;
        public bool Compiled
        {
            get
            {
                return _Compiled;
            }

            set
            {
                _Compiled = value;
            }
        }
        public string ShaderID
        {
            get
            {
                return _ShaderID;
            }

            set
            {
                _ShaderID = value;
            }
        }
        public string VertexShader_Code
        {
            get
            {
                return _VertexShader_Code;
            }

            set
            {
                _VertexShader_Code = value;
            }
        }
        public string FragmentShader_Code
        {
            get
            {
                return _FragmentShader_Code;
            }

            set
            {
                _FragmentShader_Code = value;
            }
        }
        public string GeometryShader_Code
        {
            get
            {
                return _GeometryShader_Code;
            }

            set
            {
                _GeometryShader_Code = value;
            }
        }
        public string TessellationControl_Code
        {
            get
            {
                return _TessellationControl_Code;
            }

            set
            {
                _TessellationControl_Code = value;
            }
        }
        public string TessellationEvaluation_Code
        {
            get
            {
                return _TessellationEvaluation_Code;
            }

            set
            {
                _TessellationEvaluation_Code = value;
            }
        }
        public ShaderUniformPackage Uniforms
        {
            get
            {
                return _Uniforms;
            }

            set
            {
                _Uniforms = value;
            }
        }
        public ShaderAttributePackage Attributes
        {
            get
            {
                return _Attributes;
            }

            set
            {
                _Attributes = value;
            }
        }
        public ShaderTexturePackage Textures
        {
            get
            {
                return _Textures;
            }

            set
            {
                _Textures = value;
            }
        }
        public ShaderProgram()
        {
            this._Compiled = false;
            this._Program_Indexer = -1;
            this._VertexShader_Indexer = -1;
            this._FragmentShader_Indexer = -1;
            this._GeometryShader_Indexer = -1;
            this._TessellationControl_Indexer = -1;
            this._TessellationEvaluation_Indexer = -1;
            this._ShaderID = "";
        }
        public ShaderProgram(string ShaderID)
        {
            this._Compiled = false;
            this._Program_Indexer = -1;
            this._VertexShader_Indexer = -1;
            this._FragmentShader_Indexer = -1;
            this._GeometryShader_Indexer = -1;
            this._TessellationControl_Indexer = -1;
            this._TessellationEvaluation_Indexer = -1;
            this._ShaderID = ShaderID;
        }
        public ShaderProgram(ShaderProgram ShaderProgram)
        {
            this._Compiled = ShaderProgram._Compiled;
            this._Program_Indexer = ShaderProgram._Program_Indexer;
            this._VertexShader_Indexer = ShaderProgram._VertexShader_Indexer;
            this._FragmentShader_Indexer = ShaderProgram._FragmentShader_Indexer;
            this._GeometryShader_Indexer = ShaderProgram._GeometryShader_Indexer;
            this._TessellationControl_Indexer = ShaderProgram._TessellationControl_Indexer;
            this._TessellationEvaluation_Indexer = ShaderProgram._TessellationEvaluation_Indexer;
            this._ShaderID = ShaderProgram._ShaderID;
        }
        public virtual bool Compile(string VertexShaderString, string FragmentShaderString)
        {
            return Compile(VertexShaderString, FragmentShaderString, null, null, null);
        }
        public virtual bool Compile(string VertexShaderString, string FragmentShaderString, string GeometryShaderString, string TessellationControlString, string TessellationEvaluationString)
        {
            return false;
        }
        public virtual void ReCompile()
        {
            this.Compile(this._VertexShader_Code, this._FragmentShader_Code, this._GeometryShader_Code, this._TessellationControl_Code, this._TessellationEvaluation_Code);
        }
        protected virtual void SetShaderCode(string VertexShaderString, string FragmentShaderString, string GeometryShaderString, string TessellationControlString, string TessellationEvaluationString)
        {
            this._VertexShader_Code = VertexShaderString;
            this._FragmentShader_Code = FragmentShaderString;
            this._GeometryShader_Code = GeometryShaderString;
            this._TessellationControl_Code = TessellationControlString;
            this._TessellationEvaluation_Code = TessellationEvaluationString;
        }
        public virtual void Draw(GraphicDrawMode DrawMode, int Offset)
        {
            
        }
        public virtual void Activate()
        {

        }
    }
}
