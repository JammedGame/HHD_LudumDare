using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Engineer.Engine;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Engineer.Draw
{
    public class ShaderMaterialTranslator : MaterialTranslator
    {
        public static ShaderMaterialTranslator Translator;
        private int _TexturesNumber;
        private string _MaterialTranslatorType;
        private string _VertexShaderOutput;
        private string _FragmentShaderOutput;
        private byte[] _Textures;
        private List<Bitmap> _TextureBitmaps;
        private List<ShaderMaterialTranslatorEntry> _Entries;
        public List<ShaderMaterialTranslatorEntry> Entries
        {
            get
            {
                return _Entries;
            }

            set
            {
                _Entries = value;
            }
        }
        public int TexturesNumber
        {
            get
            {
                return _TexturesNumber;
            }

            set
            {
                _TexturesNumber = value;
            }
        }
        public string VertexShaderOutput
        {
            get
            {
                return _VertexShaderOutput;
            }

            set
            {
                _VertexShaderOutput = value;
            }
        }
        public string FragmentShaderOutput
        {
            get
            {
                return _FragmentShaderOutput;
            }

            set
            {
                _FragmentShaderOutput = value;
            }
        }
        public byte[] Textures
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
        public ShaderMaterialTranslator()
        {
            this.TexturesNumber = 0;
            this._TextureBitmaps = new List<Bitmap>();
            this._MaterialTranslatorType = "Shader";
            this._Entries = new List<ShaderMaterialTranslatorEntry>();
            LoadEntries();
            Translator = this;
        }
        public ShaderMaterialTranslator(string Type)
        {
            this.TexturesNumber = 0;
            this._TextureBitmaps = new List<Bitmap>();
            this._MaterialTranslatorType = Type;
            this._Entries = new List<ShaderMaterialTranslatorEntry>();
            LoadEntries();
            Translator = this;
        } 
        public override bool TranslateMaterial(Material AppliedMaterial)
        {
            MaterialNode Output = null;
            this._TextureBitmaps = new List<Bitmap>();
            this._TexturesNumber = 0;
            this._Textures = null;
            for(int i = 0; i < AppliedMaterial.Nodes.Count; i++)
            {
                if (AppliedMaterial.Nodes[i].ID == "Output") Output = AppliedMaterial.Nodes[i];
            }
            if (Output == null) return false;
            string Fragment = GenerateFragment(Output);
            if (Fragment == "") return false;
            this._FragmentShaderOutput = Fragment;
            this._VertexShaderOutput = File.ReadAllText(Engine.Settings.LibPath + this._MaterialTranslatorType + "\\Generator\\Vertex.shader");
            if (_TexturesNumber > 0)
            {
                List<byte> Textures = new List<byte>();
                for (int i = 0; i < TexturesNumber; i++)
                {
                    _TextureBitmaps[i] = new Bitmap(_TextureBitmaps[i], new Size(256, 256));
                    Textures.AddRange(ShaderMaterialTranslator.ImageToByte(_TextureBitmaps[i]));
                }
                this._Textures = Textures.ToArray();
            }
            return true;
        }
        protected virtual string GenerateFragment(MaterialNode Out)
        {
            string FragmentCode = "";
            string FragmentHeader;
            List<string> UsedFunctions = new List<string>();
            List<string> FunctionCalls = new List<string>();
            MaterialNodeValue Surface = Out.Inputs[0];
            if (Surface.InputTarget != null)
            {
                FunctionCalls.Add("\t" + GenerateFragmentFinalizer(true, Surface.InputTarget.Parent.Name + "_" + Surface.InputTarget.Name));
                UsedFunctions.Add(Surface.InputTarget.Parent.FunctionID);
                ProcessCurrentNode(Surface.InputTarget.Parent, UsedFunctions, FunctionCalls);
            }
            else
            {
                FunctionCalls.Add(GenerateFragmentFinalizer(false, ""));
            }
            if (_TexturesNumber > 0) FragmentHeader = File.ReadAllText(Engine.Settings.LibPath + this._MaterialTranslatorType + "\\Generator\\FragmentTexturedHeader.shader") + "\n";
            else FragmentHeader = File.ReadAllText(Engine.Settings.LibPath + this._MaterialTranslatorType + "\\Generator\\FragmentHeader.shader") + "\n";
            if (FragmentHeader == "") return "";
            FragmentCode += FragmentHeader;
            FragmentCode += "\n";
            for (int i = 0; i < UsedFunctions.Count; i++)
            {
                bool Found = false;
                for(int j = 0; j < this._Entries.Count; j++)
                {
                    if (this._Entries[j].ID == UsedFunctions[i])
                    {
                        FragmentCode += this._Entries[j].FunctionCode;
                        FragmentCode += "\n";
                        FragmentCode += "\n";
                        Found = true;
                    }
                }
                if (!Found) return "";
            }
            FragmentCode += GenerateMainFunction(FunctionCalls.ToArray());
            File.WriteAllText(this._MaterialTranslatorType + "\\Generator\\Out.shader", FragmentCode);
            return FragmentCode;
        }
        public static byte[] ImageToByte(Bitmap bitmap)
        {
            BitmapData bmpdata = null;

            try
            {
                bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                int numbytes = bmpdata.Stride * bitmap.Height;
                byte[] bytedata = new byte[numbytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, bytedata, 0, numbytes);
                bitmap.UnlockBits(bmpdata);
                return bytedata;
            }
            catch { return null; }
        }
        private bool ProcessCurrentNode(MaterialNode CurNode, List<string> UsedFunctions, List<string> FunctionCalls)
        {
            List<string> ArgumentTexts = new List<string>();
            List<string> OutputNames = new List<string>();
            ShaderMaterialTranslatorEntry FunctionEntry = null;
            for(int i = 0; i < this._Entries.Count; i++)
            {
                if (CurNode.FunctionID == this._Entries[i].ID) FunctionEntry = this._Entries[i];
            }
            if (FunctionEntry == null) return false;
            if(!UsedFunctions.Contains(CurNode.FunctionID))UsedFunctions.Insert(0, CurNode.FunctionID);
            for(int i = 0; i < FunctionEntry.EngineInputs.Count; i++)
            {
                ArgumentTexts.Add(GenerateArgumentFromEngineVariable(FunctionEntry.EngineInputs[i]));
            }
            for(int i = 0; i < CurNode.Inputs.Count; i++)
            {
                if (CurNode.Inputs[i].Value.Type == MaterialValueType.TextureValue)
                {
                    if(CurNode.Inputs[i].Value.Value != "" && File.Exists(CurNode.Inputs[i].Value.Value))
                    {
                        ArgumentTexts.Add(TexturesNumber.ToString());
                        Bitmap Img = new Bitmap(Image.FromFile(CurNode.Inputs[i].Value.Value));
                        this._TextureBitmaps.Add(Img);
                        this.TexturesNumber++;
                    }
                    else ArgumentTexts.Add("-1");
                }
                else
                {
                    if (CurNode.Inputs[i].InputTarget != null) ArgumentTexts.Add(CurNode.Inputs[i].InputTarget.Parent.Name + "_" + CurNode.Inputs[i].InputTarget.Name);
                    else ArgumentTexts.Add(GenerateArgumentFromValue(CurNode.Inputs[i].Value));
                }
            }
            for (int i = 0; i < CurNode.Outputs.Count; i++)
            {
                OutputNames.Add(CurNode.Name + "_" + CurNode.Outputs[i].Name);
            }
            FunctionCalls.Insert(0,GenerateFunctionCall(CurNode.FunctionID, ArgumentTexts.ToArray(), OutputNames.ToArray()));
            for (int i = 0; i < CurNode.Inputs.Count; i++)
            {
                if (CurNode.Inputs[i].InputTarget != null) ProcessCurrentNode(CurNode.Inputs[i].InputTarget.Parent, UsedFunctions, FunctionCalls);
            }
            return true;
        }
        protected virtual string GenerateMainFunction(string[] Lines)
        {
            return "";
        }
        protected virtual string GenerateFragmentFinalizer(bool Input, string ID)
        {
            return "";
        }
        protected virtual string GenerateFunctionCall(string FunctionName, string[] ArgumentTexts, string[] OutputNames)
        {
            return "";
        }
        protected virtual string GenerateArgumentFromValue(MaterialValueHolder Value)
        {
            return "";
        }
        protected virtual string GenerateArgumentFromEngineVariable(string VariableName)
        {
            return "";
        }
        private void LoadEntries()
        {
            List<string> PossibleEntries = new List<string>();
            string[] Files = Directory.GetFiles(Engine.Settings.LibPath + _MaterialTranslatorType);
            for (int i = 0; i < Files.Length; i++)
            {
                if (!PossibleEntries.Contains(Path.GetFileNameWithoutExtension(Files[i]))) PossibleEntries.Add(Path.GetFileNameWithoutExtension(Files[i]));
            }
            for (int i = 0; i < PossibleEntries.Count; i++)
            {
                if (File.Exists(this._MaterialTranslatorType + "\\" + PossibleEntries[i] + ".xml") && File.Exists(this._MaterialTranslatorType + "\\" + PossibleEntries[i] + ".shader"))
                {
                    this._Entries.Add(new ShaderMaterialTranslatorEntry(this._MaterialTranslatorType + "\\" + PossibleEntries[i] + ".xml", this._MaterialTranslatorType));
                }
            }
        }
    }
}
