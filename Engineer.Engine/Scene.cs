using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engineer.Engine
{
    public enum SceneType
    {
        Scene2D = 0,
        Scene3D = 1
    }
    [XmlInclude(typeof(DictionaryEntry))]
    public class Scene
    {
        private string _ID;
        protected string _Name;
        protected SceneType _Type;
        protected Color _BackColor;
        private EventsPackage _Events;
        protected List<SceneObject> _Objects;
        private Dictionary<string, object> _Data;
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
        public SceneType Type
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
        public Color BackColor
        {
            get
            {
                return _BackColor;
            }

            set
            {
                _BackColor = value;
            }
        }
        [XmlIgnore]
        public EventsPackage Events
        {
            get
            {
                return _Events;
            }

            set
            {
                _Events = value;
            }
        }
        [XmlIgnore]
        public List<SceneObject> Objects
        {
            get
            {
                return _Objects;
            }
        }
        [XmlIgnore]
        public Dictionary<string, object> Data
        {
            get
            {
                return _Data;
            }

            set
            {
                _Data = value;
            }
        }
        public List<DictionaryEntry> IO_DataList
        {
            get
            {
                if (_Data == null) return null;
                List<DictionaryEntry> Entries = new List<DictionaryEntry>(_Data.Count);
                foreach (string Key in _Data.Keys)
                {
                    Entries.Add(new DictionaryEntry(Key, _Data[Key]));
                }
                return Entries;
            }
            set
            {
                foreach (DictionaryEntry Pair in value)
                {
                    if (!_Data.ContainsKey(Pair.Key))
                    {
                        _Data[Pair.Key] = Pair.Value;
                    }
                }
            }
        }
        [XmlIgnore]
        public List<ScriptSceneObject> Scripts
        {
            get
            {
                List<ScriptSceneObject> NewList = new List<ScriptSceneObject>();
                for (int i = 0; i < Objects.Count; i++)
                {
                    if (Objects[i].Type == SceneObjectType.ScriptSceneObject)
                    {
                        NewList.Add((ScriptSceneObject)Objects[i]);
                    }
                }
                return NewList;
            }
        }
        public virtual bool AddSceneObject(SceneObject Object)
        {
            Object.ParentScene = this;
            this._Objects.Add(Object);
            this.Data[Object.Name] = Object;
            return true;
        }
        public Scene()
        {
            this._ID = Guid.NewGuid().ToString();
            this._Name = this._ID;
            this._BackColor = Color.FromArgb(40, 40, 40);
            this._Objects = new List<SceneObject>();
            this._Events = new EventsPackage(EventHandlersPackage.NewSceneEventsPackage());
            this._Data = new Dictionary<string, object>();
        }
        public Scene(string Name)
        {
            this._ID = Guid.NewGuid().ToString();
            this._Name = Name;
            this._BackColor = Color.FromArgb(40, 40, 40);
            this._Objects = new List<SceneObject>();
            this._Events = new EventsPackage(EventHandlersPackage.NewSceneEventsPackage());
            this._Data = new Dictionary<string, object>();
        }
        public Scene(Scene S)
        {
            this._ID = Guid.NewGuid().ToString();
            this._Name = S.Name;
            this._Type = S._Type;
            this._BackColor = S._BackColor;
            this._Events = new EventsPackage(S._Events, this);
            this._Objects = new List<SceneObject>();
            for(int i = 0; i < S._Objects.Count; i++)
            {
                if (S._Objects[i].Type == SceneObjectType.DrawnSceneObject) this._Objects.Add(new DrawnSceneObject((DrawnSceneObject)S._Objects[i], this));
                else if (S._Objects[i].Type == SceneObjectType.ScriptSceneObject) this._Objects.Add(new ScriptSceneObject((ScriptSceneObject)S._Objects[i], this));
                else if (S._Objects[i].Type == SceneObjectType.SoundSceneObject) this._Objects.Add(new SoundSceneObject((SoundSceneObject)S._Objects[i], this));
            }
            this._Data = new Dictionary<string, object>(S.Data);
        }
        public List<SceneObject> GetObjectsWithData(string DataKey)
        {
            List<SceneObject> Objects = new List<SceneObject>();
            for (int i = 0; i < this._Objects.Count; i++)
            {
                if (this._Objects[i].Data.ContainsKey(DataKey)) Objects.Add(this._Objects[i]);
            }
            return Objects;
        }
        public List<SceneObject> GetObjectsWithData(string DataKey, object Value)
        {
            List<SceneObject> Objects = new List<SceneObject>();
            for (int i = 0; i < this._Objects.Count; i++)
            {
                if (this._Objects[i].Data[DataKey].Equals(Value)) Objects.Add(this._Objects[i]);
            }
            return Objects;
        }
    }
}
