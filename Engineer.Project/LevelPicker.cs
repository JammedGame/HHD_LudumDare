using Engineer.Engine;
using Engineer.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Project
{
    public class LevelPicker : Scene2D
    {
        public LevelPicker()
        {
            this.Name = "LevelPicker";
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
            PlayTile.Translation = new Vertex(800, 800, 0);
            Tile QuitTile = new Tile();
            QuitTile.Collection = Buttons;
            QuitTile.SetIndex(1);
            QuitTile.Scale = new Vertex(250, 60, 1);
            QuitTile.Translation = new Vertex(800, 900, 0);
            Tile Title = new Tile();
            Title.Collection = Titles;
            Title.SetIndex(1);
            Title.Scale = new Vertex(1034, 152, 1);
            Title.Translation = new Vertex(630, 450, 0);
            DrawnSceneObject Back = new DrawnSceneObject("Back", BackTile);
            DrawnSceneObject Return = new DrawnSceneObject("Return", PlayTile);
        }
    }
}
