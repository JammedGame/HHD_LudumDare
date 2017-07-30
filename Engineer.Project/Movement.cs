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
        private int MoveSpeed;
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

        private bool P1CollTop = false;
        private bool P1CollRight = false;
        private bool P1CollLeft = false;
        private bool P1CollBottom = false;

        private bool P2CollTop = false;
        private bool P2CollRight = false;
        private bool P2CollLeft = false;
        private bool P2CollBottom = false;

        private bool P1LeftP2 = false;
        private bool P1RightP2 = false;
        private bool P1TopP2 = false;
        private bool P1BottomP2 = false;

        private float RightEdge;
        private float LeftEdge;
        private float BottomEdge;
        private float TopEdge;


        private List<SceneObject> LSO = new List<SceneObject>();

        public Movement(Player P1, Player P2, Scene2D CScene)
        {
            this.MoveSpeed = 5;
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
            if (_WDown && !P1CollTop)
            {
                this.Player1.Visual.Translation = new Vertex(Player1.Visual.Translation.X, Player1.Visual.Translation.Y - MoveSpeed, 0);
            }
            if (_ADown && !P1CollLeft)
            {
                this.Player1.Visual.Translation = new Vertex(Player1.Visual.Translation.X - MoveSpeed, Player1.Visual.Translation.Y, 0);
            }
            if (_SDown && !P1CollBottom)
            {
                this.Player1.Visual.Translation = new Vertex(Player1.Visual.Translation.X, Player1.Visual.Translation.Y + MoveSpeed, 0);
            }
            if (_DDown && !P1CollRight)
            {
                this.Player1.Visual.Translation = new Vertex(Player1.Visual.Translation.X + MoveSpeed, Player1.Visual.Translation.Y, 0);
            }
            if (_Num8 && !P2CollTop)
            {
                this.Player2.Visual.Translation = new Vertex(Player2.Visual.Translation.X, Player2.Visual.Translation.Y - MoveSpeed, 0);
            }
            if (_Num4 && !P2CollLeft)
            {
                this.Player2.Visual.Translation = new Vertex(Player2.Visual.Translation.X - MoveSpeed, Player2.Visual.Translation.Y, 0);
            }
            if (_Num5 && !P2CollBottom)
            {
                this.Player2.Visual.Translation = new Vertex(Player2.Visual.Translation.X, Player2.Visual.Translation.Y + MoveSpeed, 0);
            }
            if (_Num6 && !P2CollRight)
            {
                this.Player2.Visual.Translation = new Vertex(Player2.Visual.Translation.X + MoveSpeed, Player2.Visual.Translation.Y, 0);
            }
            Player1Coll();
            Player2Coll();
        }
        public bool ChkPlayersCollision()
        {
            return Collision2D.Check(Player1.Visual.Translation, Player1.Visual.Scale, Player2.Visual.Translation, Player2.Visual.Scale, Collision2DType.Radius);
        }
        public void PlayersCollision()
        {
            this.PCollision = ChkPlayersCollision();
            if (PCollision)
            {
                float P1CentarX = Player1.Visual.Translation.X + Player1.Visual.Scale.X / 2;
                float P1CentarY = Player1.Visual.Translation.Y + Player1.Visual.Scale.Y / 2;
                float P2CentarX = Player2.Visual.Translation.X + Player2.Visual.Scale.X / 2;
                float P2CentarY = Player2.Visual.Translation.Y + Player2.Visual.Scale.Y / 2;

                if ((P1CentarX + Player1.Visual.Scale.X / 2)-(P2CentarX - Player2.Visual.Scale.X / 2) < 0)
                {
                    P1LeftP2 = true;
                }
                else
                {
                    P1LeftP2 = false;
                }
                if ((P2CentarX + Player2.Visual.Scale.X / 2) - (P1CentarX - Player1.Visual.Scale.X / 2) < 0)
                {
                    P1RightP2 = true;
                }
                else
                {
                    P1RightP2 = false;
                }
                if ((P1CentarY + Player1.Visual.Scale.Y / 2) - (P2CentarY - Player2.Visual.Scale.Y / 2) < 0)
                {
                    P1TopP2 = true;
                }
                else
                {
                    P1TopP2 = false;
                }
                if ((P2CentarY + Player2.Visual.Scale.Y / 2) - (P1CentarY - Player1.Visual.Scale.Y / 2) < 0)
                {
                    P1BottomP2 = true;
                }
                else
                {
                    P1BottomP2 = false;
                }
            }
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
        public List<DrawnSceneObject> P2FindWalls()
        {
            List<DrawnSceneObject> tempList = new List<DrawnSceneObject>();
            for (int i = 0; i < LSO.Count; i++)
            {
                this.P2WCollision = WallCollisionP2(Player2, (DrawnSceneObject)LSO[i]);
                if (P2WCollision)
                {
                    tempList.Add((DrawnSceneObject)LSO[i]);
                }
            }
            return tempList;
        }
        public void Player1Coll()
        {
            List<DrawnSceneObject> colList = new List<DrawnSceneObject>();
            colList = P1FindWalls();
            if (colList.Count > 0)
            {
                for (int i = 0; i < colList.Count; i++)
                {
                    LeftEdge = Player1.Visual.Translation.X;
                    RightEdge = Player1.Visual.Translation.X+Player1.Visual.Scale.X;                    
                    TopEdge = Player1.Visual.Translation.Y;
                    BottomEdge = Player1.Visual.Translation.Y + Player1.Visual.Scale.Y;
                    
                    if (RightEdge == colList[i].Visual.Translation.X && ((BottomEdge<= colList[i].Visual.Translation.Y+colList[i].Visual.Scale.Y && TopEdge>=colList[i].Visual.Translation.Y) || (BottomEdge >= colList[i].Visual.Translation.Y && TopEdge <= colList[i].Visual.Translation.Y) || (BottomEdge > colList[i].Visual.Translation.Y + colList[i].Visual.Scale.Y && TopEdge < colList[i].Visual.Translation.Y+colList[i].Visual.Scale.Y)))
                    {
                        P1CollRight = true;
                    }
                    else
                    {
                        P1CollRight = false;
                    }
                    if (LeftEdge==colList[i].Visual.Translation.X +colList[i].Visual.Scale.X && ((TopEdge >= colList[i].Visual.Translation.Y && BottomEdge <= colList[i].Visual.Translation.Y + colList[i].Visual.Scale.Y )||(TopEdge<=colList[i].Visual.Translation.Y && BottomEdge >= colList[i].Visual.Translation.Y) ||(TopEdge < colList[i].Visual.Translation.Y + colList[i].Visual.Scale.Y && BottomEdge > colList[i].Visual.Translation.Y+colList[i].Visual.Scale.Y)))
                    {
                        P1CollLeft = true;
                    }
                    else
                    {
                        P1CollLeft = false;
                    }
                    if (TopEdge == colList[i].Visual.Translation.Y + colList[i].Visual.Scale.Y && ((LeftEdge>=colList[i].Visual.Translation.X && RightEdge<=colList[i].Visual.Translation.X + colList[i].Visual.Scale.X)||(LeftEdge<=colList[i].Visual.Translation.X && RightEdge>=colList[i].Visual.Translation.X)||(LeftEdge<colList[i].Visual.Translation.X +colList[i].Visual.Scale.X && RightEdge>colList[i].Visual.Translation.X+colList[i].Visual.Scale.X)))
                    {
                        P1CollTop = true;
                    }
                    else
                    {
                        P1CollTop = false;
                    }
                    if (BottomEdge == colList[i].Visual.Translation.Y && ((LeftEdge >= colList[i].Visual.Translation.X && RightEdge <= colList[i].Visual.Translation.X + colList[i].Visual.Scale.X)||(LeftEdge<=colList[i].Visual.Translation.X && RightEdge>=colList[i].Visual.Translation.X)||(LeftEdge<colList[i].Visual.Translation.X+colList[i].Visual.Scale.X && RightEdge>colList[i].Visual.Translation.X+colList[i].Visual.Scale.X)))
                    {
                        P1CollBottom = true;
                    }
                    else
                    {
                        P1CollBottom = false;
                    }
                }
            }
            else
            {
                P1CollBottom = false;
                P1CollLeft = false;
                P1CollRight = false;
                P1CollTop = false;
            }
        }
        public void Player2Coll()
        {
            List<DrawnSceneObject> colList = new List<DrawnSceneObject>();
            colList = P2FindWalls();
            if (colList.Count > 0)
            {
                for (int i = 0; i < colList.Count; i++)
                {
                    if ((Player2.Visual.Translation.X + Player2.Visual.Scale.X) - colList[i].Visual.Translation.X >= 0 || (Player2.Visual.Translation.X + Player2.Visual.Scale.X) - colList[i].Visual.Translation.X <= Player2.Visual.Scale.X / 10)
                    {
                        P2CollRight = true;
                    }
                    else
                    {
                        P2CollRight = false;
                    }
                    if (Player2.Visual.Translation.X - (colList[i].Visual.Translation.X + colList[i].Visual.Scale.X) <= 0 || Player2.Visual.Translation.X - (colList[i].Visual.Translation.X + colList[i].Visual.Scale.X) >= -Player2.Visual.Scale.X / 10)
                    {
                        P2CollLeft = true;
                    }
                    else
                    {
                        P2CollLeft = false;
                    }
                    if ((Player2.Visual.Translation.Y + Player2.Visual.Scale.Y) - colList[i].Visual.Translation.Y >= 0 || ((Player2.Visual.Translation.Y + Player2.Visual.Scale.Y) - colList[i].Visual.Translation.Y) <= Player2.Visual.Scale.Y / 10)
                    {
                        P2CollBottom = true;
                    }
                    else
                    {
                        P2CollBottom = false;
                    }
                    if ((Player2.Visual.Translation.Y + Player2.Visual.Scale.Y) - colList[i].Visual.Translation.Y <= 0 || (Player2.Visual.Translation.Y + Player2.Visual.Scale.Y) - colList[i].Visual.Translation.Y >= -Player2.Visual.Scale.Y / 10)
                    {
                        P2CollTop = true;
                    }
                    else
                    {
                        P2CollTop = false;
                    }
                }
            }
        }

    }
}