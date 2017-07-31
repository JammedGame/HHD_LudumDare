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
            TileCollection Titles = new TileCollection();
            Titles.TileImages.Add(ResourceManager.Images["title"]);
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
            PlayTile.Scale = new Vertex(250, 60, 1);
            PlayTile.Translation = new Vertex(835, 800, 0);
            Tile QuitTile = new Tile();
            QuitTile.Collection = Buttons;
            QuitTile.SetIndex(1);
            QuitTile.Scale = new Vertex(250, 60, 1);
            QuitTile.Translation = new Vertex(835, 900, 0);
            Tile Title = new Tile();
            Title.Collection = Titles;
            Title.SetIndex(1);
            Title.Scale = new Vertex(1034, 152, 1);
            Title.Translation = new Vertex(630, 450, 0);
            DrawnSceneObject Back = new DrawnSceneObject("Back", BackTile);
            DrawnSceneObject Play = new DrawnSceneObject("Play", PlayTile);
            Play.Events.Extern.MouseClick += new GameEventHandler(this.PlayClick);
            DrawnSceneObject Quit = new DrawnSceneObject("Quit", QuitTile);
            Quit.Events.Extern.MouseClick += new GameEventHandler(this.QuitClick);
            DrawnSceneObject TitleDSO = new DrawnSceneObject("Title", Title);
            this.AddSceneObject(Back);
            this.AddSceneObject(Play);
            this.AddSceneObject(Quit);
            this.AddSceneObject(TitleDSO);
        }
        private void PlayClick(object sender, EventArguments e)
        {
            ExternRunner Runner = (ExternRunner)this.Data["Runner"];
            Runner.SwitchScene("LevelPicker", false);
        }
        private void QuitClick(object sender, EventArguments e)
        {
            ((ExternRunner)this.Data["Runner"]).Close();
        }
    }
}
