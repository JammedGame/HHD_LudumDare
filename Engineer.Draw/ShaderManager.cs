using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Draw
{
    public enum GraphicDrawMode
    {
        Points = 0,
        Lines = 1,
        LineLoop = 2,
        LineStrip = 3,
        Triangles = 4,
        TriangleStrip = 5,
        TriangleFan = 6,
        Quads = 7,
        QuadsExt = 7,
        LinesAdjacency = 10,
        LinesAdjacencyArb = 10,
        LinesAdjacencyExt = 10,
        LineStripAdjacency = 11,
        LineStripAdjacencyArb = 11,
        LineStripAdjacencyExt = 11,
        TrianglesAdjacency = 12,
        TrianglesAdjacencyArb = 12,
        TrianglesAdjacencyExt = 12,
        TriangleStripAdjacency = 13,
        TriangleStripAdjacencyArb = 13,
        TriangleStripAdjacencyExt = 13,
        Patches = 14,
        PatchesExt = 14
    }
    public class ShaderManager
    {
        protected string _ActiveShaderID;
        protected int _DrawLineOffset;
        protected GraphicDrawMode _DrawMode;
        protected Dictionary<string, ShaderProgram> _Shader;
        public ShaderProgram Active
        {
            get
            {
                if (_ActiveShaderID == "") return null;
                if (!_Shader.ContainsKey(_ActiveShaderID)) return null;
                return _Shader[_ActiveShaderID];
            }
        }
        public ShaderManager()
        {
            _ActiveShaderID = "";
            _Shader = new Dictionary<string, ShaderProgram>();
        }
        public virtual bool AddShader(string ID)
        {
            return false;
        }
        public virtual bool CompileShader(string ID, string VertexShaderString, string FragmentShaderString)
        {
            return _Shader[ID].Compile(VertexShaderString, FragmentShaderString);
        }
        public virtual bool CompileShader(string ID, string VertexShaderString, string FragmentShaderString, string GeometryShaderString, string TessellationControlString, string TessellationEvaluationString)
        {
            return _Shader[ID].Compile(VertexShaderString, FragmentShaderString, GeometryShaderString, TessellationControlString, TessellationEvaluationString);
        }
        virtual public bool DeleteShader(string ID)
        {
            _Shader[ID].Attributes.ClearData();
            _Shader[ID].Uniforms.ClearData();
            _Shader[ID].Textures.ClearData();
            _Shader.Remove(ID);
            return true;
        }
        virtual public bool ShaderExists(string ID)
        {
            return _Shader.ContainsKey(ID);
        }
        virtual public bool ActivateShader(string ID)
        {
            _ActiveShaderID = ID;
            return true;
        }
        virtual public void SetDrawMode(GraphicDrawMode DrawMode)
        {
            this._DrawMode = DrawMode;
        }
        virtual public void Draw()
        {
            if (_ActiveShaderID != "") _Shader[_ActiveShaderID].Draw(_DrawMode, _DrawLineOffset);
        }
    }
}
