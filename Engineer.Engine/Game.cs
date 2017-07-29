using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engineer.Engine
{
    [XmlInclude(typeof(Scene))]
    [XmlInclude(typeof(Scene2D))]
    [XmlInclude(typeof(Scene3D))]
    public class Game
    {
        private string _Name;
        private List<SceneObject> _Assets;
        private List<Scene> _Scenes;
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
        [XmlIgnore]
        public List<SceneObject> Assets
        {
            get
            {
                return _Assets;
            }

            set
            {
                _Assets = value;
            }
        }
        [XmlIgnore]
        public List<Scene> Scenes
        {
            get
            {
                return _Scenes;
            }

            set
            {
                _Scenes = value;
            }
        }
        [XmlIgnore]
        public Dictionary<string, object> Data { get => _Data; set => _Data = value; }
        public Game()
        {
            this._Assets = new List<SceneObject>();
            this._Scenes = new List<Scene>();
            this._Data = new Dictionary<string, object>();
        }
        public Game(Game G)
        {
            this._Name = G._Name;
            this._Assets = new List<SceneObject>();
            for (int i = 0; i < G._Assets.Count; i++)
            {
                if (G._Assets[i].Type == SceneObjectType.DrawnSceneObject) this._Assets.Add(new DrawnSceneObject((DrawnSceneObject)G._Assets[i], null));
                else if (G._Assets[i].Type == SceneObjectType.ScriptSceneObject) this._Assets.Add(new ScriptSceneObject((ScriptSceneObject)G._Assets[i], null));
                else if (G._Assets[i].Type == SceneObjectType.SoundSceneObject) this._Assets.Add(new SoundSceneObject((SoundSceneObject)G._Assets[i], null));
            }
            this._Scenes = new List<Scene>();
            for(int i = 0; i < G._Scenes.Count; i++)
            {
                if (G._Scenes[i].Type == SceneType.Scene2D) this._Scenes.Add(new Scene2D((Scene2D)G._Scenes[i]));
                else if (G._Scenes[i].Type == SceneType.Scene3D) this._Scenes.Add(new Scene3D((Scene3D)G._Scenes[i]));
            }
        }
        public bool AddScene(Scene S)
        {
            this._Scenes.Add(S);
            this.Data[S.Name] = S;
            return true;
        }
        public List<Scene> GetScenesWithData(string DataKey)
        {
            List<Scene> Scenes = new List<Scene>();
            for(int i = 0; i < this._Scenes.Count; i++)
            {
                if (this._Scenes[i].Data.ContainsKey(DataKey)) Scenes.Add(this.Scenes[i]);
            }
            return Scenes;
        }
        public List<Scene> GetScenesWithData(string DataKey, object Value)
        {
            List<Scene> Scenes = new List<Scene>();
            for (int i = 0; i < this._Scenes.Count; i++)
            {
                if (this._Scenes[i].Data[DataKey].Equals(Value)) Scenes.Add(this._Scenes[i]);
            }
            return Scenes;
        }
        public List<SceneObject> GetAssetsWithData(string DataKey)
        {
            List<SceneObject> Assets = new List<SceneObject>();
            for (int i = 0; i < this._Assets.Count; i++)
            {
                if (this._Assets[i].Data.ContainsKey(DataKey)) Assets.Add(this._Assets[i]);
            }
            return Assets;
        }
        public List<SceneObject> GetAssetsWithData(string DataKey, object Value)
        {
            List<SceneObject> Assets = new List<SceneObject>();
            for (int i = 0; i < this._Assets.Count; i++)
            {
                if (this._Assets[i].Data[DataKey].Equals(Value)) Assets.Add(this._Assets[i]);
            }
            return Assets;
        }
        public static void Serialize(Game CurrentGame, string Path)
        {
            XmlSerializer Serializer = new XmlSerializer(typeof(Game));
            using (TextWriter Writer = new StreamWriter(Path))
            {
                Serializer.Serialize(Writer, CurrentGame);
            }
        }
        public static Game Deserialize(string Path)
        {
            XmlSerializer Deserializer = new XmlSerializer(typeof(Game));
            TextReader Reader = new StreamReader(Path);
            Game CurrentGame = (Game)Deserializer.Deserialize(Reader);
            Reader.Close();
            return CurrentGame;
        }
    }
}
