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
    public class Glow : DrawnSceneObject
    {
        public Glow(string Name, DrawnSceneObject Parent, int Size, Color Paint)
        {
            this.Name = Name;
            SpriteSet GlowSet = new SpriteSet("Glow");
            GlowSet.Sprite.Add(ResourceManager.Images["kuglica_01"]);
            Sprite Glow = new Sprite();
            Glow.SpriteSets.Add(GlowSet);
            this.Visual = Glow;
            Glow.Paint = Paint;
            Glow.Translation = new Vertex(Parent.Visual.Translation.X  - Size / 2 + Parent.Visual.Scale.X, Parent.Visual.Translation.Y - Size / 2 + Parent.Visual.Scale.Y, 0);
            Glow.Scale = new Vertex(Size, Size, 1);
        }
    }
}
