using Engineer.Engine;
using Engineer.Mathematics;
using Engineer.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Project
{
    public class ZoomManager
    {
        private float XZoom = 1;
        private float YZoom = 1;
        private int XOffset = 300;
        private int YOffset = 200;
        private GameScene _Scene;
        private ExternRunner _Runner;
        public ZoomManager(GameScene Scene)
        {
            this._Scene = Scene;
            this._Runner = (ExternRunner)Scene.Data["Runner"];
            this._Scene.Events.Extern.TimerTick += new GameEventHandler(UpdateZoom);
        }
        private void UpdateZoom(object Sender, EventArguments E)
        {
            VertexBuilder P1 = new VertexBuilder(_Scene.Player1.Visual.Translation);
            VertexBuilder P2 = new VertexBuilder(_Scene.Player2.Visual.Translation);
            Vertex Diff = VertexBuilder.Abs(P1 - P2).ToVertex();
            Vertex Median = ((P1 + P2) * 0.5f).ToVertex();
            Vertex Translation = new Vertex(-Median.X + _Runner.Width / 2, -Median.Y + _Runner.Height / 2, 0);
            this._Scene.Transformation.Translation = Translation;
            if (Diff.X > 650) this.XZoom = 1 + (Diff.X - 650) / 1000.0f;
            else XZoom = 1;
            if (Diff.Y > 400) this.YZoom = 1 - (Diff.Y - 400) / 1000.0f;
            else YZoom = 1;
            //if (XZoom < YZoom) this._Scene.Transformation.Scale = new Vertex(XZoom, XZoom, 1);
            //else this._Scene.Transformation.Scale = new Vertex(YZoom, YZoom, 1);
        }
    }
}
