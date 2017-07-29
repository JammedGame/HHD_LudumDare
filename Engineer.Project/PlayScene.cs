using Engineer.Engine;
using Engineer.Mathematics;
using Engineer.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Project
{
    public class PlayScene : Scene2D
    {
        public static void Create(Scene2D CScene)
        {
            //DrawnSceneObject Back = CreateStaticSprite("Back", ResourceManager.Images["Back"], new Vertex(0, 0, 0), new Vertex(1920, 900, 0));
            //CScene.AddSceneObject(Back);
            DrawnSceneObject Surface = CreateStaticSprite("Surface", ResourceManager.Images["zid_01"], new Vertex(0, 900, 0), new Vertex(1920, 1000, 0), true);
            CScene.Data["Surface"] = Surface;
            CScene.AddSceneObject(Surface);

            DrawnSceneObject Player = CreateStaticSprite("Player", ResourceManager.Images["kuglica_01"], new Vertex(1920, 900, 0), new Vertex(1920, 1000, 0), true);
            CScene.Data["Player"] = Player;
            CScene.AddSceneObject(Player);


            //DrawnSceneObject Floor = CreateStaticSprite("Floor", ResourceManager.Images["Ceiling"], new Vertex(Location - 250, 850, 0), new Vertex(250, 50, 0), true, Collision2DType.Focus);
            //CScene.AddSceneObject(Floor);

            CreateLevel(CScene, 6, 4, 0, new int[] { 1, 0 }, new bool[] { false, true, false, false }, 2);
        }

        public static void CreateLevel(Scene2D CScene, int XLocation, int Length, int Level, int[] Enterances, bool[] Stairs, int Assets = 0)
        {
            int Location = XLocation * 300;
            DrawnSceneObject Wall = CreateStaticSprite("Wall", ResourceManager.Images["zid_01"], new Vertex(Location, Location, 0), new Vertex(30, 550, 0));
            CScene.AddSceneObject(Wall);

        }

            public static DrawnSceneObject CreateStaticSprite(string Name, System.Drawing.Bitmap Image, Vertex Positon, Vertex Size, bool Collision = false, Collision2DType ColType = Collision2DType.Focus)
        {
            Positon = new Vertex(Positon.X, Positon.Y, 0);
            Size = new Vertex(Size.X, Size.Y, 0);

            TileCollection BItmaps = new TileCollection(Image);

            Tile SomethingOnScene = new Tile();

            SomethingOnScene.Collection = BItmaps;

            SomethingOnScene.Translation = Positon;
            SomethingOnScene.Scale = Size;
            DrawnSceneObject Static = new DrawnSceneObject(Name, SomethingOnScene);
            if (Collision) Static.Data["Collision"] = ColType;
            return Static;
            }
        }
}
