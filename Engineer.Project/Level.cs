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
        public static int doorID = 0;
        
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
                TiledImporter.Import(Scene, "Data/TestLevel.tmx", 15, 15);
                Players[0].Visual.Translation = new Mathematics.Vertex(10 * 100 + 25, 10 * 100 + 25, 0);
                Players[1].Visual.Translation = new Mathematics.Vertex(10 * 100 + 25, 11 * 100 + 25, 0);
                Scene.AddSceneObject(Players[0]);
                Scene.AddSceneObject(Players[1]);

                GenerateLever(Scene, 11, 11);
                GenerateDoor(Scene, 6, 11);
                GenerateLever(Scene, 11, 8);
                GenerateDoor(Scene, 7, 6);
                GenerateDoor(Scene, 8, 7);
                GenerateLever(Scene, 5, 11);
                GenerateBox(Scene, 10, 7);
                GenerateFire(Scene, 11, 10);
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
            Scene.Data["Door"+ doorID++] = Door;
            Door.Data["Collision"] = Collision2DType.Rectangular;
            
            Scene.AddSceneObject(Door);
        }

        public static void GenerateFire(Scene2D Scene, int XLocation, int YLocation)
        {
            SpriteSet FireSpriteSet = new SpriteSet("Fire");
            FireSpriteSet.Sprite.Add(ResourceManager.Images["fire"]);


            Sprite FireSprite = new Sprite();
            FireSprite.SpriteSets.Add(FireSpriteSet);


            DrawnSceneObject Fire = new DrawnSceneObject("Fire", FireSprite);
            Fire.Visual.Scale = new Vertex(100, 100, 0);
            Fire.Visual.Translation = new Vertex(XLocation * 100, YLocation * 100, 0);

            //Box.Data["P1Coll"] = new CollisionModel();
            //Box.Data["P2Coll"] = new CollisionModel();            
            Fire.Data["HeatSource"] = true;
            Fire.Data["Collision"] = Collision2DType.Rectangular;

            Scene.AddSceneObject(Fire);
        }

        // Privremeno da ga napravimo
        /*public static void GenerateFire(Scene2D Scene, int XLocation, int YLocation)
        {
            DrawnSceneObject Fire = GameHelpers.createSprite("progress", new Vertex(600, 600, 0), new Vertex(100, 100, 0));
            Fire.Data["HeatSource"] = true;
            Scene.AddSceneObject(Fire);
        }*/
    }
}      