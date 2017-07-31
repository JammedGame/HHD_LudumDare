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
        private int _Size;
        private DrawnSceneObject _Parent;
        public Glow(string Name, DrawnSceneObject Parent, int Size, Color Paint)
        {
            this._Parent = Parent;
            this._Size = Size;
            this.Name = Name;
            SpriteSet GlowSet = new SpriteSet("Glow");
            GlowSet.Sprite.Add(ResourceManager.Images["kuglica_01"]);
            Sprite Glow = new Sprite();
            Glow.SpriteSets.Add(GlowSet);
            this.Visual = Glow;
            Glow.Paint = Paint;
            Glow.Translation = new Vertex(Parent.Visual.Translation.X  - Size / 2 + Parent.Visual.Scale.X / 2, Parent.Visual.Translation.Y - Size / 2 + Parent.Visual.Scale.Y / 2, 0);
            Glow.Scale = new Vertex(Size, Size, 1);
        }
        public void Update()
        {
            this.Visual.Translation = new Vertex(_Parent.Visual.Translation.X - _Size / 2 + _Parent.Visual.Scale.X / 2, _Parent.Visual.Translation.Y - _Size / 2 + _Parent.Visual.Scale.Y / 2, 0);
        }
    }
}
