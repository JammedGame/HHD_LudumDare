using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engineer.Engine
{
    public class MaterialNode
    {
        private int _Index;
        private string _ID;
        private string _Name;
        private string _FunctionID;
        private object _Tags;
        private Material _Holder;
        private List<MaterialNodeValue> _Values;
        private List<MaterialNodeValue> _Inputs;
        private List<MaterialNodeValue> _Outputs;
        public int Index
        {
            get
            {
                return _Index;
            }

            set
            {
                _Index = value;
            }
        }
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
        public string FunctionID
        {
            get
            {
                return _FunctionID;
            }

            set
            {
                _FunctionID = value;
            }
        }
        public object Tags
        {
            get
            {
                return _Tags;
            }

            set
            {
                _Tags = value;
            }
        }
        [XmlIgnore]
        public Material Holder
        {
            get
            {
                return _Holder;
            }

            set
            {
                _Holder = value;
            }
        }
        public List<MaterialNodeValue> Values
        {
            get
            {
                return _Values;
            }

            set
            {
                _Values = value;
            }
        }
        public List<MaterialNodeValue> Inputs
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
        public List<MaterialNodeValue> Outputs
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
        public MaterialNode()
        {
            this._Index = -1;
            this._ID = Guid.NewGuid().ToString();
            this._Name = this._ID;
            this._Holder = null;
            this._Values = new List<MaterialNodeValue>();
            this._Inputs = new List<MaterialNodeValue>();
            this._Outputs = new List<MaterialNodeValue>();
        }
        public MaterialNode(string Name, Material Holder)
        {
            this._Name = Name;
            this._Index = Holder.Nodes.Count;
            this._ID = Guid.NewGuid().ToString();
            this._Holder = Holder;
            this._Values = new List<MaterialNodeValue>();
            this._Inputs = new List<MaterialNodeValue>();
            this._Outputs = new List<MaterialNodeValue>();
        }
        public MaterialNode(XmlNode XNode, Material Holder)
        {
            this._ID = Guid.NewGuid().ToString();
            this._Index = Holder.Nodes.Count;
            this._Holder = Holder;
            this._Values = new List<MaterialNodeValue>();
            this._Inputs = new List<MaterialNodeValue>();
            this._Outputs = new List<MaterialNodeValue>();
            for(int i = 0; i < XNode.ChildNodes.Count; i++)
            {
                if(XNode.ChildNodes[i].Name == "ID")
                {
                    this._ID = XNode.ChildNodes[i].InnerText;
                }
                else if (XNode.ChildNodes[i].Name == "Name")
                {
                    this._Name = XNode.ChildNodes[i].InnerText;
                }
                else if (XNode.ChildNodes[i].Name == "Function")
                {
                    this._FunctionID = XNode.ChildNodes[i].InnerText;
                    if (this._FunctionID == "") this._FunctionID = "None";
                }
                else if (XNode.ChildNodes[i].Name == "Value")
                {
                    this._Values.Add(new MaterialNodeValue(this, XNode.ChildNodes[i], MaterialValueType.Value));
                }
                else if (XNode.ChildNodes[i].Name == "Input")
                {
                    this._Inputs.Add(new MaterialNodeValue(this, XNode.ChildNodes[i], MaterialValueType.Input));
                }
                else if (XNode.ChildNodes[i].Name == "Output")
                {
                    this._Outputs.Add(new MaterialNodeValue(this, XNode.ChildNodes[i], MaterialValueType.Output));
                }
            }
        }
        public MaterialNode(MaterialNode Node, Material Holder)
        {
            this._Name = Node.Name;
            this._Index = Holder.Nodes.Count;
            this._ID = Guid.NewGuid().ToString();
            this._FunctionID = Node._FunctionID;
            if (Node.ID == "Output") this._ID = "Output";
            this._Holder = Holder;
            this._Values = new List<MaterialNodeValue>();
            for (int i = 0; i < Node.Values.Count; i++) this._Values.Add(new MaterialNodeValue(Node._Values[i], this));
            this._Inputs = new List<MaterialNodeValue>();
            for (int i = 0; i < Node.Inputs.Count; i++) this._Inputs.Add(new MaterialNodeValue(Node._Inputs[i], this));
            this._Outputs = new List<MaterialNodeValue>();
            for (int i = 0; i < Node.Outputs.Count; i++) this._Outputs.Add(new MaterialNodeValue(Node._Outputs[i], this));
        }
    }
}
