﻿using Engineer.Engine;
using Engineer.Mathematics;
using Engineer.Runner;
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
        public static int LastGen = -1;
        public static int leverID = 0;
        public static int doorID = 0;
        public static int fanID = 0;

        public static void Generate(Scene2D Scene, int Index, Player[] Players)
        {
            Player.id = 0;
            leverID = 0;
            doorID = 0;
            fanID = 0;

            LastGen = Index;
            if (Index == 1)
            {
                TiledImporter.Import(Scene, "Data/Level01_D.tmx", 10, 10);
                Players[0].Visual.Translation = new Mathematics.Vertex(2 * 100 + 25, 7 * 100 + 25, 0);
                Players[1].Visual.Translation = new Mathematics.Vertex(3 * 100 + 25, 7 * 100 + 25, 0);
                Players[0].Data["OriginalLocation"] = Players[0].Visual.Translation;
                Players[1].Data["OriginalLocation"] = Players[1].Visual.Translation;
                Scene.AddSceneObject(Players[0]);
                Scene.AddSceneObject(Players[1]);
                                
                GenerateFire(Scene, 4, 3);
                GenerateExit(Scene, 5, 7);
                GenerateDescrip(Scene, 1);
            }
            if (Index == 2)
            {
                TiledImporter.Import(Scene, "Data/Level02_D.tmx", 15, 15);
                Players[0].Visual.Translation = new Mathematics.Vertex(3 * 100 + 25, 12 * 100 + 25, 0);
                Players[1].Visual.Translation = new Mathematics.Vertex(4 * 100 + 25, 12 * 100 + 25, 0);
                Players[0].Data["OriginalLocation"] = Players[0].Visual.Translation;
                Players[1].Data["OriginalLocation"] = Players[1].Visual.Translation;
                Scene.AddSceneObject(Players[0]);
                Scene.AddSceneObject(Players[1]);

                GenerateFire(Scene, 3, 5);
                GenerateCold(Scene,10,9);
                GenerateExit(Scene, 12, 7);
                GenerateDescrip(Scene, 2);
            }
            if (Index == 3)
            {
                TiledImporter.Import(Scene, "Data/Level03_D.tmx", 10, 10);
                Players[0].Visual.Translation = new Mathematics.Vertex(2 * 100 + 25, 7 * 100 + 25, 0);
                Players[1].Visual.Translation = new Mathematics.Vertex(4 * 100 + 25, 6 * 100 + 25, 0);
                Players[0].Data["OriginalLocation"] = Players[0].Visual.Translation;
                Players[1].Data["OriginalLocation"] = Players[1].Visual.Translation;
                Scene.AddSceneObject(Players[0]);
                Scene.AddSceneObject(Players[1]);
                GenerateBox(Scene, 4, 5);
                GenerateExit(Scene, 7, 4);
                GenerateDescrip(Scene, 3);
            }
            if (Index == 4)
            {
                TiledImporter.Import(Scene, "Data/Level04_D.tmx", 10, 10);
                Players[0].Visual.Translation = new Mathematics.Vertex(2 * 100 + 25, 2 * 100 + 25, 0);
                Players[1].Visual.Translation = new Mathematics.Vertex(7 * 100 + 25, 2 * 100 + 25, 0);
                Players[0].Data["OriginalLocation"] = Players[0].Visual.Translation;
                Players[1].Data["OriginalLocation"] = Players[1].Visual.Translation;
                Scene.AddSceneObject(Players[0]);
                Scene.AddSceneObject(Players[1]);
                Players[0].MaxHeat = 2000;
                Players[0].Heat = 2000;
                Players[1].MaxHeat = 2000;
                Players[1].Heat = 2000;
                GenerateLever(Scene, 7, 3, GenerateDoor(Scene, 2, 5));
                GenerateLever(Scene, 4, 7, GenerateDoor(Scene, 6, 4));
                GenerateLever(Scene, 7, 5, GenerateDoor(Scene, 5, 6));
                GenerateExit(Scene, 7, 7);
            }
            if (Index == 5)
            {
                TiledImporter.Import(Scene, "Data/Level05_D.tmx", 10, 10);
                Players[0].Visual.Translation = new Mathematics.Vertex(6 * 100 + 25, 5 * 100 + 25, 0);
                Players[1].Visual.Translation = new Mathematics.Vertex(6 * 100 + 25, 6 * 100 + 25, 0);
                Players[0].Data["OriginalLocation"] = Players[0].Visual.Translation;
                Players[1].Data["OriginalLocation"] = Players[1].Visual.Translation;
                Players[0].MaxHeat = 1000;
                Players[0].Heat = 1000;
                Players[1].MaxHeat = 1000;
                Players[1].Heat = 1000;
                Scene.AddSceneObject(Players[0]);
                Scene.AddSceneObject(Players[1]);
                GenerateBox(Scene, 2, 3);
                GeneratePresurePlate(Scene, 2, 7, GenerateDoor(Scene, 2, 5));
                GenerateExit(Scene, 7, 3);
            }
            if (Index == 6)
            {
                TiledImporter.Import(Scene, "Data/Level06_D.tmx", 16, 16);
                Players[0].Visual.Translation = new Mathematics.Vertex(1 * 100 + 25, 5 * 100 + 25, 0);
                Players[1].Visual.Translation = new Mathematics.Vertex(1 * 100 + 25, 6 * 100 + 25, 0);
                Players[0].Data["OriginalLocation"] = Players[0].Visual.Translation;
                Players[1].Data["OriginalLocation"] = Players[1].Visual.Translation;
                Scene.AddSceneObject(Players[0]);
                Scene.AddSceneObject(Players[1]);
                Players[0].MaxHeat = 800;
                Players[0].Heat = 800;
                Players[1].MaxHeat = 800;
                Players[1].Heat = 800;
                GenerateFire(Scene, 7, 8);
                GenerateFire(Scene, 7, 3);
                GeneratePresurePlate(Scene, 5, 7, GenerateFan(Scene, 5, 5, 0, 3));
                GeneratePresurePlate(Scene, 8, 3, GenerateFan(Scene, 8, 5, 2, 4));
                GeneratePresurePlate(Scene, 11, 7, GenerateFan(Scene, 11, 5, 0, 3));
                GenerateExit(Scene, 15, 5);
                GenerateDescrip(Scene, 4);
            }
            if (Index == 7)
            {
                TiledImporter.Import(Scene, "Data/Level07_D.tmx", 15, 15);
                Players[0].Visual.Translation = new Mathematics.Vertex(10 * 100 + 25, 10 * 100 + 25, 0);
                Players[1].Visual.Translation = new Mathematics.Vertex(10 * 100 + 25, 11 * 100 + 25, 0);
                Players[0].Data["OriginalLocation"] = Players[0].Visual.Translation;
                Players[1].Data["OriginalLocation"] = Players[1].Visual.Translation;
                Scene.AddSceneObject(Players[0]);
                Scene.AddSceneObject(Players[1]);

                GenerateLever(Scene, 11, 11, GenerateDoor(Scene, 6, 11));
                GenerateLever(Scene, 11, 8, GenerateDoor(Scene, 7, 6));
                GenerateLever(Scene, 5, 11, GenerateDoor(Scene, 8, 7));
                GenerateFan(Scene, 4, 2, 1, 7);
                GenerateHeater(Scene, 10, 7);
                GenerateFire(Scene, 11, 10);
                GenerateExit(Scene, 11, 2);
                GenerateDescrip(Scene, 5);
            }
            if (Index == 8)
            {
                TiledImporter.Import(Scene, "Data/Level08_D.tmx", 20, 20);
                Players[0].Visual.Translation = new Mathematics.Vertex(2 * 100 + 25, 5 * 100 + 25, 0);
                Players[1].Visual.Translation = new Mathematics.Vertex(2 * 100 + 25, 6 * 100 + 25, 0);
                Players[0].Data["OriginalLocation"] = Players[0].Visual.Translation;
                Players[1].Data["OriginalLocation"] = Players[1].Visual.Translation;
                Scene.AddSceneObject(Players[0]);
                Scene.AddSceneObject(Players[1]);
                GenerateLever(Scene, 10, 1, GenerateDoor(Scene, 4, 3));
                GenerateFan(Scene, 15, 1, 3, 5);
                GenerateBox(Scene, 11, 7);
                GenerateExit(Scene, 4, 0);
            }
            if (Index == 9)
            {
                TiledImporter.Import(Scene, "Data/Level09_D.tmx", 16, 16);
                Players[0].Visual.Translation = new Mathematics.Vertex(2 * 100 + 25, 12 * 100 + 25, 0);
                Players[1].Visual.Translation = new Mathematics.Vertex(3 * 100 + 25, 12 * 100 + 25, 0);
                Players[0].Data["OriginalLocation"] = Players[0].Visual.Translation;
                Players[1].Data["OriginalLocation"] = Players[1].Visual.Translation;

                Players[0].HeatRange = 150;
                Players[1].HeatRange = 150;

                Players[0].Heat = Players[0].MaxHeat = 1400;
                Players[1].Heat = Players[1].MaxHeat = 1400;


                GenerateFire(Scene, 3, 8);
                GenerateFire(Scene, 8, 4);
                GenerateCold(Scene, 4, 1);
                GenerateBox(Scene, 12, 6);
                GenerateExit(Scene, 13, 3);
                GenerateFan(Scene, 1, 3, 1, 3);
                GeneratePresurePlate(Scene, 13, 10, GenerateDoor(Scene, 11, 7));
                GenerateLever(Scene, 1, 1, GenerateDoor(Scene, 11, 3));

                Scene.AddSceneObject(Players[0]);
                Scene.AddSceneObject(Players[1]);
            }

            ExternRunner Runner = (ExternRunner)Scene.Data["Runner"];
            TileCollection SpaceCollection = new TileCollection(ResourceManager.Images["press_space"]);
            Tile PressSpace = new Tile();
            PressSpace.Scale = new Vertex(500, 50, 1);
            PressSpace.Translation = new Vertex(Runner.Width - 550, Runner.Height - 100, 0);
            PressSpace.Collection = SpaceCollection;
            PressSpace.Fixed = true;
            PressSpace.Active = false;
            DrawnSceneObject SpaceDSO = new DrawnSceneObject("PressSpace", PressSpace);
            Scene.AddSceneObject(SpaceDSO);
        }
        public static void GenerateBox(Scene2D Scene, int XLocation, int YLocation)
        {
            SpriteSet Boxset = new SpriteSet("Box");
            Boxset.Sprite.Add(ResourceManager.Images["kutija_3"]);

            Sprite BoxSprite = new Sprite();
            BoxSprite.SpriteSets.Add(Boxset);

            DrawnSceneObject Box = new DrawnSceneObject("Box", BoxSprite);
            Box.Visual.Scale = new Vertex(80, 80, 0);
            Box.Visual.Translation = new Vertex(XLocation * 100 + 10, YLocation * 100 + 10, 0);
            Box.Data["Box"] = true;
            Box.Data["OriginalLocation"] = new Vertex(XLocation * 100 + 10, YLocation * 100 + 10, 0);
            Box.Data["P1Coll"] = new CollisionModel();
            Box.Data["P2Coll"] = new CollisionModel();
            Box.Data["WallColl"] = new CollisionModel();

            Scene.AddSceneObject(Box);
        }
        public static void GenerateHeater(Scene2D Scene, int XLocation, int YLocation)
        {
            SpriteSet Boxset = new SpriteSet("Box");
            Boxset.Sprite.Add(ResourceManager.Images["grejac_2"]);

            Sprite BoxSprite = new Sprite();
            BoxSprite.SpriteSets.Add(Boxset);

            DrawnSceneObject Box = new DrawnSceneObject("Box", BoxSprite);
            Box.Visual.Scale = new Vertex(80, 80, 0);
            Box.Visual.Translation = new Vertex(XLocation * 100 + 10, YLocation * 100 + 10, 0);
            Box.Data["Box"] = true;
            Box.Data["OriginalLocation"] = new Vertex(XLocation * 100 + 10, YLocation * 100 + 10, 0);
            Box.Data["P1Coll"] = new CollisionModel();
            Box.Data["P2Coll"] = new CollisionModel();
            Box.Data["WallColl"] = new CollisionModel();
            Box.Data["HeatSource"] = true;
            Box.Data["Heater"] = true;

            DrawnSceneObject GlowDSO = new Glow(Box.ID + "Glow", Box, 300, Color.FromArgb(150, 204, 0, 0));
            Scene.Objects.Insert(0, GlowDSO);
            Scene.Data[Box.ID + "Glow"] = GlowDSO;

            Scene.AddSceneObject(Box);
        }
        public static void GenerateLever(Scene2D Scene, int XLocation, int YLocation, string TargetID)
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

            Lever.Data["Lever"] = true;
            Lever.Data["Target"] = TargetID;

            LeverSprite.UpdateSpriteSet("LeverUp");          

            Scene.AddSceneObject(Lever);
        }
        public static void GeneratePresurePlate(Scene2D Scene, int XLocation, int YLocation, string TargetID)
        {
            SpriteSet LeverSpriteSetUp = new SpriteSet("LeverUp");
            SpriteSet LeverSpriteSetDown = new SpriteSet("LeverDown");

            LeverSpriteSetUp.Sprite.Add(ResourceManager.Images["pressure_up"]);
            LeverSpriteSetDown.Sprite.Add(ResourceManager.Images["pressure_down"]);

            Sprite LeverSprite = new Sprite();
            LeverSprite.SpriteSets.Add(LeverSpriteSetUp);
            LeverSprite.SpriteSets.Add(LeverSpriteSetDown);

            DrawnSceneObject Lever = new DrawnSceneObject("Lever", LeverSprite);
            Lever.Visual.Scale = new Vertex(100, 100, 0);
            Lever.Visual.Translation = new Vertex(XLocation * 100, YLocation * 100, 0);

            Lever.Data["Plate"] = true;
            Lever.Data["Target"] = TargetID;

            LeverSprite.UpdateSpriteSet("LeverUp");

            Scene.AddSceneObject(Lever);
        }
        public static string GenerateDoor(Scene2D Scene, int XLocation, int YLocation)
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
            Scene.Data[Door.ID] = Door;
            Door.Data["Collision"] = Collision2DType.Rectangular;

            Door.Data["Door"] = true;

            Scene.AddSceneObject(Door);

            return Door.ID;
        }
        public static void GenerateFire(Scene2D Scene, int XLocation, int YLocation)
        {
            SpriteSet FireSpriteSet = new SpriteSet("Fire");
            for(int i = 0; i < 4; i++) FireSpriteSet.Sprite.Add(ResourceManager.Images["kotlarnica"+i]);


            Sprite FireSprite = new Sprite();
            FireSprite.SpriteSets.Add(FireSpriteSet);


            DrawnSceneObject Fire = new DrawnSceneObject("Fire", FireSprite);
            Fire.Visual.Scale = new Vertex(100, 100, 0);
            Fire.Visual.Translation = new Vertex(XLocation * 100, YLocation * 100, 0);
                                
            Fire.Data["HeatSource"] = true;
            Fire.Data["Collision"] = Collision2DType.Rectangular;

            DrawnSceneObject GlowDSO = new Glow(Fire.ID + "Glow", Fire, 300, Color.FromArgb(150, 204, 0, 0));
            Scene.Objects.Insert(0, GlowDSO);
            Scene.Data[Fire.ID + "Glow"] = GlowDSO;

            Scene.AddSceneObject(Fire);
        }
        public static string GenerateFan(Scene2D Scene, int XLocation, int YLocation, int Direction, int Range)
        {
            SpriteSet FanSpriteSet = new SpriteSet("Fan");
            for(int i = 0; i < 5; i++)
            { 
                FanSpriteSet.Sprite.Add(ResourceManager.Images["fan"+i]);
            }
            Sprite FanSprite = new Sprite();
            FanSprite.SpriteSets.Add(FanSpriteSet);

            Range *= 100;

            DrawnSceneObject Fan = new DrawnSceneObject("Fan", FanSprite);
            Fan.Visual.Scale = new Vertex(100, 100, 0);
            Fan.Visual.Translation = new Vertex(XLocation * 100, YLocation * 100, 0);
            Fan.Data["Fan"] = true;
            Fan.Data["Range"] = Range;
            Fan.Data["MaxRange"] = Range;
            Fan.Data["Direction"] = Direction;
            Fan.Data["Enabled"] = true;

            Scene.Data[Fan.ID] = Fan;

            DrawnSceneObject GlowDSO = new FanGlow(Fan);
            Scene.Objects.Insert(0, GlowDSO);
            Scene.Data[Fan.ID + "Glow"] = GlowDSO;

            Scene.AddSceneObject(Fan);

            return Fan.ID;
        }
        public static void GenerateCold(Scene2D Scene, int XLocation, int YLocation)
        {
            SpriteSet ColdSpriteSet = new SpriteSet("Cold");
            for (int i = 0; i < 4; i++) ColdSpriteSet.Sprite.Add(ResourceManager.Images["freezer_0" + (i+1)]);


            Sprite ColdSprite = new Sprite();
            ColdSprite.SpriteSets.Add(ColdSpriteSet);


            DrawnSceneObject Cold = new DrawnSceneObject("Cold", ColdSprite);
            Cold.Visual.Scale = new Vertex(100, 100, 0);
            Cold.Visual.Translation = new Vertex(XLocation * 100, YLocation * 100, 0);

            Cold.Data["ColdSource"] = true;
            Cold.Data["Collision"] = Collision2DType.Rectangular;

            DrawnSceneObject GlowDSO = new Glow(Cold.ID + "Glow", Cold, 400, Color.FromArgb(150, 65, 105, 205));
            Scene.Objects.Insert(0, GlowDSO);
            Scene.Data[Cold.ID + "Glow"] = GlowDSO;

            Scene.AddSceneObject(Cold);
        }
        public static void GenerateDescrip(Scene2D Scene, int Index)
        {
            ExternRunner Runner = (ExternRunner)Scene.Data["Runner"];
            TileCollection Collection = new TileCollection(ResourceManager.Images["tutorial_0"+Index]);
            Tile PressSpace = new Tile();
            PressSpace.Scale = new Vertex(500, 50, 1);
            PressSpace.Translation = new Vertex(Runner.Width - 550, Runner.Height - 150, 0);
            PressSpace.Collection = Collection;
            PressSpace.Fixed = true;
            DrawnSceneObject SpaceDSO = new DrawnSceneObject("Tutor", PressSpace);
            Scene.AddSceneObject(SpaceDSO);
        }
        public static void GenerateExit(Scene2D Scene, int XLocation, int YLocation)
        {
            SpriteSet DoorSpriteSet = new SpriteSet("Exit");
            DoorSpriteSet.Sprite.Add(ResourceManager.Images["exit"]);
            Sprite DoorSprite = new Sprite();
            DoorSprite.SpriteSets.Add(DoorSpriteSet);
            DrawnSceneObject Door = new DrawnSceneObject("Exit", DoorSprite);
            Door.Visual.Scale = new Vertex(100, 100, 0);
            Door.Visual.Translation = new Vertex(XLocation * 100, YLocation * 100, 0);      
            Scene.Data["ExitDoor"] = Door;
            Scene.AddSceneObject(Door);
        }
        public static void Reset(Scene2D Scene)
        {
            List<SceneObject> Boxes = Scene.GetObjectsWithData("Box");
            List<SceneObject> Fans = Scene.GetObjectsWithData("Fan");
            List<SceneObject> Doors = Scene.GetObjectsWithData("Door");
            List<SceneObject> Levers = Scene.GetObjectsWithData("Lever");
            List<SceneObject> Players = Scene.GetObjectsWithData("Player");
            for (int i = 0; i < Boxes.Count; i++)
            {
                Boxes[i].Visual.Translation = (Vertex)Boxes[i].Data["OriginalLocation"];
            }
            for (int i = 0; i < Doors.Count; i++)
            {
                Doors[i].Visual.Active = true;
                Doors[i].Data["Collision"] = true;
            }
            for (int i = 0; i < Fans.Count; i++)
            {
                Fans[i].Data["Enabled"] = true;
            }
            for (int i = 0; i < Levers.Count; i++)
            {
                ((Sprite)Levers[i].Visual).SetSpriteSet("LeverUp");
            }
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].Visual.Translation = (Vertex)Players[i].Data["OriginalLocation"];
                ((Player)Players[i]).Heat = ((Player)Players[i]).MaxHeat;
            }
            ((DrawnSceneObject)Scene.Data["PressSpace"]).Active = false;
        }
    }
}      