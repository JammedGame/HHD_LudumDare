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

        private CollisionModel P1Wall = new CollisionModel();
        private CollisionModel P2Wall = new CollisionModel();
        private CollisionModel P1Other = new CollisionModel();
        private CollisionModel P2Other = new CollisionModel();

        private List<SceneObject> LSO = new List<SceneObject>();
        private List<SceneObject> Boxes = new List<SceneObject>();

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
            Boxes = CScene.GetObjectsWithData("Box");
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
            if (_WDown && !P1Wall.Top && !P1Other.Top)
            {
                this.Player1.Visual.Translation = new Vertex(Player1.Visual.Translation.X, Player1.Visual.Translation.Y - MoveSpeed, 0);
                for(int i = 0; i < Boxes.Count; i++)
                {
                    if(((CollisionModel)Boxes[i].Data["P1Coll"]).Top) Boxes[i].Visual.Translation = new Vertex(Boxes[i].Visual.Translation.X, Boxes[i].Visual.Translation.Y - MoveSpeed, 0);
                }
            }
            if (_ADown && !P1Wall.Left && !P1Other.Left)
            {
                this.Player1.Visual.Translation = new Vertex(Player1.Visual.Translation.X - MoveSpeed, Player1.Visual.Translation.Y, 0);
                for (int i = 0; i < Boxes.Count; i++)
                {
                    if (((CollisionModel)Boxes[i].Data["P1Coll"]).Left) Boxes[i].Visual.Translation = new Vertex(Boxes[i].Visual.Translation.X - MoveSpeed, Boxes[i].Visual.Translation.Y, 0);
                }
            }
            if (_SDown && !P1Wall.Bottom && !P1Other.Bottom)
            {
                this.Player1.Visual.Translation = new Vertex(Player1.Visual.Translation.X, Player1.Visual.Translation.Y + MoveSpeed, 0);
                for (int i = 0; i < Boxes.Count; i++)
                {
                    if (((CollisionModel)Boxes[i].Data["P1Coll"]).Bottom) Boxes[i].Visual.Translation = new Vertex(Boxes[i].Visual.Translation.X, Boxes[i].Visual.Translation.Y + MoveSpeed, 0);
                }
            }
            if (_DDown && !P1Wall.Right && !P1Other.Right)
            {
                this.Player1.Visual.Translation = new Vertex(Player1.Visual.Translation.X + MoveSpeed, Player1.Visual.Translation.Y, 0);
                for (int i = 0; i < Boxes.Count; i++)
                {
                    if (((CollisionModel)Boxes[i].Data["P1Coll"]).Right) Boxes[i].Visual.Translation = new Vertex(Boxes[i].Visual.Translation.X + MoveSpeed, Boxes[i].Visual.Translation.Y, 0);
                }
            }
            if (_Num8 && !P2Wall.Top && !P2Other.Top)
            {
                this.Player2.Visual.Translation = new Vertex(Player2.Visual.Translation.X, Player2.Visual.Translation.Y - MoveSpeed, 0);
                for (int i = 0; i < Boxes.Count; i++)
                {
                    if (((CollisionModel)Boxes[i].Data["P2Coll"]).Top) Boxes[i].Visual.Translation = new Vertex(Boxes[i].Visual.Translation.X, Boxes[i].Visual.Translation.Y - MoveSpeed, 0);
                }
            }
            if (_Num4 && !P2Wall.Left && !P2Other.Left)
            {
                this.Player2.Visual.Translation = new Vertex(Player2.Visual.Translation.X - MoveSpeed, Player2.Visual.Translation.Y, 0);
                for (int i = 0; i < Boxes.Count; i++)
                {
                    if (((CollisionModel)Boxes[i].Data["P2Coll"]).Left) Boxes[i].Visual.Translation = new Vertex(Boxes[i].Visual.Translation.X - MoveSpeed, Boxes[i].Visual.Translation.Y, 0);
                }
            }
            if (_Num5 && !P2Wall.Bottom && !P2Other.Bottom)
            {
                this.Player2.Visual.Translation = new Vertex(Player2.Visual.Translation.X, Player2.Visual.Translation.Y + MoveSpeed, 0);
                for (int i = 0; i < Boxes.Count; i++)
                {
                    if (((CollisionModel)Boxes[i].Data["P2Coll"]).Bottom) Boxes[i].Visual.Translation = new Vertex(Boxes[i].Visual.Translation.X, Boxes[i].Visual.Translation.Y + MoveSpeed, 0);
                }
            }
            if (_Num6 && !P2Wall.Right && !P2Other.Right)
            {
                this.Player2.Visual.Translation = new Vertex(Player2.Visual.Translation.X + MoveSpeed, Player2.Visual.Translation.Y, 0);
                for (int i = 0; i < Boxes.Count; i++)
                {
                    if (((CollisionModel)Boxes[i].Data["P2Coll"]).Right) Boxes[i].Visual.Translation = new Vertex(Boxes[i].Visual.Translation.X + MoveSpeed, Boxes[i].Visual.Translation.Y, 0);
                }
            }
            PlayersCollision();
        }
        private void PlayersCollision()
        {
            this.P1Wall = new CollisionModel();
            this.P2Wall = new CollisionModel();
            this.P1Other = Collision2D.RadiusRectangularModel(Player1.Visual.Translation, Player1.Visual.Scale, Player2.Visual.Translation, Player2.Visual.Scale);
            this.P2Other = Collision2D.RadiusRectangularModel(Player2.Visual.Translation, Player2.Visual.Scale, Player1.Visual.Translation, Player1.Visual.Scale);
            for (int i = 0; i < LSO.Count; i++)
            {
                CollisionModel New = Collision2D.RadiusRectangularModel(Player1.Visual.Translation, Player1.Visual.Scale, LSO[i].Visual.Translation, LSO[i].Visual.Scale);
                P1Wall = CombineModels(P1Wall, New);
                New = Collision2D.RadiusRectangularModel(Player2.Visual.Translation, Player2.Visual.Scale, LSO[i].Visual.Translation, LSO[i].Visual.Scale);
                P2Wall = CombineModels(P2Wall, New);
            }
            for (int i = 0; i < Boxes.Count; i++)
            {
                Boxes[i].Data["P1Coll"] = new CollisionModel();
                CollisionModel New = Collision2D.RadiusRectangularModel(Player1.Visual.Translation, Player1.Visual.Scale, Boxes[i].Visual.Translation, Boxes[i].Visual.Scale);
                Boxes[i].Data["P1Coll"] = CombineModels((CollisionModel)Boxes[i].Data["P1Coll"], New);
                Boxes[i].Data["P2Coll"] = new CollisionModel();
                New = Collision2D.RadiusRectangularModel(Player2.Visual.Translation, Player2.Visual.Scale, Boxes[i].Visual.Translation, Boxes[i].Visual.Scale);
                Boxes[i].Data["P2Coll"] = CombineModels((CollisionModel)Boxes[i].Data["P2Coll"], New);
            }
        }
        private CollisionModel CombineModels(CollisionModel Old, CollisionModel New)
        {
            CollisionModel Model = new CollisionModel();
            Model.Left = Old.Left || New.Left;
            Model.Right = Old.Right || New.Right;
            Model.Top = Old.Top || New.Top;
            Model.Bottom = Old.Bottom || New.Bottom;
            return Model;
        }
    }
}