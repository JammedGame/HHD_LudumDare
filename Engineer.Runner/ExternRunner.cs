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

namespace Engineer.Runner
{
    public class ExternRunner : Runner
    {
        public ExternRunner(int Width, int Height, GraphicsMode Mode, string Title) : base(Width, Height, Mode, Title)
        {
        }
        public ExternRunner(int Width, int Height, string Title) : base(Width, Height, new GraphicsMode(32, 24, 0, 8), Title)
        {
        }
        protected override void CallEvents(string EventName, EventArguments Args)
        {
            _CurrentScene.Events.Extern.Invoke(EventName, _CurrentGame, Args);
        }
        protected override void CallObjectEvents(int Index, string EventName, EventArguments Args)
        {
            _CurrentScene.Objects[Index].Events.Extern.Invoke(EventName, _CurrentGame, Args);
        }
    }
}
