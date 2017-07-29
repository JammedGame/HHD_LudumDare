using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace Engineer.Engine
{
    public class Material
    {
        public static Material Default;
        private bool _Modified;
        private string _Name;
        private string _ID;
        private object _Tags;
        private List<MaterialNode> _Nodes;
        public bool Modified
        {
            get
            {
                return _Modified;
            }

            set
            {
                _Modified = value;
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
        public List<MaterialNode> Nodes
        {
            get
            {
                return _Nodes;
            }

            set
            {
                _Nodes = value;
            }
        }
        public Material()
        {
            this._Modified = true;
            this._ID = Guid.NewGuid().ToString();
            this._Name = this._ID;
            this._Tags = null;
            this._Nodes = new List<MaterialNode>();
        }
        public Material(string Name)
        {
            this._Modified = true;
            this._Name = Name;
            this._ID = Guid.NewGuid().ToString();
            this._Tags = null;
            this._Nodes = new List<MaterialNode>();
        }
        public Material(XmlNode XNode)
        {
            this._Modified = true;
            this._Name = Guid.NewGuid().ToString();
            this._ID = Guid.NewGuid().ToString();
            this._Tags = null;
            this._Nodes = new List<MaterialNode>();
            for(int i = 0; i < XNode.ChildNodes.Count; i++)
            {
                if(XNode.ChildNodes[i].Name == "Name")
                {
                    this._Name = XNode.ChildNodes[i].InnerText;
                }
                else if (XNode.ChildNodes[i].Name == "MaterialNode")
                {
                    this._Nodes.Add(new MaterialNode(XNode.ChildNodes[i], this));
                }
            }
        }
        public Material(string Name, Material OldMaterial)
        {
            this._Modified = true;
            this._Name = Name;
            this._ID = Guid.NewGuid().ToString();
            this._Tags = OldMaterial._Tags;
            this._Nodes = new List<MaterialNode>(OldMaterial.Nodes.Count);
            for (int i = 0; i < OldMaterial.Nodes.Count; i++) this._Nodes.Add(new MaterialNode(OldMaterial.Nodes[i], this));
        }
        public bool IsNodeIDFree(string ID)
        {
            bool Free = true;
            for(int i = 0; i < this._Nodes.Count; i++)
            {
                if(this._Nodes[i].ID == ID)
                {
                    Free = false;
                    break;
                }
            }
            return Free;
        }
    }
}
