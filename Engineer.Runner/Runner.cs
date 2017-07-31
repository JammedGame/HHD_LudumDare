//#define FixedPipeline
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Engineer.Mathematics;
using Engineer.Data;
using Engineer.Draw;
using Engineer.Draw.OpenGL;
using Engineer.Draw.OpenGL.FixedGL;
using Engineer.Draw.OpenGL.GLSL;
using Engineer.Engine;
using OpenTK.Input;
using System.ComponentModel;
using System.Runtime;

namespace Engineer.Runner
{
    public enum WindowState
    {
        Normal = 0,
        Minimized = 1,
        Maximized = 2,
        Fullscreen = 3
    }
    public class Runner : OpenTK.GameWindow
    {
        private int _Seed;
        private int _FrameUpdateRate;
        protected bool _EventsAttached;
        protected bool _GameInit;
        protected bool _EngineInit;
        protected bool _ContextInit;
        protected Timer _Time;
        protected Scene _NextScene;
        protected Scene _PrevScene;
        protected Scene _CurrentScene;
        protected Game _CurrentGame;
        protected DrawEngine _Engine;
        protected BackgroundWorker _Worker;
        public int FrameUpdateRate { get => _FrameUpdateRate; set => _FrameUpdateRate = value; }
        public Runner(int Width, int Height, GraphicsMode Mode, string Title) : base(Width, Height, Mode, Title)
        {
            this._Seed = 0;
            this._FrameUpdateRate = 6;
            this._GameInit = false;
            this._EngineInit = false;
            this._ContextInit = false;
            this._EventsAttached = false;
            this._Time = new Timer(8.33);
            this._Time.Elapsed += Event_TimerTick;
            this._Time.AutoReset = true;
        }
        public void SetWindowState(WindowState State)
        {
            this.WindowState = (OpenTK.WindowState)State;
        }
        private void EngineInit()
        {
            this._EngineInit = true;
            _Engine = new DrawEngine();
            GLSLShaderRenderer Render = new GLSLShaderRenderer();
            Render.RenderDestination = this;
            Render.TargetType = RenderTargetType.Runner;
            _Engine.CurrentRenderer = Render;
            GLSLShaderMaterialTranslator Translator = new GLSLShaderMaterialTranslator();
            _Engine.CurrentTranslator = Translator;
            _Engine.SetDefaults();
        }
        public void Init(Game CurrentGame)
        {
            if (!_EngineInit) EngineInit();
            this._CurrentGame = CurrentGame;
        }
        public void Init(Game CurrentGame, Scene CurrentScene)
        {
            if (!_EngineInit) EngineInit();
            this._Time.Stop();
            this._GameInit = true;
            this._CurrentGame = CurrentGame;
            this._CurrentScene = CurrentScene;
            if (!this._EventsAttached)
            {
                this.Closing += new EventHandler<System.ComponentModel.CancelEventArgs>(Event_Closing);
                this.KeyDown += new EventHandler<KeyboardKeyEventArgs>(Event_KeyPress);
                this.KeyDown += new EventHandler<KeyboardKeyEventArgs>(Event_KeyDown);
                this.KeyUp += new EventHandler<KeyboardKeyEventArgs>(Event_KeyUp);
                this.MouseDown += new EventHandler<MouseButtonEventArgs>(Event_MouseClick);
                this.MouseDown += new EventHandler<MouseButtonEventArgs>(Event_MouseDown);
                this.MouseUp += new EventHandler<MouseButtonEventArgs>(Event_MouseUp);
                this.MouseMove += new EventHandler<MouseMoveEventArgs>(Event_MouseMove);
                this.MouseWheel += new EventHandler<MouseWheelEventArgs>(Event_MouseWheel);
                this.Resize += new EventHandler<EventArgs>(Event_Resize);
                PrepareEvents();
                this._EventsAttached = true;
            }
            this._Time.Start();
            Event_Load();
        }
        private bool SwitchSceneBackground(Scene NextScene, bool Preload)
        {
            if (this._Worker != null) return false;
            if (!this._ContextInit || !Preload)
            {
                this.Init(this._CurrentGame, NextScene);
                this._ContextInit = true;
                return true;
            }
            this._Worker = new BackgroundWorker();
            this._Worker.WorkerReportsProgress = true;
            this._Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.SwitchSceneFinish);
            this._Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.Event_OperationFinished);
            this._Worker.ProgressChanged += new ProgressChangedEventHandler(this.Event_OperationProgress);
            this._NextScene = NextScene;
            if (NextScene.Type == SceneType.Scene2D) this._Engine.Preload2DScene((Scene2D)NextScene, this._Worker);
            return true;
        }
        public void SwitchScene(Scene NextScene, bool Preload = true)
        {
            if (NextScene == null) return;
            while (!this.SwitchSceneBackground(NextScene, Preload));
        }
        public void SwitchScene(string SceneName, bool Preload = true)
        {
            for(int i = 0; i < this._CurrentGame.Scenes.Count; i++)
            {
                if(this._CurrentGame.Scenes[i].Name == SceneName)
                {
                    this.SwitchScene(this._CurrentGame.Scenes[i], Preload);
                }
            }
        }
        private void SwitchSceneFinish(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Init(this._CurrentGame, this._NextScene);
            if (this._Worker != null)
            {
                this._Worker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(this.SwitchSceneFinish);
                this._Worker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(this.Event_OperationFinished);
                this._Worker.ProgressChanged -= new ProgressChangedEventHandler(this.Event_OperationProgress);
                this._Worker.Dispose();
                this._Worker = null;
            }
        }
        private bool ClearSceneBackground(Scene ClearedScene)
        {
            if (this._Worker != null) return false;
            this._Worker = new BackgroundWorker();
            this._Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ClearSceneFinish);
            this._Worker.WorkerReportsProgress = true;
            if (ClearedScene.Type == SceneType.Scene2D) this._Engine.Destroy2DScene((Scene2D)ClearedScene, this._Worker);
            return true;
        }
        public void ClearScene(Scene ClearedScene)
        {
            if (ClearedScene == null) return;
            while (!this.ClearSceneBackground(ClearedScene)) ;
        }
        public void ClearScene(string SceneName)
        {
            for (int i = 0; i < this._CurrentGame.Scenes.Count; i++)
            {
                if (this._CurrentGame.Scenes[i].Name == SceneName)
                {
                    this.ClearScene(this._CurrentGame.Scenes[i]);
                }
            }
        }  
        private void ClearSceneFinish(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this._Worker != null)
            {
                this._Worker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(this.ClearSceneFinish);
                this._Worker.Dispose();
                this._Worker = null;
            }
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            System.GC.Collect();
        }
        protected virtual void PrepareEvents()
        {

        }
        protected override void OnResize(EventArgs e)
        {
            this.Event_Resize(null, e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            if (!_GameInit || !_EngineInit) return;
            this.MakeCurrent();
            if (_CurrentScene.Type == SceneType.Scene2D)
            {
                _Engine.Draw2DScene((Scene2D)_CurrentScene, this.ClientRectangle.Width, this.ClientRectangle.Height);
            }
            else if (_CurrentScene.Type == SceneType.Scene3D)
            {
                _Engine.Draw3DScene((Scene3D)_CurrentScene, this.ClientRectangle.Width, this.ClientRectangle.Height);
            }
            Event_RenderFrame(this, e);
            SwapBuffers();
        }
        private void Event_Closing(object sender, EventArgs e)
        {
            EventArguments Arguments = new EventArguments();
            CallEvents("Closing", Arguments);
        }
        private void Event_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            EventArguments Arguments = new EventArguments();
            Arguments.KeyDown = (KeyType)e.Key;
            Arguments.Control = e.Control;
            Arguments.Alt = e.Alt;
            Arguments.Shift = e.Shift;
            CallEvents("KeyDown", Arguments);
        }
        private void Event_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            EventArguments Arguments = new EventArguments();
            Arguments.KeyDown = (KeyType)e.Key;
            Arguments.Control = e.Control;
            Arguments.Alt = e.Alt;
            Arguments.Shift = e.Shift;
            CallEvents("KeyUp", Arguments);
        }
        private void Event_KeyPress(object sender, KeyboardKeyEventArgs e)
        {
            EventArguments Arguments = new EventArguments();
            Arguments.KeyDown = (KeyType)e.Key;
            Arguments.Control = e.Control;
            Arguments.Alt = e.Alt;
            Arguments.Shift = e.Shift;
            CallEvents("KeyPress", Arguments);
        }
        private void Event_Load()
        {
            EventArguments Arguments = new EventArguments();
            CallEvents("Load", Arguments);
        }
        private void Event_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EventArguments Arguments = new EventArguments();
            Arguments.Location = new Vertex(e.X, e.Y, 0);
            Arguments.ButtonDown = (MouseClickType)e.Button;
            Arguments.Handled = false;
            if (_CurrentScene.Type == SceneType.Scene2D)
            {
                Scene2D Current2DScene = (Scene2D)_CurrentScene;
                Vertex STrans = Current2DScene.Transformation.Translation;
                STrans = new Vertex(STrans.X * Current2DScene.Transformation.Scale.X, STrans.Y * Current2DScene.Transformation.Scale.Y, 0);
                for (int i = _CurrentScene.Objects.Count - 1; i >= 0; i--)
                {
                    if (_CurrentScene.Objects[i].Type == SceneObjectType.DrawnSceneObject)
                    {
                        DrawnSceneObject Current = (DrawnSceneObject)_CurrentScene.Objects[i];
                        Vertex Trans = Current.Visual.Translation;
                        Trans = new Vertex(Trans.X * Current2DScene.Transformation.Scale.X, Trans.Y * Current2DScene.Transformation.Scale.Y, 0);
                        Vertex Scale = Current.Visual.Scale;
                        float X = e.X;
                        float Y = e.Y;
                        Scale = new Vertex(Scale.X * Current2DScene.Transformation.Scale.X, Scale.Y * Current2DScene.Transformation.Scale.Y, 1);
                        if(Current.Visual.Fixed)
                        if (STrans.X + Trans.X < X && X < STrans.X + Trans.X + Scale.X &&
                            STrans.Y + Trans.Y < Y && Y < STrans.Y + Trans.Y + Scale.Y)
                        {
                            Arguments.Target = Current;
                            CallObjectEvents(i, "MouseDown", Arguments);
                            Arguments.Handled = true;
                        }
                    }
                }
            }
            Arguments.Target = null;
            CallEvents("MouseDown", Arguments);
        }
        private void Event_MouseUp(object sender, MouseButtonEventArgs e)
        {
            EventArguments Arguments = new EventArguments();
            Arguments.Location = new Vertex(e.X, e.Y, 0);
            Arguments.ButtonDown = (MouseClickType)e.Button;
            Arguments.Handled = false;
            if (_CurrentScene.Type == SceneType.Scene2D)
            {
                Scene2D Current2DScene = (Scene2D)_CurrentScene;
                Vertex STrans = Current2DScene.Transformation.Translation;
                STrans = new Vertex(STrans.X * Current2DScene.Transformation.Scale.X, STrans.Y * Current2DScene.Transformation.Scale.Y, 0);
                for (int i = _CurrentScene.Objects.Count - 1; i >= 0; i--)
                {
                    if (_CurrentScene.Objects[i].Type == SceneObjectType.DrawnSceneObject)
                    {
                        DrawnSceneObject Current = (DrawnSceneObject)_CurrentScene.Objects[i];
                        Vertex Trans = Current.Visual.Translation;
                        Trans = new Vertex(Trans.X * Current2DScene.Transformation.Scale.X, Trans.Y * Current2DScene.Transformation.Scale.Y, 0);
                        Vertex Scale = Current.Visual.Scale;
                        float X = e.X;
                        float Y = e.Y;
                        Scale = new Vertex(Scale.X * Current2DScene.Transformation.Scale.X, Scale.Y * Current2DScene.Transformation.Scale.Y, 1);
                        if (STrans.X + Trans.X < X && X < STrans.X + Trans.X + Scale.X &&
                            STrans.Y + Trans.Y < Y && Y < STrans.Y + Trans.Y + Scale.Y)
                        {
                            Arguments.Target = Current;
                            CallObjectEvents(i, "MouseUp", Arguments);
                            Arguments.Handled = true;
                        }
                    }
                }
            }
            Arguments.Target = null;
            CallEvents("MouseUp", Arguments);
        }
        private void Event_MouseClick(object sender, MouseButtonEventArgs e)
        {
            EventArguments Arguments = new EventArguments();
            Arguments.Location = new Vertex(e.X, e.Y, 0);
            Arguments.ButtonDown = (MouseClickType)e.Button;
            Arguments.Handled = false;
            if (_CurrentScene.Type == SceneType.Scene2D)
            {
                Scene2D Current2DScene = (Scene2D)_CurrentScene;
                Vertex STrans = Current2DScene.Transformation.Translation;
                STrans = new Vertex(STrans.X * Current2DScene.Transformation.Scale.X, STrans.Y * Current2DScene.Transformation.Scale.Y, 0);
                for (int i = _CurrentScene.Objects.Count - 1; i >= 0; i--)
                {
                    if (_CurrentScene.Objects[i].Type == SceneObjectType.DrawnSceneObject)
                    {
                        DrawnSceneObject Current = (DrawnSceneObject)_CurrentScene.Objects[i];
                        Vertex Trans = Current.Visual.Translation;
                        Trans = new Vertex(Trans.X * Current2DScene.Transformation.Scale.X, Trans.Y * Current2DScene.Transformation.Scale.Y, 0);
                        Vertex Scale = Current.Visual.Scale;
                        float X = e.X;
                        float Y = e.Y;
                        Scale = new Vertex(Scale.X * Current2DScene.Transformation.Scale.X, Scale.Y * Current2DScene.Transformation.Scale.Y, 1);
                        if (STrans.X + Trans.X < X && X < STrans.X + Trans.X + Scale.X &&
                            STrans.Y + Trans.Y < Y && Y < STrans.Y + Trans.Y + Scale.Y)
                        {
                            Arguments.Target = Current;
                            CallObjectEvents(i, "MouseClick", Arguments);
                            Arguments.Handled = true;
                        }
                    }
                }
            }
            Arguments.Target = null;
            CallEvents("MouseClick", Arguments);
        }
        private void Event_MouseMove(object sender, MouseMoveEventArgs e)
        {
            EventArguments Arguments = new EventArguments();
            Arguments.Location = new Vertex(e.X, e.Y, 0);
            Arguments.Handled = false;
            if (_CurrentScene.Type == SceneType.Scene2D)
            {
                Scene2D Current2DScene = (Scene2D)_CurrentScene;
                Vertex STrans = Current2DScene.Transformation.Translation;
                STrans = new Vertex(STrans.X * Current2DScene.Transformation.Scale.X, STrans.Y * Current2DScene.Transformation.Scale.Y, 0);
                for (int i = _CurrentScene.Objects.Count - 1; i >= 0; i--)
                {
                    if (_CurrentScene.Objects[i].Type == SceneObjectType.DrawnSceneObject)
                    {
                        DrawnSceneObject Current = (DrawnSceneObject)_CurrentScene.Objects[i];
                        Vertex Trans = Current.Visual.Translation;
                        Trans = new Vertex(Trans.X * Current2DScene.Transformation.Scale.X, Trans.Y * Current2DScene.Transformation.Scale.Y, 0);
                        Vertex Scale = Current.Visual.Scale;
                        float X = e.X;
                        float Y = e.Y;
                        Scale = new Vertex(Scale.X * Current2DScene.Transformation.Scale.X, Scale.Y * Current2DScene.Transformation.Scale.Y, 1);
                        if (STrans.X + Trans.X < X && X < STrans.X + Trans.X + Scale.X &&
                            STrans.Y + Trans.Y < Y && Y < STrans.Y + Trans.Y + Scale.Y)
                        {
                            Arguments.Target = Current;
                            CallObjectEvents(i, "MouseMove", Arguments);
                            Arguments.Handled = true;
                        }
                    }
                }
            }
            Arguments.Target = null;
            CallEvents("MouseMove", Arguments);
        }
        private void Event_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            EventArguments Arguments = new EventArguments();
            Arguments.Location = new Vertex(e.X, e.Y, 0);
            Arguments.Delta = e.Delta;
            CallEvents("MouseWheel", Arguments);
        }
        private void Event_RenderFrame(object sender, EventArgs e)
        {
            EventArguments Arguments = new EventArguments();
            CallEvents("RenderFrame", Arguments);
        }
        private void Event_Resize(object sender, EventArgs e)
        {
            if (this._CurrentScene == null) return;
            EventArguments Arguments = new EventArguments();
            Arguments.Size = new Vertex(this.Width, this.Height, 0);
            CallEvents("Resize", Arguments);
        }
        private void Event_TimerTick(object sender, ElapsedEventArgs e)
        {
            this._Seed++;
            if (_CurrentScene.Type == SceneType.Scene2D)
            {
                Scene2D C2DS = (Scene2D)_CurrentScene;
                for (int i = 0; i < C2DS.Sprites.Count; i++)
                {
                    if (C2DS.Sprites[i].SpriteSets.Count == 0) continue;
                    int FrameUpdateRate = this._FrameUpdateRate;
                    if (C2DS.Sprites[i].SpriteSets[C2DS.Sprites[i].CurrentSpriteSet].Seed != -1) FrameUpdateRate = C2DS.Sprites[i].SpriteSets[C2DS.Sprites[i].CurrentSpriteSet].Seed;
                    if (this._Seed % FrameUpdateRate == 0) C2DS.Sprites[i].RaiseIndex();
                }
            }
            EventArguments Arguments = new EventArguments();
            CallEvents("TimerTick", Arguments);
        }
        private void Event_OperationProgress(object sender, ProgressChangedEventArgs e)
        {
            if (this._CurrentScene == null) return;
            EventArguments Arguments = new EventArguments();
            Arguments.Progress = e.ProgressPercentage;
            CallEvents("OperationProgress", Arguments);
        }
        private void Event_OperationFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this._CurrentScene == null) return;
            EventArguments Arguments = new EventArguments();
            CallEvents("OperationFinished", Arguments);
        }
        protected virtual void CallEvents(string EventName, EventArguments Args)
        {
        }
        protected virtual void CallObjectEvents(int Index, string EventName, EventArguments Args)
        {
        }
    }
}
