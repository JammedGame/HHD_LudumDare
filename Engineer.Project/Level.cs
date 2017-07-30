using Engineer.Engine;
using Engineer.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Project
{
    public class Level
    {
        public static void Generate(Scene2D Scene, int Index, Player[] Players)
        {
            if(Index == 0)
            {
                Scene.BackColor = Color.BlueViolet;
                TiledImporter.Import(Scene, "Data/sample.tmx", 15, 15);
                Players[0].Visual.Translation = new Mathematics.Vertex(3 * 100 + 25, 2 * 100 + 25, 0);
                Players[1].Visual.Translation = new Mathematics.Vertex(2 * 100 + 25, 3 * 100 + 25, 0);
                Scene.AddSceneObject(Players[0]);
                Scene.AddSceneObject(Players[1]);

                GenerateBox(Scene, 4, 3);
            }
        }
        public static void GenerateBox(Scene2D Scene, int XLocation, int YLocation)
        {
            DrawnSceneObject Box = new DrawnSceneObject();

            SpriteSet Boxset = new SpriteSet("Box");
            Boxset.Sprite.Add(ResourceManager.Images["kutija_3"]);

            Sprite BoxSprite = new Sprite();
            BoxSprite.SpriteSets.Add(Boxset);

            Box.Visual = BoxSprite;
            Box.Visual.Scale = new Vertex(100, 100, 0);
            Box.Visual.Translation = new Vertex(XLocation * 100, YLocation * 100, 0);
            Box.Data["Collision"] = true;
            Box.Data["P1Coll"] = new CollisionModel();
            Box.Data["P2Coll"] = new CollisionModel();

            Scene.AddSceneObject(Box);
        }
    }
}
