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
    public class GameScene : Scene2D
    {
        private int _Seed = 0;
        private int _BackColorChange;
        private int _BackColorValue;
        private Movement _Movement;
        private Player _Player1;
        private Player _Player2;
        public Player Player1 { get => _Player1; set => _Player1 = value; }
        public Player Player2 { get => _Player2; set => _Player2 = value; }
        public GameScene()
        {
            this._Name = "GameScene";
            this.BackColor = Color.Black;
            this.Events.Extern.TimerTick += new GameEventHandler(ColorUpdates);
            this._BackColorValue = 0;
        }
        public void Init()
        {
            ExternRunner Runner = (ExternRunner)this.Data["Runner"];
            this.Transformation.Scale = new Vertex(Runner.Height * 2 / 1080f, Runner.Height * 2 / 1080f, 1);
            this._Player1 = new Player(this);
            this._Player2 = new Player(this);
            Level.Generate(this, 0, new Player[] { this._Player1, this._Player2 });
            this._Movement = new Movement(_Player1, _Player2, this);
            ZoomManager ZM = new ZoomManager(this);
            this.Events.Extern.KeyPress += new GameEventHandler(this.KeyPress);

            // Privremeno da ga napravimo

            DrawnSceneObject Vatraaaa = GameHelpers.createSprite("progress", new Vertex(600, 600, 0), new Vertex(100, 100, 0));
            Vatraaaa.Data["HeatSource"] = true;
            this.AddSceneObject(Vatraaaa);
        }
        private void KeyPress(object Sender, EventArguments E)
        {
            if(E.KeyDown == KeyType.Escape)
            {
                ExternRunner Runner = (ExternRunner)this.Data["Runner"];
                Runner.SwitchScene("Menu", false);
            }
        }
        private void ColorUpdates(object Sender, EventArguments E)
        {
            this._Seed++;
            if (this._Seed % 3 != 0) return;
            this._BackColorValue += this._BackColorChange;
            this.BackColor = Color.FromArgb((int)(104 * (this._BackColorValue / 100.0f)), (int)(58 * (this._BackColorValue / 100.0f)), (int)(94 * (this._BackColorValue / 100.0f)));
            if (this._BackColorValue == 100) this._BackColorChange = -1;
            if (this._BackColorValue == 0) this._BackColorChange = +1;
        }
    }
}

