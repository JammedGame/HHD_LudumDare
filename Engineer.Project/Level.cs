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

                DrawnSceneObject Box = new DrawnSceneObject();

                SpriteSet Boxset = new SpriteSet("Box");
                Boxset.Sprite.Add(ResourceManager.Images["kutija_3"]);

                Sprite BoxSprite = new Sprite();
                BoxSprite.SpriteSets.Add(Boxset);

                Box.Visual = BoxSprite;
                Box.Visual.Scale = new Vertex(50, 50, 0);
                Box.Visual.Translation = new Vertex(400, 200, 0);

                Scene.AddSceneObject(Box);
            }
        }
    }
}
