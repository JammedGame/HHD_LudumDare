using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Engineer.Engine;

namespace Engineer.Draw.OpenGL
{

    public class GLSLShaderMaterialTranslator : ShaderMaterialTranslator
    {
        public GLSLShaderMaterialTranslator() : base("GLSL")
        {

        }
        public GLSLShaderMaterialTranslator(string Type) : base(Type)
        {

        }
        protected override string GenerateMainFunction(string[] Lines)
        {
            string Main = "void main()\n{\n";
            for(int i = 0; i < Lines.Length; i++)
            {
                Main += Lines[i] + "\n";
            }
            Main += "}";
            return Main;
        }
        protected override string GenerateFragmentFinalizer(bool Input, string ID)
        {
            if(Input) return "FinalColor = " + ID + ";";
            else return "FinalColor = vec4(1,1,1,1);";
        }
        protected override string GenerateFunctionCall(string FunctionName, string[] ArgumentTexts, string[] OutputNames)
        {
            string Function = "";
            for(int i = 0; i < OutputNames.Length; i++)
            {
                Function += "\tvec4 " + OutputNames[i] + ";\n";
            }
            Function += "\t" + FunctionName + " (";
            for (int i = 0; i < ArgumentTexts.Length; i++)
            {
                if (i == ArgumentTexts.Length - 1 && OutputNames.Length == 0) Function += ArgumentTexts[i] + ");";
                else Function += ArgumentTexts[i] + ",";
            }
            for (int i = 0; i < OutputNames.Length; i++)
            {
                if (i == OutputNames.Length - 1) Function += OutputNames[i] + ");";
                else Function += OutputNames[i] + ",";
            }
            return Function;
        }
        protected override string GenerateArgumentFromValue(MaterialValueHolder Value)
        {
            return "vec4(" + Value.X + "," + Value.Y + "," + Value.Z + "," + Value.W + ")";
        }
        protected override string GenerateArgumentFromEngineVariable(string VariableName)
        {
            return VariableName;
        }
    }
}
