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
    public class LevelPicker : Scene2D
    {
        public int Max = 0;
        public LevelPicker()
        {
            this.Name = "LevelPicker";
            Max = PlayerProgress.Read();
            this.Transformation.Scale = new Vertex(LocalSettings.Window.Y / LocalSettings.Scale.Y, LocalSettings.Window.Y / LocalSettings.Scale.Y, 1);
            TileCollection Backgrounds = new TileCollection();
            Backgrounds.TileImages.Add(ResourceManager.Images["back"]);
            TileCollection Buttons = new TileCollection();
            Buttons.TileImages.Add(ResourceManager.Images["back_button"]);
            TileCollection Levels = new TileCollection();
            Levels.TileImages.Add(ResourceManager.Images["lvl1"]);
            Levels.TileImages.Add(ResourceManager.Images["lvl2"]);
            Levels.TileImages.Add(ResourceManager.Images["lvl3"]);
            Levels.TileImages.Add(ResourceManager.Images["lvl2"]);
            TileCollection BackForth = new TileCollection();
            BackForth.TileImages.Add(ResourceManager.Images["left"]);
            BackForth.TileImages.Add(ResourceManager.Images["right"]);
            Tile BackTile = new Tile();
            BackTile.Collection = Backgrounds;
            BackTile.SetIndex(0);
            BackTile.Scale = LocalSettings.Scale;
            BackTile.Translation = new Vertex();
            Tile ReturnTile = new Tile();
            ReturnTile.Collection = Buttons;
            ReturnTile.SetIndex(0);
            ReturnTile.Scale = new Vertex(250, 60, 1);
            ReturnTile.Translation = new Vertex(835, 900, 0);
            Tile LevelDisplay = new Tile();
            LevelDisplay.Collection = Levels;
            LevelDisplay.SetIndex(Max);
            LevelDisplay.Scale = new Vertex(1000, 600, 1);
            LevelDisplay.Translation = new Vertex(460, 200, 0);
            Tile Left = new Tile();
            Left.Collection = BackForth;
            Left.SetIndex(0);
            Left.Scale = new Vertex(80, 140, 1);
            Left.Translation = new Vertex(340, 450, 0);
            Tile Right = new Tile();
            Right.Collection = BackForth;
            Right.SetIndex(1);
            Right.Scale = new Vertex(80, 140, 1);
            Right.Translation = new Vertex(1500, 450, 0);
            DrawnSceneObject Back = new DrawnSceneObject("Back", BackTile);
            DrawnSceneObject Return = new DrawnSceneObject("Return", ReturnTile);
            DrawnSceneObject Level = new DrawnSceneObject("Level", LevelDisplay);
            DrawnSceneObject LeftDSO = new DrawnSceneObject("Left", Left);
            DrawnSceneObject RightDSO = new DrawnSceneObject("Right", Right);
            LeftDSO.Events.Extern.MouseClick += new GameEventHandler(this.LeftClick);
            RightDSO.Events.Extern.MouseClick += new GameEventHandler(this.RightClick);
            Return.Events.Extern.MouseClick += new GameEventHandler(this.ReturnClick);
            Level.Events.Extern.MouseClick += new GameEventHandler(this.LevelClick);
            this.AddSceneObject(Back);
            this.AddSceneObject(Return);
            this.AddSceneObject(Level);
            this.AddSceneObject(LeftDSO);
            this.AddSceneObject(RightDSO);
            if (LevelDisplay.Index() == 0) Left.Active = false;
            else Left.Active = true;
            if (LevelDisplay.Index() == Max) Right.Active = false;
            else Right.Active = true;
        }
        public void SetCurrent(int Index)
        {
            Tile Left = (Tile)((DrawnSceneObject)this.Data["Left"]).Visual;
            Tile Right = (Tile)((DrawnSceneObject)this.Data["Right"]).Visual;
            Tile Level = (Tile)((DrawnSceneObject)this.Data["Level"]).Visual;
            Level.SetIndex(Index);
            if (Level.Index() == 0) Left.Active = false;
            else Left.Active = true;
            if (Level.Index() == Max) Right.Active = false;
            else Right.Active = true;
        }
        private void ReturnClick(object sender, EventArguments e)
        {
            ExternRunner Runner = (ExternRunner)this.Data["Runner"];
            Runner.SwitchScene("Menu", false);
        }
        private void LeftClick(object sender, EventArguments e)
        {
            Tile Left = (Tile)((DrawnSceneObject)this.Data["Left"]).Visual;
            Tile Right = (Tile)((DrawnSceneObject)this.Data["Right"]).Visual;
            Tile Level = (Tile)((DrawnSceneObject)this.Data["Level"]).Visual;
            Level.SetIndex(Level.Index() - 1);
            if (Level.Index() == 0) Left.Active = false;
            else Left.Active = true;
            if (Level.Index() == Max) Right.Active = false;
            else Right.Active = true;
            Right.Active = true;
        }
        private void RightClick(object sender, EventArguments e)
        {
            Tile Left = (Tile)((DrawnSceneObject)this.Data["Left"]).Visual;
            Tile Right = (Tile)((DrawnSceneObject)this.Data["Right"]).Visual;
            Tile Level = (Tile)((DrawnSceneObject)this.Data["Level"]).Visual;
            Level.SetIndex(Level.Index() + 1);
            if (Level.Index() == 0) Left.Active = false;
            else Left.Active = true;
            if (Level.Index() == Max) Right.Active = false;
            else Right.Active = true;
            Left.Active = true;
        }
        private void LevelClick(object sender, EventArguments e)
        {
            Tile Level = (Tile)((DrawnSceneObject)this.Data["Level"]).Visual;
            Game CurrentGame = (Game)this.Data["Game"];
            ExternRunner Runner = (ExternRunner)this.Data["Runner"];
            GameScene OldGame = null;
            if (CurrentGame.Data.ContainsKey("GameScene"))
            {
                OldGame = (GameScene)CurrentGame.Data["GameScene"];
                CurrentGame.Scenes.Remove(OldGame);
                Runner.ClearScene("GameScene");
                Runner.Collect();
            }
            GameScene NewGame = new GameScene();
            NewGame.Data["Game"] = CurrentGame;
            NewGame.Data["Runner"] = Runner;
            NewGame.Data["DesiredLevel"] = Level.Index();
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
    }
}
