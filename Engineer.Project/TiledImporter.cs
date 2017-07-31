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
    public class CollisionElement
    {
        public Point Location;
        public Point Size;
    }
    public class TiledImporter
    {
        private static int FieldSize = 100;
        public static void Import(Scene2D Scene, string FilePath, int Width, int Height)
        {
            var UserInfos = XDocument.Load(FilePath).Descendants("map")
            .Select(d => d.Elements().ToDictionary(e => e.Name.LocalName, e => (string)e)).ToList();
            int[,] Indices = new int[Height, Width];
            int[,] Collision = new int[Height, Width];
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
                    }
                    if (Item.Key == "collision")
                    {
                        string[] Values = Item.Value.Replace("\n", "").Split(',');
                        int Index = 0;
                        for (int i = 0; i < Width; i++)
                        {
                            for (int j = 0; j < Height; j++)
                            {
                                Collision[j, i] = Convert.ToInt32(Values[Index]);
                                Index++;
                            }
                        }
                    }
                }
            }
            ColliderColleciton = new List<CollisionElement>();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (Collision[i, j] == 0) GenerateCollider(Scene, Collision, i, j, Height, Width);
                }
            }
            for (int i = 0; i < ColliderColleciton.Count; i++)
            {
                CreateCollisionTile(Scene, ColliderColleciton[i].Location.X, ColliderColleciton[i].Location.Y, ColliderColleciton[i].Size.X, ColliderColleciton[i].Size.Y);
            }
            TileCollection Collection = new TileCollection();
            for (int i = 0; i < 12; i++)
            {
                Collection.TileImages.Add(ResourceManager.Images["tile_"+i]);
            }
            TileMap Map = new TileMap();
            Map.FieldSize = TiledImporter.FieldSize;
            Map.MapCollection = Collection;
            Map.SetMap(new Point(15, 15), Indices);
            DrawnSceneObject DSO = new DrawnSceneObject("Map", Map);
            Scene.AddSceneObject(DSO);
        }
        private static List<CollisionElement> ColliderColleciton;
        private static void GenerateCollider(Scene2D Scene, int[,] Collider, int X, int Y, int Width, int Height)
        {
            CollisionElement Element = new CollisionElement();
            Element.Location.X = X;
            Element.Location.Y = Y;
            Element.Size.X = 1;
            Element.Size.Y = 1;
            for (int i = X-1; i >= 0; i--)
            {
                if (Collider[i, Y] == 0)
                {
                    Element.Location.X = i;
                    Element.Size.X++;
                }
                else break;
            }
            for (int i = X + 1; i < Width; i++)
            {
                if (Collider[i, Y] == 0)
                {
                    Element.Size.X++;
                }
                else break;
            }
            for (int i = Y - 1; i >= 0; i--)
            {
                bool Viable = true;
                for (int j = Element.Location.X; j < Element.Location.X + Element.Size.X; j++)
                {
                    if (Collider[j, i] >= 1)
                    {
                        Viable = false;
                    }
                }
                if (Viable)
                {
                    Element.Location.Y = i;
                    Element.Size.Y++;
                }
                else break;
            }
            for (int i = Y + 1; i < Height; i++)
            {
                bool Viable = true;
                for (int j = Element.Location.X; j < Element.Location.X + Element.Size.X; j++)
                {
                    if (Collider[j, i] >= 1)
                    {
                        Viable = false;
                    }
                }
                if (Viable)
                {
                    Element.Size.Y++;
                }
                else break;
            }
            bool New = true;
            for(int i = 0; i < ColliderColleciton.Count; i++)
            {
                if (ColliderColleciton[i].Location.X == Element.Location.X &&
                    ColliderColleciton[i].Location.Y == Element.Location.Y &&
                    ColliderColleciton[i].Size.X == Element.Size.X &&
                    ColliderColleciton[i].Size.Y == Element.Size.Y) New = false;
            }
            if (New) ColliderColleciton.Add(Element);
        }
        private static TileCollection EmptyCollection = new TileCollection();
        private static void CreateCollisionTile(Scene2D Scene, int X, int Y, int XSize, int YSize)
        {
            Tile NewTile = new Tile();
            //NewTile.Paint = Color.Red;
            NewTile.Translation = new Vertex(X * TiledImporter.FieldSize, Y * TiledImporter.FieldSize, 0);
            NewTile.Scale = new Vertex(XSize * TiledImporter.FieldSize, YSize * TiledImporter.FieldSize, 1);
            NewTile.Active = false;
            DrawnSceneObject DSO = new DrawnSceneObject("Collider", NewTile);
            DSO.Data["Collision"] = Collision2DType.Rectangular;
            Scene.AddSceneObject(DSO);
        }
    }
}