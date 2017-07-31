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
        private float _GlobalScale;
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
            this._GlobalScale = _Runner.Height / 1080.0f;
            this._Scene.Events.Extern.TimerTick += new GameEventHandler(UpdateZoom);
        }
        private void UpdateZoom(object Sender, EventArguments E)
        {
            VertexBuilder P1 = new VertexBuilder(_Scene.Player1.Visual.Translation);
            VertexBuilder P2 = new VertexBuilder(_Scene.Player2.Visual.Translation);
            Vertex Diff = VertexBuilder.Abs(P1 - P2).ToVertex();
            Vertex Median = ((P1 + P2) * 0.5f).ToVertex();
            
            if (Diff.X > 325) this.XZoom = 1 - (Diff.X - 325) / 600.0f;
            else XZoom = 1;
            if (Diff.Y > 200) this.YZoom = 1 - (Diff.Y - 200) / 600.0f;
            else YZoom = 1;
            Vertex Translation = new Vertex(-Median.X * this._GlobalScale * 2 + 480, -Median.Y * this._GlobalScale * 2 + 270, 0);
            if (XZoom < YZoom)
            {
                if (XZoom < 0.5f) XZoom = 0.5f;
                Translation = new Vertex(-Median.X * XZoom * this._GlobalScale * 2 + 480, -Median.Y * XZoom * this._GlobalScale * 2 + 270, 0);
                this._Scene.Transformation.Scale = new Vertex(XZoom * this._GlobalScale * 2, XZoom * this._GlobalScale * 2, 1);
            }
            else
            {
                if (YZoom < 0.5f) YZoom = 0.5f;
                Translation = new Vertex(-Median.X * YZoom * this._GlobalScale * 2 + 480, -Median.Y * YZoom * this._GlobalScale * 2 + 270, 0);
                this._Scene.Transformation.Scale = new Vertex(YZoom * this._GlobalScale * 2, YZoom * this._GlobalScale * 2, 1);
            }
            this._Scene.Transformation.Translation = Translation;
        }
    }
}
