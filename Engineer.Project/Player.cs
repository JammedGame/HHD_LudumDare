using Engineer.Engine;
using Engineer.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Project
{
    class Player:DrawnSceneObject
    {
        public static int id=1;
        private int _Heat;                    

        public Player(Scene2D CScene)
        {
            _Heat = 100;
            //DrawnSceneObject Player = GameScene.CreateStaticTile("Player"+id++, ResourceManager.Images["kuglica_01"], new Vertex(100*id, 100 * id, 0), new Vertex(100 , 100 , 0), true);
            SpriteSet Player = new SpriteSet("Player"+id);
            Player.Sprite.Add(ResourceManager.Images["kuglica_01"]);

            Sprite PlayerSprite = new Sprite();
            PlayerSprite.SpriteSets.Add(Player);

            this.Visual = PlayerSprite;
            this.Visual.Scale = new Vertex(100, 100, 0);
            this.Visual.Translation = new Vertex(100*id, 100*id, 0);

            CScene.AddSceneObject(this);
        }

        public int Heat
        {
            get { return _Heat; }
            set { _Heat = value; }
        }
    }
}
