using Engineer.Engine;
using Engineer.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Project
{
    class Movement
    {
        private bool _WDown=false;
        private bool _ADown = false;
        private bool _SDown = false;
        private bool _DDown = false;

        private bool _Num8 = false;
        private bool _Num4 = false;
        private bool _Num5 = false;
        private bool _Num6 = false;

        private Player Player1, Player2;
        private Scene2D CScene;

        public Movement(Player P1, Player P2, Scene2D CScene)
        {
            this.Player1 = P1;
            this.Player2 = P2;

            this.CScene = CScene;
            this.CScene.Events.Extern.TimerTick += new GameEventHandler(GameUpdate);
        }       
        public void KeyDownEvent(Game G, EventArguments E)
        {
            if (E.KeyDown == KeyType.W)
            {
                _WDown = true;
            }
            if (E.KeyDown == KeyType.A)
            {
                _ADown = true;
            }
            if (E.KeyDown == KeyType.S)
            {
                _SDown = true;
            }
            if (E.KeyDown == KeyType.D)
            {
                _DDown = true;
            }
            if (E.KeyDown == KeyType.Keypad8)
            {
                _Num8 = true;
            }
            if (E.KeyDown == KeyType.Keypad4)
            {
                _Num4 = true;
            }
            if (E.KeyDown == KeyType.Keypad5)
            {
                _Num5 = true;
            }
            if (E.KeyDown == KeyType.Keypad6)
            {
                _Num6 = true;
            }
        }
        public void KeyUpEvent(Game G, EventArguments E)
        {
            if (E.KeyDown == KeyType.W)
            {
                _WDown = false;
            }
            if (E.KeyDown == KeyType.A)
            {
                _ADown = false;
            }
            if (E.KeyDown == KeyType.S)
            {
                _SDown = false;
            }
            if (E.KeyDown == KeyType.D)
            {
                _DDown = false;
            }
            if (E.KeyDown == KeyType.Keypad8)
            {
                _Num8 = false;
            }
            if (E.KeyDown == KeyType.Keypad4)
            {
                _Num4 = false;
            }
            if (E.KeyDown == KeyType.Keypad5)
            {
                _Num5 = false;
            }
            if (E.KeyDown == KeyType.Keypad6)
            {
                _Num6 = false;
            }
        }
        public void GameUpdate(Game G, EventArguments E)
        {
            if (_WDown)
            {
                this.Player1.Visual.Translation = new Vertex(Player1.Visual.Translation.X, Player1.Visual.Translation.Y - 5, 0);
            }
            if (_ADown)
            {
                this.Player1.Visual.Translation = new Vertex(Player1.Visual.Translation.X-5, Player1.Visual.Translation.Y, 0);
            }
            if (_SDown)
            {
                this.Player1.Visual.Translation = new Vertex(Player1.Visual.Translation.X, Player1.Visual.Translation.Y + 5, 0);
            }
            if (_DDown)
            {
                this.Player1.Visual.Translation = new Vertex(Player1.Visual.Translation.X+5, Player1.Visual.Translation.Y, 0);
            }
            if (_Num8)
            {
                this.Player2.Visual.Translation = new Vertex(Player2.Visual.Translation.X, Player2.Visual.Translation.Y - 5, 0);
            }
            if (_Num4)
            {
                this.Player2.Visual.Translation = new Vertex(Player2.Visual.Translation.X-5, Player2.Visual.Translation.Y , 0);
            }
            if (_Num5)
            {
                this.Player2.Visual.Translation = new Vertex(Player2.Visual.Translation.X, Player2.Visual.Translation.Y + 5, 0);
            }
            if (_Num6)
            {
                this.Player2.Visual.Translation = new Vertex(Player2.Visual.Translation.X+5, Player2.Visual.Translation.Y, 0);
            }
        }
    }
}
