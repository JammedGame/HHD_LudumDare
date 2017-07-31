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
            TileCollection Buttons = new TileCollection();
            Buttons.TileImages.Add(ResourceManager.Images["back_button"]);
            Tile BackTile = new Tile();
            BackTile.Collection = Backgrounds;
            BackTile.SetIndex(0);
            BackTile.Scale = LocalSettings.Scale;
            BackTile.Translation = new Vertex();
            Tile ReturnTile = new Tile();
            ReturnTile.Collection = Buttons;
            ReturnTile.SetIndex(0);
            ReturnTile.Scale = new Vertex(250, 60, 1);
            ReturnTile.Translation = new Vertex(800, 900, 0);
            DrawnSceneObject Back = new DrawnSceneObject("Back", BackTile);
            DrawnSceneObject Return = new DrawnSceneObject("Return", ReturnTile);
            Return.Events.Extern.MouseClick += new GameEventHandler(this.ReturnClick);
            this.AddSceneObject(Back);
            this.AddSceneObject(Return);
        }
        private void ReturnClick(object sender, EventArguments e)
        {

        }
    }
}
