using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Engineer.Engine;

namespace Engineer.Draw
{
    public enum ShaderMaterialTranslatorEntryType
    {
        Generic = 0,
        Vertex = 1,
        Displacement = 2,
        Surface = 3,
        Texture = 4,
        Postprocess = 5
    }
    public class ShaderMaterialTranslatorEntryInput
    {
        private string _Name;
        private MaterialValueType _Type;
        private MaterialValueHolder _DefaultValue;
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
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
        public MaterialValueHolder DefaultValue
        {
            get
            {
                return _DefaultValue;
            }

            set
            {
                _DefaultValue = value;
            }
        }
        public ShaderMaterialTranslatorEntryInput()
        {
            this._Name = "";
            this._Type = MaterialValueType.VertexValue;
            this._DefaultValue = null;
        }
        public MaterialNodeValue ToMaterialNodeValue()
        {
            MaterialNodeValue NewValue = new MaterialNodeValue(this._Name, null);
            if (_DefaultValue == null) NewValue.Value = new MaterialValueHolder();
            else NewValue.Value = new MaterialValueHolder(_DefaultValue.X, _DefaultValue.Y, _DefaultValue.Z, _DefaultValue.W);
            NewValue.Value.Type = _Type;
            return NewValue;
        }
    }
    public class ShaderMaterialTranslatorEntryOutput
    {
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
            }
        }
        public ShaderMaterialTranslatorEntryOutput()
        {
            this._Name = "";
        }
        public MaterialNodeValue ToMaterialNodeValue()
        {
            MaterialNodeValue NewValue = new MaterialNodeValue(this._Name, null);
            NewValue.Value = new MaterialValueHolder();
            NewValue.Value.Type = MaterialValueType.Output;
            return NewValue;
        }
    }
    public class ShaderMaterialTranslatorEntry
    {
        private string _ID;
        private string _FunctionCode;
        private ShaderMaterialTranslatorEntryType _Type;
        private List<string> _EngineInputs;
        private List<ShaderMaterialTranslatorEntryInput> _Inputs;
        private List<ShaderMaterialTranslatorEntryOutput> _Outputs;
        public string ID
        {
            get
            {
                return _ID;
            }

            set
            {
                _ID = value;
            }
        }
        public string FunctionCode
        {
            get
            {
                return _FunctionCode;
            }

            set
            {
                _FunctionCode = value;
            }
        }
        public ShaderMaterialTranslatorEntryType Type
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
        public List<string> EngineInputs
        {
            get
            {
                return _EngineInputs;
            }

            set
            {
                _EngineInputs = value;
            }
        }
        public List<ShaderMaterialTranslatorEntryInput> Inputs
        {
            get
            {
                return _Inputs;
            }

            set
            {
                _Inputs = value;
            }
        }
        public List<ShaderMaterialTranslatorEntryOutput> Outputs
        {
            get
            {
                return _Outputs;
            }

            set
            {
                _Outputs = value;
            }
        }
        public ShaderMaterialTranslatorEntry(string FileRoot, string Type)
        {
            this._ID = "";
            this._FunctionCode = "";
            this._Type = ShaderMaterialTranslatorEntryType.Generic;
            this._EngineInputs = new List<string>();
            this._Inputs = new List<ShaderMaterialTranslatorEntryInput>();
            this._Outputs = new List<ShaderMaterialTranslatorEntryOutput>();
            XmlDocument Document = new XmlDocument();
            Document.Load(FileRoot);
            XmlNode Main = Document.FirstChild;
            for (int i = 0; i < Main.ChildNodes.Count; i++)
            {
                if (Main.ChildNodes[i].Name == "ID")
                {
                    this._ID = Main.ChildNodes[i].InnerText;
                }
                else if (Main.ChildNodes[i].Name == "Type")
                {
                    if (Main.ChildNodes[i].InnerText == "Vertex") this._Type = ShaderMaterialTranslatorEntryType.Vertex;
                    else if (Main.ChildNodes[i].InnerText == "Displacement") this._Type = ShaderMaterialTranslatorEntryType.Displacement;
                    else if (Main.ChildNodes[i].InnerText == "Surface") this._Type = ShaderMaterialTranslatorEntryType.Surface;
                    else if (Main.ChildNodes[i].InnerText == "Texture") this._Type = ShaderMaterialTranslatorEntryType.Texture;
                    else if (Main.ChildNodes[i].InnerText == "Postprocess") this._Type = ShaderMaterialTranslatorEntryType.Postprocess;
                    else this._Type = ShaderMaterialTranslatorEntryType.Generic;
                }
                else if (Main.ChildNodes[i].Name == "EngineInput")
                {
                    for (int j = 0; j < Main.ChildNodes[i].ChildNodes.Count; j++)
                    {
                        if (Main.ChildNodes[i].ChildNodes[j].Name == "Name")
                        {
                            this._EngineInputs.Add(Main.ChildNodes[i].ChildNodes[j].InnerText);
                        }
                    }
                }
                else if (Main.ChildNodes[i].Name == "Input")
                {
                    ShaderMaterialTranslatorEntryInput NewInput = new ShaderMaterialTranslatorEntryInput();
                    for (int j = 0; j < Main.ChildNodes[i].ChildNodes.Count; j++)
                    {
                        if (Main.ChildNodes[i].ChildNodes[j].Name == "Name")
                        {
                            NewInput.Name = Main.ChildNodes[i].ChildNodes[j].InnerText;
                        }
                        else if (Main.ChildNodes[i].ChildNodes[j].Name == "Type")
                        {
                            if (Main.ChildNodes[i].ChildNodes[j].InnerText == "Input") NewInput.Type = MaterialValueType.Input;
                            else if (Main.ChildNodes[i].ChildNodes[j].InnerText == "Output") NewInput.Type = MaterialValueType.Output;
                            else if (Main.ChildNodes[i].ChildNodes[j].InnerText == "Bool") NewInput.Type = MaterialValueType.BoolValue;
                            else if (Main.ChildNodes[i].ChildNodes[j].InnerText == "Int") NewInput.Type = MaterialValueType.IntValue;
                            else if (Main.ChildNodes[i].ChildNodes[j].InnerText == "Float") NewInput.Type = MaterialValueType.FloatValue;
                            else if (Main.ChildNodes[i].ChildNodes[j].InnerText == "Vertex") NewInput.Type = MaterialValueType.VertexValue;
                            else if (Main.ChildNodes[i].ChildNodes[j].InnerText == "Color") NewInput.Type = MaterialValueType.ColorValue;
                            else if (Main.ChildNodes[i].ChildNodes[j].InnerText == "Image") NewInput.Type = MaterialValueType.TextureValue;
                        }
                        else if (Main.ChildNodes[i].ChildNodes[j].Name == "Value")
                        {
                            NewInput.DefaultValue = new MaterialValueHolder();
                            for (int k = 0; k < Main.ChildNodes[i].ChildNodes[j].ChildNodes.Count; k++)
                            {
                                if (Main.ChildNodes[i].ChildNodes[j].Name == "R")
                                {
                                    NewInput.DefaultValue.X = Convert.ToInt32(Main.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerText);
                                }
                                if (Main.ChildNodes[i].ChildNodes[j].Name == "G")
                                {
                                    NewInput.DefaultValue.Y = Convert.ToInt32(Main.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerText);
                                }
                                if (Main.ChildNodes[i].ChildNodes[j].Name == "B")
                                {
                                    NewInput.DefaultValue.Z = Convert.ToInt32(Main.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerText);
                                }
                                if (Main.ChildNodes[i].ChildNodes[j].Name == "A")
                                {
                                    NewInput.DefaultValue.W = Convert.ToInt32(Main.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerText);
                                }
                                if (Main.ChildNodes[i].ChildNodes[j].Name == "X")
                                {
                                    NewInput.DefaultValue.X = Convert.ToInt32(Main.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerText);
                                }
                                if (Main.ChildNodes[i].ChildNodes[j].Name == "Y")
                                {
                                    NewInput.DefaultValue.Y = Convert.ToInt32(Main.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerText);
                                }
                                if (Main.ChildNodes[i].ChildNodes[j].Name == "Z")
                                {
                                    NewInput.DefaultValue.Z = Convert.ToInt32(Main.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerText);
                                }
                                if (Main.ChildNodes[i].ChildNodes[j].Name == "W")
                                {
                                    NewInput.DefaultValue.W = Convert.ToInt32(Main.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerText);
                                }
                                if (Main.ChildNodes[i].ChildNodes[j].Name == "Value")
                                {
                                    NewInput.DefaultValue.Value = Main.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerText;
                                }
                            }    
                        }
                    }
                    this.Inputs.Add(NewInput);
                }
                else if (Main.ChildNodes[i].Name == "Output")
                {
                    ShaderMaterialTranslatorEntryOutput NewOutput = new ShaderMaterialTranslatorEntryOutput();
                    for (int j = 0; j < Main.ChildNodes[i].ChildNodes.Count; j++)
                    {
                        if (Main.ChildNodes[i].ChildNodes[j].Name == "Name")
                        {
                            NewOutput.Name = Main.ChildNodes[i].ChildNodes[j].InnerText;
                        }
                    }
                    this.Outputs.Add(NewOutput);
                }
                else if (Main.ChildNodes[i].Name == "Function")
                {
                    this._FunctionCode = File.ReadAllText(Type + "\\" + Main.ChildNodes[i].InnerText);
                }
            }
        }
        public MaterialNode ToMaterialNode(Material Holder)
        {
            MaterialNode NewNode = new MaterialNode(this.ID, Holder);
            NewNode.FunctionID = this.ID;
            for(int i = 0; i < this.Inputs.Count; i++)
            {
                MaterialNodeValue Value = this.Inputs[i].ToMaterialNodeValue();
                Value.Parent = NewNode;
                NewNode.Inputs.Add(Value);
            }
            for (int i = 0; i < this.Outputs.Count; i++)
            {
                MaterialNodeValue Value = this.Outputs[i].ToMaterialNodeValue();
                Value.Parent = NewNode;
                NewNode.Outputs.Add(Value);
            }
            return NewNode;
        }
    }
}