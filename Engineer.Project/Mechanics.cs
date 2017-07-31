using Engineer.Engine;
using Engineer.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Project
{
    class Mechanics
    {
        private Scene2D CScene;
        public Mechanics(Scene2D Scene)
        {
            this.CScene = Scene;
        }

        public double Distance(DrawnSceneObject Obj1, DrawnSceneObject Obj2)
        {
            return (Math.Sqrt(Math.Pow(Math.Abs(Obj1.Visual.Translation.X - Obj2.Visual.Translation.X), 2) + Math.Pow(Math.Abs(Obj1.Visual.Translation.Y - Obj2.Visual.Translation.Y), 2)));
        }
        public void CheckLever(Player Player1, Player Player2)
        {
            for (int i = 0; i < Level.leverID; i++)
            {
                DrawnSceneObject Lever = (DrawnSceneObject)CScene.Data["Lever" + i];
                DrawnSceneObject Door = (DrawnSceneObject)CScene.Data["Door" + i];
                float P1X = Player1.Visual.Translation.X + Player1.Visual.Scale.X / 2;
                float P1Y = Player1.Visual.Translation.Y + Player1.Visual.Scale.Y / 2;
                float P2X = Player2.Visual.Translation.X + Player2.Visual.Scale.X / 2;
                float P2Y = Player2.Visual.Translation.Y + Player2.Visual.Scale.Y / 2;
                float LX = Lever.Visual.Translation.X + Lever.Visual.Scale.X / 2;
                float LY = Lever.Visual.Translation.Y + Lever.Visual.Scale.Y / 2;

                if (Collision2D.Check(Player1.Visual.Translation, Player1.Visual.Scale, Lever.Visual.Translation, Lever.Visual.Scale, Collision2DType.Rectangular))
                {
                    Door.Data.Remove("Collision");
                    Door.Active = false;
                    ((Sprite)Lever.Visual).UpdateSpriteSet("LeverDown");
                }
                if (Collision2D.Check(Player2.Visual.Translation, Player2.Visual.Scale, Lever.Visual.Translation, Lever.Visual.Scale, Collision2DType.Rectangular))
                {
                    Door.Data.Remove("Collision");
                    Door.Active = false;
                    ((Sprite)Lever.Visual).UpdateSpriteSet("LeverDown");
                }
            }
        }
        public void CheckFan()
        {
            List<SceneObject> Fans = CScene.GetObjectsWithData("Fan");
            List<SceneObject> Boxes = CScene.GetObjectsWithData("Box");
            for (int i = 0; i < Fans.Count; i++)
            {
                Sprite Fan = (Sprite)Fans[i].Visual;
                int Direction = (int)Fans[i].Data["Direction"];
                int Range = (int)Fans[i].Data["MaxRange"];
                for (int j = 0; j < Boxes.Count; j++)
                {
                    int NewRange = Range;
                    Sprite Box = (Sprite)Boxes[j].Visual;
                    if(Direction == 0)
                    {
                        if(Math.Abs(Fan.Translation.X - Box.Translation.X) < 50 && Box.Translation.Y < Fan.Translation.Y)
                        {
                            NewRange = (int)(Fan.Translation.Y - (Box.Translation.Y + Box.Scale.Y));
                        }
                    }
                    if (Direction == 1)
                    {
                        if (Math.Abs(Fan.Translation.Y - Box.Translation.Y) < 50 && Box.Translation.X > Fan.Translation.X)
                        {
                            NewRange = (int)(Box.Translation.X - (Fan.Translation.X + Fan.Scale.X));
                        }
                    }
                    if (Direction == 2)
                    {
                        if (Math.Abs(Fan.Translation.X - Box.Translation.X) < 50 && Box.Translation.Y > Fan.Translation.Y)
                        {
                            NewRange = (int)(Box.Translation.Y - (Fan.Translation.Y + Fan.Scale.Y));
                        }
                    }
                    if (Direction == 3)
                    {
                        if (Math.Abs(Fan.Translation.Y - Box.Translation.Y) < 50 && Box.Translation.X < Fan.Translation.X)
                        {
                            NewRange = (int)(Fan.Translation.X - (Box.Translation.X + Box.Scale.X));
                        }
                    }
                    if (NewRange < Range) Range = NewRange;
                }
                if(Range != (int)Fans[i].Data["Range"])
                {
                    FanGlow Glow = (FanGlow)CScene.Data[Fans[i].ID+"Glow"];
                    Fans[i].Data["Range"] = Range;
                    Glow.Update();
                }
            }
        }
        public void CheckHeaters()
        {
            List<SceneObject> Heaters = CScene.GetObjectsWithData("Heater");
            for (int i = 0; i < Heaters.Count; i++)
            {
                ((Glow)CScene.Data[Heaters[i].ID + "Glow"]).Update();
            }
        }
    }
}
