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
    public class FanGlow : DrawnSceneObject
    {
        private DrawnSceneObject _Fan;
        public FanGlow(DrawnSceneObject Fan)
        {
            this._Fan = Fan;
            Sprite Glow = new Sprite();
            this.Visual = Glow;
            Glow.Paint = Color.FromArgb(150, 0, 191, 255);
            this.Update();
        }
        public void Update()
        {
            int Direction = (int)this._Fan.Data["Direction"];
            int Range = (int)this._Fan.Data["Range"];
            float XLocation = this._Fan.Visual.Translation.X;
            float YLocation = this._Fan.Visual.Translation.Y;
            Sprite Glow = (Sprite)this.Visual;
            if (Direction == 0)
            {
                Glow.Translation = new Vertex(XLocation, YLocation - Range, 0);
                Glow.Scale = new Vertex(100, Range, 1);
            }
            else if (Direction == 1)
            {
                Glow.Translation = new Vertex(XLocation + 100, YLocation, 0);
                Glow.Scale = new Vertex(Range, 100, 1);
            }
            else if (Direction == 2)
            {
                Glow.Translation = new Vertex(XLocation, YLocation + 100, 0);
                Glow.Scale = new Vertex(100, Range, 1);
            }
            else if (Direction == 3)
            {
                Glow.Translation = new Vertex(XLocation - Range - 100, YLocation, 0);
                Glow.Scale = new Vertex(Range, 100, 1);
            }
        }
    }
}
