using Engineer.Engine;
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
    public class Player:DrawnSceneObject
    {
        public static int id=1;
        private double _Heat;
        private double _MaxHeat;

        private Scene2D Scene;
        private Sprite HealthBar;

        public Player(Scene2D CScene)
        {
            this.Scene = CScene;
            MaxHeat = Heat = 100;

            //DrawnSceneObject Player = GameScene.CreateStaticTile("Player"+id++, ResourceManager.Images["kuglica_01"], new Vertex(100*id, 100 * id, 0), new Vertex(100 , 100 , 0), true);
            SpriteSet Player = new SpriteSet("Player" + id);
            Player.Sprite.Add(ResourceManager.Images["kuglica_01"]);

            Sprite PlayerSprite = new Sprite();
            PlayerSprite.SpriteSets.Add(Player);

            

            this.Visual = PlayerSprite;
            this.Visual.Scale = new Vertex(50, 50, 0);
            this.Visual.Translation = new Vertex(100 * id, 100 * id, 0);
            CScene.Events.Extern.TimerTick += new GameEventHandler(GameUpdate);

            id += 1;

            DrawnSceneObject GlowDSO;
            if (id == 2)
            {
                GlowDSO = new Glow("P" + id + "Glow", this, 300, Color.FromArgb(150, 255, 140, 0));
                GlowDSO.Data["ColorModel"] = 2;
            }
            else
            {
                GlowDSO = new Glow("P" + id + "Glow", this, 300, Color.FromArgb(150, 204, 0, 0));
                GlowDSO.Data["ColorModel"] = 1;
            }

            CScene.Objects.Insert(0, GlowDSO);
            CScene.Data[this.ID + "Glow"] = GlowDSO;

            this.Data["Player"] = true;

            ExternRunner Runner = (ExternRunner)Scene.Data["Runner"];

            float Right = 20;
            if (id == 2)
            {
                Right = Runner.Width - 320;
            }

            DrawnSceneObject HealthBar = GameHelpers.createSprite("progressbar", new Vertex(Right, 20, 0), new Vertex(300, 40, 0));
            DrawnSceneObject Health = GameHelpers.createSprite("progress", new Vertex(Right, 20, 0), new Vertex(300, 40, 0));
            HealthBar.Visual.Fixed = true;
            Health.Visual.Fixed = true;
            Scene.AddSceneObject(HealthBar);
            Scene.AddSceneObject(Health);
            this.HealthBar = (Sprite)Health.Visual;
        }

        public double Heat { get => _Heat; set => _Heat = value; }
        public double MaxHeat { get => _MaxHeat; set => _MaxHeat = value; }

        public void GameUpdate(Game G, EventArguments E)
        {

            foreach (SceneObject HeatSource in Scene.GetObjectsWithData("HeatSource"))
            {
                if (this.GetDistance((DrawnSceneObject)HeatSource) < 150)
                {
                    Heat = Math.Min(Heat + 5, MaxHeat);
                }
            }

            foreach (SceneObject HeatSource in Scene.GetObjectsWithData("ColdSource"))
            {
                if (this.GetDistance((DrawnSceneObject)HeatSource) < 150)
                {
                    Heat -= 3;
                }
            }

            foreach (SceneObject Fan in Scene.GetObjectsWithData("Fan"))
            {
                FanGlow Glow = (FanGlow)Scene.Data[Fan.ID+"Glow"];
                Sprite GlowSprite = (Sprite)Glow.Visual;
                if(GlowSprite.InCollision(this.Visual, Collision2DType.Rectangular))
                {
                    Heat -= 10;
                }
            }

            List<SceneObject> Players = Scene.GetObjectsWithData("Player");

            foreach(SceneObject SceneObj in Players)
            {
                Player Player = (Player)SceneObj;

                if (Player != this)
                {
                    double distance = Player.GetDistance(this);

                    distance = Math.Max(0, distance - 150);

                    if (distance == 0)
                    {
                        double CommonHeat = (Player.Heat + this.Heat) / 2;
                        if (this.Heat > CommonHeat)
                        {
                            double MyNewHeat = Math.Max(CommonHeat, this.Heat - 1);
                            double ILose = this.Heat - MyNewHeat;
                            Player.Heat += ILose;
                            this.Heat = MyNewHeat;
                        }
                    }
                    Heat -= distance / 150;
                }
            }
            float MinHeat = (float)Heat;
            if (MinHeat < 0) MinHeat = 0;
            float Value = MinHeat / (float)MaxHeat;
            DrawnSceneObject PlayerGlow = (DrawnSceneObject)Scene.Data[this.ID + "Glow"];
            if ((int)PlayerGlow.Data["ColorModel"] == 2)
            {
                Sprite Glow = (Sprite)PlayerGlow.Visual;
                Glow.Paint = Color.FromArgb(150, (int)(Value * 255), (int)(140 + (1 - Value) * 51), (int)((1 - Value) * 255));
            }
            else
            {
                Sprite Glow = (Sprite)PlayerGlow.Visual;
                Glow.Paint = Color.FromArgb(150, (int)(65 + Value * 149), (int)((1 - Value) * 105), (int)((1 - Value) * 205));
            }

            HealthBar.Scale = new Vertex(((float)Heat / (float)MaxHeat) * 300.0f, HealthBar.Scale.Y, 0);


            Heat = Math.Max(0, Heat);

            if (((Player)Players[0]).Heat == 0 && ((Player)Players[1]).Heat == 0)
            {
                ((DrawnSceneObject)Scene.Data["PressSpace"]).Active = true;
            }
        }

    }
}
