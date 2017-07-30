using Engineer.Engine;
using Engineer.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Project
{
    class GameHelpers
    {
        private static int Counter = 0;

        public static DrawnSceneObject createSprite(string Image, Vertex position, Vertex size, string Name = null )
        {
            if (Name == null)
            {
                Name = "SomeObject" + (++Counter);
            }
            SpriteSet SomeSpriteSet = new SpriteSet(Name);
            SomeSpriteSet.Sprite.Add(ResourceManager.Images[Image]);
            Sprite SomeSprite = new Sprite();
            SomeSprite.SpriteSets.Add(SomeSpriteSet);
            SomeSprite.Scale = new Vertex(size.X, size.Y, 0);
            SomeSprite.Translation = new Vertex(position.X, position.Y, 0);

            DrawnSceneObject DrawnObject = new DrawnSceneObject();
            DrawnObject.Visual = SomeSprite;
            return DrawnObject;

        }
    }
}
