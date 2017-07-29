using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engineer.Engine
{
    public class Scene3D : Scene
    {
        protected Camera _ActiveCamera;
        protected Camera _EditorCamera;
        public Camera ActiveCamera
        {
            get
            {
                return _ActiveCamera;
            }

            set
            {
                _ActiveCamera = value;
            }
        }
        public Camera EditorCamera
        {
            get
            {
                return _ActiveCamera;
            }

            set
            {
                _ActiveCamera = value;
            }
        }
        [XmlIgnore]
        public List<Actor> Actors
        {
            get
            {
                List<Actor> NewList = new List<Actor>();
                for (int i = 0; i < Objects.Count; i++)
                {
                    if (Objects[i].Type == SceneObjectType.DrawnSceneObject && ((DrawnSceneObject)Objects[i]).Visual.Type == DrawObjectType.Actor)
                        NewList.Add(((DrawnSceneObject)Objects[i]).Visual as Actor);
                }
                return NewList;
            }
        }
        [XmlIgnore]
        public List<Light> Lights
        {
            get
            {
                List<Light> NewList = new List<Light>();
                for (int i = 0; i < Objects.Count; i++)
                {
                    if (Objects[i].Type == SceneObjectType.DrawnSceneObject && ((DrawnSceneObject)Objects[i]).Visual.Type == DrawObjectType.Light)
                        NewList.Add(((DrawnSceneObject)Objects[i]).Visual as Light);
                }
                return NewList;
            }
        }
        [XmlIgnore]
        public List<Camera> Cameras
        {
            get
            {
                List<Camera> NewList = new List<Camera>();
                for(int i = 0; i < Objects.Count; i++)
                {
                    if (Objects[i].Type == SceneObjectType.DrawnSceneObject && ((DrawnSceneObject)Objects[i]).Visual.Type == DrawObjectType.Camera)
                        NewList.Add(((DrawnSceneObject)Objects[i]).Visual as Camera);
                }
                return NewList;
            }
        }
        [XmlIgnore]
        public Background Background
        {
            get
            {
                for (int i = 0; i < Objects.Count; i++)
                {
                    if (Objects[i].Type == SceneObjectType.DrawnSceneObject && ((DrawnSceneObject)Objects[i]).Visual.Type == DrawObjectType.Background)
                        return ((DrawnSceneObject)Objects[i]).Visual as Background;
                }
                return null;
            }
        }
        public override bool AddSceneObject(SceneObject Object)
        {
            if (Object.Type == SceneObjectType.DrawnSceneObject && ((DrawnSceneObject)Object).Visual.Type == DrawObjectType.Sprite) return false;
            Object.ParentScene = this;
            this._Objects.Add(Object);
            this.Data[Object.Name] = Object;
            return true;
        }
        public Scene3D() : base()
        {
            this._ActiveCamera = new Camera();
            this._EditorCamera = new Camera();
        }
        public Scene3D(string Name) : base(Name)
        {
            this._ActiveCamera = new Camera();
            this._EditorCamera = new Camera();
        }
        public Scene3D(Scene3D S3D) : base(S3D)
        {
            this._ActiveCamera = new Camera(S3D._ActiveCamera);
            this._EditorCamera = new Camera(S3D.EditorCamera);
        }
    }
}
