using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml.Serialization;
using Engineer.Engine;
using Engineer.Data;
using System.Reflection;

namespace Engineer.Engine.IO
{
    public class EFXInterface : Interface
    {
        protected override void SaveObject(object ObjectToSave, string FilePath, bool Head)
        {
            string FileName = Path.GetFileNameWithoutExtension(FilePath);
            string DirPath = FilePath.Replace(Path.GetFileName(FilePath), "");
            XmlSerializer Serializer = new XmlSerializer(ObjectToSave.GetType());
            DirPath = DirPath + FileName + "/";
            Directory.CreateDirectory(DirPath);
            if (Head) WriteConfig(ObjectToSave.GetType(), DirPath + "data.conf");
            using (TextWriter Writer = new StreamWriter(DirPath + "data.xml"))
            {
                Serializer.Serialize(Writer, ObjectToSave);
            }
            if (ObjectToSave.GetType() == typeof(Actor))
            {
                Actor Current = ObjectToSave as Actor;
                for (int i = 0; i < Current.Geometries.Count; i++)
                {
                    OBJContainer OBJ = new OBJContainer();
                    OBJ.Geometries.Add(Current.Geometries[i]);
                    OBJ.Save(DirPath + "Geometries_" + i + ".obj", null);
                }
            }
            if (ObjectToSave.GetType() == typeof(SpriteSet))
            {
                SpriteSet Current = ObjectToSave as SpriteSet;
                for (int i = 0; i < Current.Sprite.Count; i++) Current.Sprite[i].Save(DirPath + "Sprite_" + i + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
            if (ObjectToSave.GetType() == typeof(Sprite))
            {
                Sprite Current = ObjectToSave as Sprite;
                for (int i = 0; i < Current.SpriteSets.Count; i++) SaveObject(Current.SpriteSets[i], DirPath + "SpriteSets_" + i + ".efx", false);
                for (int i = 0; i < Current.SubSprites.Count; i++) SaveObject(Current.SubSprites[i], DirPath + "SubSprites_" + i + ".efx", false);
            }
            if (ObjectToSave.GetType() == typeof(TileCollection))
            {
                TileCollection Current = ObjectToSave as TileCollection;
                for (int i = 0; i < Current.TileImages.Count; i++) Current.TileImages[i].Save(DirPath + "TileImage_" + i + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
            if (ObjectToSave.GetType() == typeof(Tile))
            {
                Tile Current = ObjectToSave as Tile;
                SaveObject(Current.Collection, DirPath + "TileCollection.efx", false);
            }
            if (ObjectToSave.GetType() == typeof(DrawnSceneObject))
            {
                DrawnSceneObject Current = ObjectToSave as DrawnSceneObject;
                SaveObject(Current.Visual, DirPath + "Visual.efx", true);
            }
            if (ObjectToSave.GetType() == typeof(Scene2D) || ObjectToSave.GetType() == typeof(Scene3D))
            {
                Scene Current = ObjectToSave as Scene;
                for (int i = 0; i < Current.Objects.Count; i++) SaveObject(Current.Objects[i], DirPath + "Objects_" + i + ".efx", true);
            }
            if (ObjectToSave.GetType() == typeof(Game))
            {
                Game Current = ObjectToSave as Game;
                for (int i = 0; i < Current.Scenes.Count; i++) SaveObject(Current.Scenes[i], DirPath + "Scenes_" + i + ".efx", true);
            }
        }
        protected override object LoadObject(string DirPath, Type FileType)
        {
            if(FileType == null)
            {
                object[] Config = ReadConfig(DirPath + "data.conf");
                string FileTypeString = Config[1] as string;
                Assembly ASM = typeof(Game).Assembly;
                FileType = ASM.GetType(FileTypeString);
            }
            XmlSerializer Deserializer = new XmlSerializer(FileType);
            TextReader Reader = new StreamReader(DirPath + "data.xml");
            object CurrentObject = Deserializer.Deserialize(Reader);
            Reader.Close();
            List<string> Files = new List<string>(Directory.EnumerateFiles(DirPath));
            List<string> Directories = new List<string>(Directory.EnumerateDirectories(DirPath));
            if (FileType == typeof(Actor))
            {
                Actor Current = CurrentObject as Actor;
                for (int i = 0; Files.Contains(DirPath + "Geometries_" + i + ".obj"); i++)
                {
                    OBJContainer OBJ = new OBJContainer();
                    OBJ.Load(DirPath + "Geometries_" + i + ".obj", null);
                    for(int j = 0; j < OBJ.Geometries.Count; j++) Current.Geometries.Add(OBJ.Geometries[j]);
                }
            }
            if (FileType == typeof(SpriteSet))
            {
                SpriteSet Current = CurrentObject as SpriteSet;
                for (int i = 0; Files.Contains(DirPath + "Sprite_" + i + ".png"); i++)
                {
                    Bitmap SpriteImage = null;
                    using (Image Img = Image.FromFile(DirPath + "Sprite_" + i + ".png"))
                    {
                        SpriteImage = new Bitmap(Img);
                    }
                    Current.Sprite.Add(SpriteImage);
                }
            }
            if (FileType == typeof(Sprite))
            {
                Sprite Current = CurrentObject as Sprite;
                for (int i = 0; Directories.Contains(DirPath + "SpriteSets_" + i); i++)
                {
                    SpriteSet Child = (SpriteSet)LoadObject(DirPath + "SpriteSets_" + i + "/", typeof(SpriteSet));
                    Current.SpriteSets.Add(Child);
                }
                for (int i = 0; Directories.Contains(DirPath + "SubSprites_" + i); i++)
                {
                    Sprite Child = (Sprite)LoadObject(DirPath + "SubSprites_" + i + "/", typeof(Sprite));
                    Current.SubSprites.Add(Child);
                }
            }
            if (FileType == typeof(TileCollection))
            {
                TileCollection Current = CurrentObject as TileCollection;
                for (int i = 0; Files.Contains(DirPath + "Sprite_" + i + ".png"); i++)
                {
                    Bitmap TileImage = null;
                    using (Image Img = Image.FromFile(DirPath + "Sprite_" + i + ".png"))
                    {
                        TileImage = new Bitmap(Img);
                    }
                    Current.TileImages.Add(TileImage);
                }
            }
            if (FileType == typeof(Tile))
            {
                Tile Current = CurrentObject as Tile;
                TileCollection Child = (TileCollection)LoadObject(DirPath + "TileCollection/", typeof(SpriteSet));
                Current.Collection = Child;
            }
            if (FileType == typeof(DrawnSceneObject))
            {
                DrawnSceneObject Current = CurrentObject as DrawnSceneObject;
                Current.Visual = (DrawObject)LoadObject(DirPath + "Visual/", null);
            }
            if (FileType == typeof(Scene2D) || FileType == typeof(Scene3D))
            {
                Scene Current = CurrentObject as Scene;
                for (int i = 0; Directories.Contains(DirPath + "Objects_" + i); i++)
                {
                    SceneObject Child = (SceneObject)LoadObject(DirPath + "Objects_" + i + "/", null);
                    Current.Objects.Add(Child);
                }
            }
            if (FileType == typeof(Game))
            {
                Game Current = CurrentObject as Game;
                for (int i = 0; Directories.Contains(DirPath + "Scenes_" + i); i++)
                {
                    Scene Child = (Scene)LoadObject(DirPath + "Scenes_" + i + "/", null);
                    Current.Scenes.Add(Child);
                }
            }
            return CurrentObject;
        }
        
    }
}
