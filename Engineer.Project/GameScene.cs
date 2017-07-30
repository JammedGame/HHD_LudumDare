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
        private Movement _Movement;
        private Player _Player1;
        private Player _Player2;
        public Player Player1 { get => _Player1; set => _Player1 = value; }
        public Player Player2 { get => _Player2; set => _Player2 = value; }
        public GameScene()
        {
            this._Name = "GameScene";                
        }
        public void Init()
        {
            this._Player1 = new Player(this);
            this._Player2 = new Player(this);
            Level.Generate(this, 0, new Player[] { this._Player1, this._Player2 });
            this._Movement = new Movement(_Player1, _Player2, this);
            ZoomManager ZM = new ZoomManager(this);
            this.Events.Extern.KeyPress += new GameEventHandler(this.KeyPress);
        }
        private void KeyPress(object Sender, EventArguments E)
        {
            if(E.KeyDown == KeyType.Escape)
            {
                ExternRunner Runner = (ExternRunner)this.Data["Runner"];
                Runner.SwitchScene("Menu", false);
                
            }
        }
    }
}

