using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engineer.Mathematics;

namespace Engineer.Data
{
    public class Segment
    {
        private int _Index;
        private string _Name;
        private double _BoneLength;
        private object _Tag;
        private Segment _Parrent;
        private List<Vertex> _Translation;
        private List<Vertex> _Rotation;
        private List<Vertex> _Scale;
        private List<Segment> _Children;
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
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public double BoneLength
        {
            get { return _BoneLength; }
            set { _BoneLength = value; }
        }
        public object Tag
        {
            get
            {
                return _Tag;
            }

            set
            {
                _Tag = value;
            }
        }
        public Segment Parrent
        {
            get { return _Parrent; }
            set { _Parrent = value; }
        }
        public List<Vertex> Translation
        {
            get { return _Translation; }
            set { _Translation = value; }
        }
        public List<Vertex> Rotation
        {
            get { return _Rotation; }
            set { _Rotation = value; }
        }
        public List<Vertex> Scale
        {
            get { return _Scale; }
            set { _Scale = value; }
        }
        public List<Segment> Children
        {
            get
            {
                return _Children;
            }

            set
            {
                _Children = value;
            }
        }
        public Segment(Segment OldSegment)
        {
            this._Index = -1;
            this._Name = OldSegment.Name;
            this._BoneLength = OldSegment._BoneLength;
            this._Parrent = OldSegment._Parrent;
            this._Translation = new List<Vertex>(OldSegment._Translation);
            this._Rotation = new List<Vertex>(OldSegment._Rotation);
            this._Scale = new List<Vertex>(OldSegment._Scale);
            this._Children = new List<Segment>();
        }
        public Segment(string Name)
        {
            this._Index = -1;
            this._Name = Name;
            this._Parrent = null;
            this._BoneLength = 1;
            this._Translation = new List<Vertex>();
            this._Rotation = new List<Vertex>();
            this._Scale = new List<Vertex>();
            this._Children = new List<Segment>();
        }
        public Segment(string Name, Segment Parrent)
        {
            this._Index = -1;
            this._Name = Name;
            this._Parrent = Parrent;
            this._BoneLength = 1;
            this._Translation = new List<Vertex>();
            this._Rotation = new List<Vertex>();
            this._Scale = new List<Vertex>();
            this._Children = new List<Segment>();
        }
        public Segment(string Name, Segment Parrent, double BoneLength)
        {
            this._Index = -1;
            this._Name = Name;
            this._Parrent = Parrent;
            this._BoneLength = BoneLength;
            this._Translation = new List<Vertex>();
            this._Rotation = new List<Vertex>();
            this._Scale = new List<Vertex>();
            this._Children = new List<Segment>();
        }
        public Segment(string Name, Segment Parrent, List<Vertex> Translation, List<Vertex> Rotation, List<Vertex> Scale)
        {
            this._Index = -1;
            this._Name = Name;
            this._Parrent = Parrent;
            this._BoneLength = 1;
            this._Translation = Translation;
            this._Rotation = Rotation;
            this._Scale = Scale;
            this._Children = new List<Segment>();
        }
        public Segment(string Name, Segment Parrent, double BoneLength, List<Vertex> Translation, List<Vertex> Rotation, List<Vertex> Scale)
        {
            this._Index = -1;
            this._Name = Name;
            this._Parrent = Parrent;
            this._BoneLength = BoneLength;
            this._Translation = Translation;
            this._Rotation = Rotation;
            this._Scale = Scale;
            this._Children = new List<Segment>();
        }
    }
}
