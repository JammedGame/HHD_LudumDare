using Engineer.Data;
using Engineer.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engineer.Engine
{
    public enum SceneObjectType
    {
        Undefined = 0,
        DrawnSceneObject = 1,
        ScriptSceneObject = 2,
        SoundSceneObject = 3
    }
    [XmlInclude(typeof(DictionaryEntry))]
    public class SceneObject
    {
        private string _Name;
        private string _ID;
        private SceneObjectType _Type;
        private Scene _ParentScene;
        private EventsPackage _Events;
        private Dictionary<string, object> _Data;
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
        public SceneObjectType Type
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
        public virtual DrawObject Visual
        {
            get
            {
                return null;
            }

            set
            {
            }
        }
        [XmlIgnore]
        public Scene ParentScene
        {
            get
            {
                return _ParentScene;
            }

            set
            {
                _ParentScene = value;
            }
        }
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
                Console.WriteLine("test");
                foreach (DictionaryEntry Pair in value)
                {
                    if (!_Data.ContainsKey(Pair.Key))
                    {
                        _Data[Pair.Key] = Pair.Value;
                    }
                }
            }
        }
        public SceneObject()
        {
            this._ID = Guid.NewGuid().ToString();
            this._Name = this._ID;
            this._Type = SceneObjectType.Undefined;
            this._Events = new EventsPackage();
            this._Data = new Dictionary<string, object>();
        }
        public SceneObject(string Name)
        {
            this._Name = Name;
            this._ID = Guid.NewGuid().ToString();
            this._Type = SceneObjectType.Undefined;
            this._Events = new EventsPackage();
            this._Data = new Dictionary<string, object>();
        }
        public SceneObject(SceneObject SO, Scene ParentScene)
        {
            this._Name = SO._Name;
            this._ID = Guid.NewGuid().ToString();
            this._Type = SO._Type;
            this._ParentScene = ParentScene;
            this._Events = new EventsPackage();
            this._Data = new Dictionary<string, object>(SO._Data);
        }
        public static SceneObject FromFile(string FilePath, Scene CurrentScene)
        {
            if (!File.Exists(FilePath)) return null;
            SceneObject New = null;
            if(FilePath.ToUpper().EndsWith(".OBJ") && CurrentScene.Type == SceneType.Scene3D)
            {
                OBJContainer OBJ = new OBJContainer();
                OBJ.Load(FilePath, null);
                if (OBJ.Geometries[0].Normals.Count == 0) OBJ.RecalculateNormals();
                OBJ.Repack();
                Actor NewActor = new Actor(OBJ, Path.GetFileNameWithoutExtension(FilePath));
                NewActor.Scale = new Vertex(0.001f, 0.001f, 0.001f);
                New = new DrawnSceneObject(NewActor.Name, NewActor);
                New.ParentScene = CurrentScene;
            }
            return New;
        }

        public float GetDistance(DrawnSceneObject Other)
        {
            return (float)Math.Sqrt(Math.Pow(this.Visual.Translation.X - Other.Visual.Translation.X, 2) + Math.Pow(this.Visual.Translation.Y - Other.Visual.Translation.Y, 2));

        }
    }
}
