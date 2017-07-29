using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engineer.Engine
{
    [XmlInclude(typeof(DrawObject))]
    [XmlInclude(typeof(Actor))]
    [XmlInclude(typeof(Background))]
    [XmlInclude(typeof(Camera))]
    [XmlInclude(typeof(Light))]
    [XmlInclude(typeof(Sprite))]
    [XmlInclude(typeof(Tile))]
    public class DrawnSceneObject : SceneObject
    {
        private DrawObject _Visual;
        [XmlIgnore]
        public bool Active
        {
            get
            {
                return _Visual.Active;
            }

            set
            {
                _Visual.Active = value;
            }
        }
        public override DrawObject Visual
        {
            get
            {
                return _Visual;
            }

            set
            {
                _Visual = value;
            }
        }
        public DrawnSceneObject() : base()
        {
            this.Type = SceneObjectType.DrawnSceneObject;
            this.Visual = null;
            this.Events = new EventsPackage(EventHandlersPackage.NewDrawnSceneObjectEventsPackage());
        }
        public DrawnSceneObject(string Name, DrawObject Visual) : base(Name)
        {
            this.Type = SceneObjectType.DrawnSceneObject;
            this.Visual = Visual;
            this.Events = new EventsPackage(EventHandlersPackage.NewDrawnSceneObjectEventsPackage());
        }
        public DrawnSceneObject(DrawnSceneObject DSO, Scene ParentScene) : base(DSO, ParentScene)
        {
            this.Type = SceneObjectType.DrawnSceneObject;
            if (DSO._Visual.Type == DrawObjectType.Actor) this._Visual = new Actor((Actor)DSO._Visual);
            else if (DSO._Visual.Type == DrawObjectType.Background) this._Visual = new Background((Background)DSO._Visual);
            else if (DSO._Visual.Type == DrawObjectType.Camera) this._Visual = new Camera((Camera)DSO._Visual);
            else if (DSO._Visual.Type == DrawObjectType.Light) this._Visual = new Light((Light)DSO._Visual);
            else if (DSO._Visual.Type == DrawObjectType.Sprite) this._Visual = new Sprite((Sprite)DSO._Visual);
            else if (DSO._Visual.Type == DrawObjectType.Tile) this._Visual = new Tile((Tile)DSO._Visual);
            this.Events = new EventsPackage(DSO.Events, ParentScene);
        }
        public static void Serialize(DrawnSceneObject CurrentDrawnSceneObject, string Path)
        {
            XmlSerializer Serializer = new XmlSerializer(typeof(DrawnSceneObject));
            using (TextWriter Writer = new StreamWriter(Path))
            {
                Serializer.Serialize(Writer, CurrentDrawnSceneObject);
            }
        }
        public static DrawnSceneObject Deserialize(string Path)
        {
            XmlSerializer Deserializer = new XmlSerializer(typeof(DrawnSceneObject));
            TextReader Reader = new StreamReader(Path);
            DrawnSceneObject CurrentDrawnSceneObject = (DrawnSceneObject)Deserializer.Deserialize(Reader);
            Reader.Close();
            return CurrentDrawnSceneObject;
        }
    }
}
