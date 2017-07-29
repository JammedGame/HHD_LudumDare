using Engineer.Engine;
using Engineer.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Project
{
    public class LoadingScene : Scene2D
    {
        public LoadingScene()
        {
            this.Name = "LoadingScene";
            this.Transformation.Scale = new Vertex(LocalSettings.Window.Y / LocalSettings.Scale.Y, LocalSettings.Window.Y / LocalSettings.Scale.Y, 1);
            TileCollection Backgrounds = new TileCollection();
            Backgrounds.TileImages.Add(ResourceManager.Images["back"]);
            TileCollection BarCollection = new TileCollection();
            BarCollection.TileImages.Add(ResourceManager.Images["progressbar"]);
            TileCollection ProgressCollection = new TileCollection();
            ProgressCollection.TileImages.Add(ResourceManager.Images["progress"]);
            Tile BackTile = new Tile();
            BackTile.Collection = Backgrounds;
            BackTile.SetIndex(0);
            BackTile.Scale = LocalSettings.Scale;
            BackTile.Translation = new Vertex();
            Tile BarTile = new Tile();
            BarTile.Collection = BarCollection;
            BarTile.SetIndex(0);
            BarTile.Scale = new Vertex(1600, 100, 1);
            BarTile.Translation = new Vertex(120, 900, 0);
            Tile ProgressTile = new Tile();
            ProgressTile.Collection = ProgressCollection;
            ProgressTile.SetIndex(0);
            ProgressTile.Scale = new Vertex(0, 100, 1);
            ProgressTile.Translation = new Vertex(120, 900, 0);
            DrawnSceneObject Back = new DrawnSceneObject("Back", BackTile);
            DrawnSceneObject Bar = new DrawnSceneObject("Bar", BarTile);
            DrawnSceneObject Progress = new DrawnSceneObject("Progress", ProgressTile);
            this.AddSceneObject(Back);
            this.AddSceneObject(Bar);
            this.AddSceneObject(Progress);
            this.Events.Extern.OperationProgress += new GameEventHandler(ProgressUpdate);
        }
        public void Reset()
        {
            ((DrawnSceneObject)this.Data["Progress"]).Visual.Scale = new Vertex(0, 100, 1);
        }
        private void ProgressUpdate(object Sender, EventArguments E)
        {
            ((DrawnSceneObject)this.Data["Progress"]).Visual.Scale = new Vertex(E.Progress * 16, 100, 1);
        }
    }
}
