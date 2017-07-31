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
            List<SceneObject> Boxes = CScene.GetObjectsWithData("Box");
            List<SceneObject> Levers = CScene.GetObjectsWithData("Lever");
            List<SceneObject> Plates = CScene.GetObjectsWithData("Plate");
            for (int i = 0; i < Levers.Count; i++)
            {
                DrawnSceneObject Lever = (DrawnSceneObject)Levers[i];

                DrawnSceneObject Target = (DrawnSceneObject)CScene.Data[(string)Lever.Data["Target"]];

                if (Collision2D.Check(Player1.Visual.Translation, Player1.Visual.Scale, Lever.Visual.Translation, Lever.Visual.Scale, Collision2DType.Rectangular))
                {
                    if(Target.Data.ContainsKey("Door"))
                    {
                        Target.Data.Remove("Collision");
                        Target.Active = false;
                        ((Sprite)Lever.Visual).UpdateSpriteSet("LeverDown");
                    }
                    else if (Target.Data.ContainsKey("Fan"))
                    {
                        Target.Data["Enabled"] = false;
                        ((Sprite)Lever.Visual).UpdateSpriteSet("LeverDown");
                    }
                }
                if (Collision2D.Check(Player2.Visual.Translation, Player2.Visual.Scale, Lever.Visual.Translation, Lever.Visual.Scale, Collision2DType.Rectangular))
                {
                    if (Target.Data.ContainsKey("Door"))
                    {
                        Target.Data.Remove("Collision");
                        Target.Active = false;
                        ((Sprite)Lever.Visual).UpdateSpriteSet("LeverDown");
                    }
                    else if (Target.Data.ContainsKey("Fan"))
                    {
                        Target.Data["Enabled"] = false;
                        ((Sprite)Lever.Visual).UpdateSpriteSet("LeverDown");
                    }
                }
            }
            for (int i = 0; i < Plates.Count; i++)
            {
                DrawnSceneObject Plate = (DrawnSceneObject)Plates[i];
                DrawnSceneObject Target = (DrawnSceneObject)CScene.Data[(string)Plate.Data["Target"]];

                bool Pressed = false;
                if (Collision2D.Check(Player1.Visual.Translation, Player1.Visual.Scale, Plate.Visual.Translation, Plate.Visual.Scale, Collision2DType.Rectangular)) Pressed = true;
                if (Collision2D.Check(Player2.Visual.Translation, Player2.Visual.Scale, Plate.Visual.Translation, Plate.Visual.Scale, Collision2DType.Rectangular)) Pressed = true;
                for(int j = 0; j < Boxes.Count; j++)
                {
                    if (Collision2D.Check(Boxes[j].Visual.Translation, Boxes[j].Visual.Scale, Plate.Visual.Translation, Plate.Visual.Scale, Collision2DType.Rectangular)) Pressed = true;
                }
                if(Pressed)
                {
                    if (Target.Data.ContainsKey("Door"))
                    {
                        Target.Data.Remove("Collision");
                        Target.Active = false;
                        ((Sprite)Plate.Visual).UpdateSpriteSet("LeverDown");
                    }
                    else if (Target.Data.ContainsKey("Fan"))
                    {
                        Target.Data["Enabled"] = false;
                        ((Sprite)Plate.Visual).UpdateSpriteSet("LeverDown");
                    }
                }
                else
                {
                    if (Target.Data.ContainsKey("Door"))
                    {
                        Target.Data["Collision"] = true;
                        Target.Active = true;
                        ((Sprite)Plate.Visual).UpdateSpriteSet("LeverUp");
                    }
                    else if (Target.Data.ContainsKey("Fan"))
                    {
                        Target.Data["Enabled"] = true;
                        ((Sprite)Plate.Visual).UpdateSpriteSet("LeverDown");
                    }
                }
            }
        }
        public void CheckFan()
        {
            List<SceneObject> Fans = CScene.GetObjectsWithData("Fan");
            List<SceneObject> Boxes = CScene.GetObjectsWithData("Box");
            for (int i = 0; i < Fans.Count; i++)
            {
                FanGlow Glow = (FanGlow)CScene.Data[Fans[i].ID + "Glow"];
                if(!(bool)Fans[i].Data["Enabled"])
                {
                    Glow.Active = false;
                    continue;
                }
                else Glow.Active = true;
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
        public void CheckExit(Player Player1, Player Player2)
        {
            if (!CScene.Data.ContainsKey("ExitDoor")) return;
            DrawnSceneObject Exit = (DrawnSceneObject)CScene.Data["ExitDoor"];
            if(Collision2D.Check(Player1.Visual.Translation, Player1.Visual.Scale, Exit.Visual.Translation, Exit.Visual.Scale, Collision2DType.Rectangular) &&
                Collision2D.Check(Player2.Visual.Translation, Player2.Visual.Scale, Exit.Visual.Translation, Exit.Visual.Scale, Collision2DType.Radius))
            {
                ExternRunner Runner = (ExternRunner)CScene.Data["Runner"];
                Runner.SwitchScene("LevelPicker", false);
            }
        }
    }
}
