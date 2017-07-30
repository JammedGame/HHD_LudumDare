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
        public static int leverID = 0;
        public static void Generate(Scene2D Scene, int Index, Player[] Players)
        {
            if (Index == 0)
            {
                TiledImporter.Import(Scene, "Data/sample.tmx", 15, 15);
                Players[0].Visual.Translation = new Mathematics.Vertex(3 * 100 + 25, 2 * 100 + 25, 0);
                Players[1].Visual.Translation = new Mathematics.Vertex(2 * 100 + 25, 3 * 100 + 25, 0);
                Scene.AddSceneObject(Players[0]);
                Scene.AddSceneObject(Players[1]);

                GenerateBox(Scene, 3, 3);
                GenerateLever(Scene, 3, 8);
                GenerateDoor(Scene, 2, 6);
            }
            if (Index == 1)
            {
                TiledImporter.Import(Scene, "Data/untitled1.tmx", 15, 15);
                Players[0].Visual.Translation = new Mathematics.Vertex(3 * 100 + 25, 2 * 100 + 25, 0);
                Players[1].Visual.Translation = new Mathematics.Vertex(2 * 100 + 25, 3 * 100 + 25, 0);
                Scene.AddSceneObject(Players[0]);
                Scene.AddSceneObject(Players[1]);

                GenerateBox(Scene, 3, 3);
                GenerateLever(Scene, 3, 8);
                GenerateDoor(Scene, 2, 6);
            }
        }
        public static void GenerateBox(Scene2D Scene, int XLocation, int YLocation)
        {
            SpriteSet Boxset = new SpriteSet("Box");
            Boxset.Sprite.Add(ResourceManager.Images["kutija_3"]);

            Sprite BoxSprite = new Sprite();
            BoxSprite.SpriteSets.Add(Boxset);

            DrawnSceneObject Box = new DrawnSceneObject("Box", BoxSprite);
            Box.Visual.Scale = new Vertex(100, 100, 0);
            Box.Visual.Translation = new Vertex(XLocation * 100, YLocation * 100, 0);
            Box.Data["Box"] = true;
            Box.Data["P1Coll"] = new CollisionModel();
            Box.Data["P2Coll"] = new CollisionModel();
            Box.Data["WallColl"] = new CollisionModel();

            Scene.AddSceneObject(Box);
        }
        public static void GenerateLever(Scene2D Scene, int XLocation, int YLocation)
        {
            SpriteSet LeverSpriteSetUp = new SpriteSet("LeverUp");
            SpriteSet LeverSpriteSetDown = new SpriteSet("LeverDown");

            LeverSpriteSetUp.Sprite.Add(ResourceManager.Images["lever_up"]);
            LeverSpriteSetDown.Sprite.Add(ResourceManager.Images["lever_down"]);

            Sprite LeverSprite = new Sprite();
            LeverSprite.SpriteSets.Add(LeverSpriteSetUp);
            LeverSprite.SpriteSets.Add(LeverSpriteSetDown);

            DrawnSceneObject Lever = new DrawnSceneObject("Lever", LeverSprite);
            Lever.Visual.Scale = new Vertex(100, 100, 0);
            Lever.Visual.Translation = new Vertex(XLocation * 100, YLocation * 100, 0);
           
            ((Sprite)Lever.Visual).UpdateSpriteSet("LeverUp");
            Scene.Data["Lever"+ leverID++] = Lever;
            //Box.Data["P2Coll"] = new CollisionModel();            

            Scene.AddSceneObject(Lever);
        }

        public static void GenerateDoor(Scene2D Scene, int XLocation, int YLocation)
        {
            SpriteSet DoorSpriteSet = new SpriteSet("Door");
            DoorSpriteSet.Sprite.Add(ResourceManager.Images["door"]);
          

            Sprite DoorSprite = new Sprite();
            DoorSprite.SpriteSets.Add(DoorSpriteSet);
          

            DrawnSceneObject Door = new DrawnSceneObject("Door", DoorSprite);
            Door.Visual.Scale = new Vertex(100, 100, 0);
            Door.Visual.Translation = new Vertex(XLocation * 100, YLocation * 100, 0);

            //Box.Data["P1Coll"] = new CollisionModel();
            //Box.Data["P2Coll"] = new CollisionModel();            
            Scene.Data["Door"] = Door;
            Door.Data["Collision"] = Collision2DType.Rectangular;
            
            Scene.AddSceneObject(Door);
        }
    }
}      