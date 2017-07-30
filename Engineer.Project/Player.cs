using Engineer.Engine;
using Engineer.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Project
{
    public class Player:DrawnSceneObject
    {
        public static int id=1;
        private int _Heat;
        private int MaxHeat;

        private Scene2D Scene;
        private Sprite HealthBar;

        public Player(Scene2D CScene)
        {
            this.Scene = CScene;
            MaxHeat = _Heat = 1000;

            //DrawnSceneObject Player = GameScene.CreateStaticTile("Player"+id++, ResourceManager.Images["kuglica_01"], new Vertex(100*id, 100 * id, 0), new Vertex(100 , 100 , 0), true);
            SpriteSet Player = new SpriteSet("Player" + id);
            Player.Sprite.Add(ResourceManager.Images["kuglica_01"]);

            Sprite PlayerSprite = new Sprite();
            PlayerSprite.SpriteSets.Add(Player);

            this.Visual = PlayerSprite;
            this.Visual.Scale = new Vertex(50, 50, 0);
            this.Visual.Translation = new Vertex(100 * id, 100 * id, 0);
            CScene.Events.Extern.TimerTick += new GameEventHandler(GameUpdate);


            this.Data["Player"] = true;

            DrawnSceneObject HealthBar = GameHelpers.createSprite("progressbar", new Vertex(20, 20, 0), new Vertex(300, 40, 0));

            this.HealthBar = (Sprite) HealthBar.Visual;
            this.HealthBar.Fixed = true;
           
            Scene.AddSceneObject(HealthBar);

        }

        public int Heat
        {
            get { return _Heat; }
            set { _Heat = value; }
        }

        public void GameUpdate(Game G, EventArguments E)
        {
            if (Scene.GetObjectsWithData("HeatSource").Count > 0)
            {

            }
            else if (Scene.GetObjectsWithData("ColdSource").Count > 0) {

            } else {

                List<SceneObject> Players = Scene.GetObjectsWithData("Player");

                foreach(SceneObject Player in Players)
                {
                    if (Player != this)
                    {
                        int b = 1;
                    }
                }

                this._Heat -= 1;
            }
            HealthBar.Scale = new Vertex(((float)_Heat / (float)MaxHeat) * 300.0f, HealthBar.Scale.Y, 0);
        }

    }
}
