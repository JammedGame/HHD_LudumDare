using Engineer.Engine;
using Engineer.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Engineer.Project
{
    public class TiledImporter
    {
        private static int FieldSize = 150;
        public static void Import(Scene2D Scene, string FilePath, int Width, int Height)
        {
            var UserInfos = XDocument.Load(FilePath).Descendants("map")
            .Select(d => d.Elements().ToDictionary(e => e.Name.LocalName, e => (string)e)).ToList();
            int[,] Indices = new int[Width, Height];
            foreach (var UI in UserInfos)
            {
                foreach (var Item in UI)
                {
                    if (Item.Key == "layer")
                    {
                        string[] Values = Item.Value.Replace("\n", "").Split(',');
                        int Index = 0;
                        for (int i = 0; i < Width; i++)
                        {
                            for (int j = 0; j < Height; j++)
                            {
                                int Current = Convert.ToInt32(Values[Index]) - 1;
                                Indices[j, i] = Current;
                                Index++;
                            }
                        }
                        break;
                    }
                }
            }
            TileCollection Collection = new TileCollection();
            Collection.TileImages.Add(ResourceManager.Images["zid_08"]);
            TileMap Map = new TileMap();
            Map.FieldSize = TiledImporter.FieldSize;
            Map.MapCollection = Collection;
            Map.SetMap(new Point(15, 15), Indices);
            DrawnSceneObject DSO = new DrawnSceneObject("Map", Map);
            Scene.AddSceneObject(DSO);
        }
        private static TileCollection EmptyCollection = new TileCollection();
        private static void CreateCollisionTile(Scene2D Scene, int X, int Y)
        {
            Tile NewTile = new Tile();
            NewTile.Translation = new Vertex(X * TiledImporter.FieldSize, Y * TiledImporter.FieldSize, 0);
            DrawnSceneObject DSO = new DrawnSceneObject("Collider", NewTile);
            DSO.Data["Collision"] = Collision2DType.Rectangular;
            Scene.AddSceneObject(DSO);
        }
    }
}