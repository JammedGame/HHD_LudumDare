using Engineer.Engine;
using Engineer.Mathematics;
using Engineer.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engineer.Project
{
    public class GameLogic
    {
      
        private Game _Game;
        private ExternRunner _Runner;
        public GameLogic()
        {
            this._Game = new Game();
            this._Game.Name = "Dissipate";
            Scene2D Menu = new Menu();
            this._Game.AddScene(Menu);
            Scene2D Picker = new LevelPicker();
            this._Game.AddScene(Picker);
            this._Runner = new ExternRunner((int)LocalSettings.Window.X, (int)LocalSettings.Window.Y, "Dissipate");
            this._Runner.WindowBorder = OpenTK.WindowBorder.Fixed;
            //this._Runner = new ExternRunner(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, "Tekillah!");
            Menu.Data["Game"] = this._Game;
            Menu.Data["Runner"] = this._Runner;
            Picker.Data["Game"] = this._Game;
            Picker.Data["Runner"] = this._Runner;
            this._Runner.SetWindowState(LocalSettings.State);
            this._Runner.Init(this._Game);
            SoundSceneObject SSO = new SoundSceneObject("Data/Music.wav", "Music");
            SSO.PlayLooped();
            _Game.Data["Music"] = SSO;
        }
        public void Run()
        {
            this._Runner.SwitchScene("Menu");
            this._Runner.Run();
        }
    }
}
