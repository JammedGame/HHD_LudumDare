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
    public class Menu : Scene2D
    {
        public Menu()
        {
            this.Name = "Menu";
            this.Transformation.Scale = new Vertex(LocalSettings.Window.Y / LocalSettings.Scale.Y, LocalSettings.Window.Y / LocalSettings.Scale.Y, 1);
            TileCollection Backgrounds = new TileCollection();
            Backgrounds.TileImages.Add(ResourceManager.Images["back"]);
            TileCollection Buttons = new TileCollection();
            Buttons.TileImages.Add(ResourceManager.Images["play"]);
            Buttons.TileImages.Add(ResourceManager.Images["quit"]);
            Buttons.TileImages.Add(ResourceManager.Images["settings"]);
            Tile BackTile = new Tile();
            BackTile.Collection = Backgrounds;
            BackTile.SetIndex(0);
            BackTile.Scale = LocalSettings.Scale;
            BackTile.Translation = new Vertex();
            Tile PlayTile = new Tile();
            PlayTile.Collection = Buttons;
            PlayTile.SetIndex(0);
            PlayTile.Scale = new Vertex(300, 150, 1);
            PlayTile.Translation = new Vertex(100, 100, 0);
            Tile QuitTile = new Tile();
            QuitTile.Collection = Buttons;
            QuitTile.SetIndex(1);
            QuitTile.Scale = new Vertex(300, 150, 1);
            QuitTile.Translation = new Vertex(100, 350, 0);
            DrawnSceneObject Back = new DrawnSceneObject("Back", BackTile);
            DrawnSceneObject Play = new DrawnSceneObject("Play", PlayTile);
            Play.Events.Extern.MouseClick += new GameEventHandler(this.PlayClick);
            DrawnSceneObject Quit = new DrawnSceneObject("Quit", QuitTile);
            Quit.Events.Extern.MouseClick += new GameEventHandler(this.QuitClick);
            this.AddSceneObject(Back);
            this.AddSceneObject(Play);
            this.AddSceneObject(Quit);
        }
        private void PlayClick(object sender, EventArguments e)
        {
            Game CurrentGame = (Game)this.Data["Game"];
            ExternRunner Runner = (ExternRunner)this.Data["Runner"];
            GameScene OldGame = null;
            if (CurrentGame.Data.ContainsKey("GameScene"))
            {
                OldGame = (GameScene)CurrentGame.Data["GameScene"];
                CurrentGame.Scenes.Remove(OldGame);
            }
            GameScene NewGame = new GameScene();
            NewGame.Data["Game"] = CurrentGame;
            NewGame.Data["Runner"] = Runner;
            NewGame.Init();
            LoadingScene Loading;
            if (!CurrentGame.Data.ContainsKey("LoadingScene"))
            {
                Loading = new LoadingScene();
                CurrentGame.AddScene(Loading);
                Loading.Data["Game"] = CurrentGame;
                Loading.Data["Runner"] = Runner;
            }
            else
            {
                Loading = (LoadingScene)CurrentGame.Data["LoadingScene"];
                Loading.Reset();
            }
            Runner.SwitchScene("LoadingScene", false);
            CurrentGame.AddScene(NewGame);
            Runner.SwitchScene("GameScene");
        }
        private void QuitClick(object sender, EventArguments e)
        {
            ((ExternRunner)this.Data["Runner"]).Close();
        }
    }
}
