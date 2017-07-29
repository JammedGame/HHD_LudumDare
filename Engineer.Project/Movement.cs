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
        private bool _WDown = false;
        private bool _ADown = false;
        private bool _SDown = false;
        private bool _DDown = false;

        private bool _Num8 = false;
        private bool _Num4 = false;
        private bool _Num5 = false;
        private bool _Num6 = false;

        private Player Player1, Player2;
        private Scene2D CScene;

        private bool PCollision = false;
        private bool P1WCollision = false;
        private bool P2WCollision = false;

        private bool CollTop = false;
        private bool CollRight = false;
        private bool CollLeft = false;
        private bool CollBottom = false;

        private List<SceneObject> LSO = new List<SceneObject>();


        public Movement(Player P1, Player P2, Scene2D CScene)
        {
            this.Player1 = P1;
            this.Player2 = P2;

            this.CScene = CScene;
            this.CScene.Events.Extern.TimerTick += new GameEventHandler(GameUpdate);
            this.CScene.Events.Extern.KeyDown += new GameEventHandler(KeyDownEvent);
            this.CScene.Events.Extern.KeyUp += new GameEventHandler(KeyUpEvent);

            LSO = CScene.GetObjectsWithData("Collision");
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
            List<DrawnSceneObject> colList = new List<DrawnSceneObject>();
            colList = P1FindWalls();
            if (colList.Count > 0)
            {
                for (int i = 0; i < colList.Count; i++)
                {
                    if (Player1.Visual.Translation.X + Player1.Visual.Scale.X == colList[i].Visual.Translation.X)
                    {
                        CollRight = true;
                    }
                    else
                    {
                        CollRight = false;
                    }
                    if (Player1.Visual.Translation.X == colList[i].Visual.Translation.X + colList[i].Visual.Scale.X)
                    {
                        CollLeft = true;
                    }
                    else
                    {
                        CollLeft = false;
                    }
                    if (Player1.Visual.Translation.Y + Player1.Visual.Scale.Y == colList[i].Visual.Translation.Y)
                    {
                        CollBottom = true;
                    }
                    else
                    {
                        CollBottom = false;
                    }
                    if (Player1.Visual.Translation.Y + Player1.Visual.Scale.Y == colList[i].Visual.Translation.Y)
                    {
                        CollTop = true;
                    }
                    else
                    {
                        CollTop = false;
                    }
                }
            }

            if (_WDown && !CollTop)
            {
                this.Player1.Visual.Translation = new Vertex(Player1.Visual.Translation.X, Player1.Visual.Translation.Y - 15, 0);
            }
            if (_ADown && !CollLeft)
            {
                this.Player1.Visual.Translation = new Vertex(Player1.Visual.Translation.X - 15, Player1.Visual.Translation.Y, 0);
            }
            if (_SDown && !CollBottom)
            {
                this.Player1.Visual.Translation = new Vertex(Player1.Visual.Translation.X, Player1.Visual.Translation.Y + 15, 0);
            }
            if (_DDown && !CollRight)
            {
                this.Player1.Visual.Translation = new Vertex(Player1.Visual.Translation.X + 15, Player1.Visual.Translation.Y, 0);
            }
            if (_Num8)
            {
                this.Player2.Visual.Translation = new Vertex(Player2.Visual.Translation.X, Player2.Visual.Translation.Y - 15, 0);
            }
            if (_Num4)
            {
                this.Player2.Visual.Translation = new Vertex(Player2.Visual.Translation.X - 15, Player2.Visual.Translation.Y, 0);
            }
            if (_Num5)
            {
                this.Player2.Visual.Translation = new Vertex(Player2.Visual.Translation.X, Player2.Visual.Translation.Y + 15, 0);
            }
            if (_Num6)
            {
                this.Player2.Visual.Translation = new Vertex(Player2.Visual.Translation.X + 15, Player2.Visual.Translation.Y, 0);
            }
        }
        public bool PlayerCollision(Player Player1, Player Player2)
        {
            return Collision2D.Check(Player1.Visual.Translation, Player1.Visual.Scale, Player2.Visual.Translation, Player2.Visual.Scale, Collision2DType.Radius);
        }
        public bool WallCollisionP1(Player Player, DrawnSceneObject DSO)
        {
            return Collision2D.Check(Player.Visual.Translation, Player.Visual.Scale, DSO.Visual.Translation, DSO.Visual.Scale, Collision2DType.Rectangular);
        }
        public bool WallCollisionP2(Player Player, DrawnSceneObject DSO)
        {
            return Collision2D.Check(Player.Visual.Translation, Player.Visual.Scale, DSO.Visual.Translation, DSO.Visual.Scale, Collision2DType.Rectangular);
        }
        public List<DrawnSceneObject> P1FindWalls()
        {
            List<DrawnSceneObject> tempList = new List<DrawnSceneObject>();
            for (int i = 0; i < LSO.Count; i++)
            {
                this.P1WCollision = WallCollisionP1(Player1, (DrawnSceneObject)LSO[i]);
                if (P1WCollision)
                {
                    tempList.Add((DrawnSceneObject)LSO[i]);
                }
            }
            return tempList;
        }

    }
}