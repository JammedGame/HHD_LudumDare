using Engineer.Engine;
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
        public GameScene()
        {
            this._Name = "GameScene";
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
