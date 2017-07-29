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
    public class GameLogic
    {
        private Game _Game;
        private ExternRunner _Runner;
        public GameLogic()
        {
            this._Game = new Game();
            this._Game.Name = "Engineer Project";
            Scene2D Menu = new Menu();
            this._Game.AddScene(Menu);
            this._Runner = new ExternRunner((int)LocalSettings.Window.X, (int)LocalSettings.Window.Y, "Engineer Project");
            Menu.Data["Game"] = this._Game;
            Menu.Data["Runner"] = this._Runner;
            this._Runner.SetWindowState(LocalSettings.State);
            this._Runner.Init(this._Game);
        }
        public void Run()
        {
            this._Runner.SwitchScene("Menu");
            this._Runner.Run();
        }
    }
}
