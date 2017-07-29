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
        private Movement Movement;
        private Player Player1;
        private Player Player2;
        public GameScene()
        {
            this._Name = "GameScene";
            TiledImporter.Import(this, "Data/sample.tmx", 15, 15);
            this.Player1 = new Player(this);
            this.Player2 = new Player(this);
            this.Movement = new Movement(Player1, Player2, this);
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

