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
            DrawnSceneObject Lever = (DrawnSceneObject)CScene.Data["Lever"];
            DrawnSceneObject Door = (DrawnSceneObject)CScene.Data["Door"];
            float P1X = Player1.Visual.Translation.X + Player1.Visual.Scale.X / 2;
            float P1Y = Player1.Visual.Translation.Y + Player1.Visual.Scale.Y / 2;
            float P2X = Player2.Visual.Translation.X + Player2.Visual.Scale.X / 2;
            float P2Y = Player2.Visual.Translation.Y + Player2.Visual.Scale.Y / 2;
            float LX = Lever.Visual.Translation.X + Lever.Visual.Scale.X / 2;
            float LY = Lever.Visual.Translation.Y + Lever.Visual.Scale.Y / 2;

            if (Collision2D.Check(Player1.Visual.Translation,Player1.Visual.Scale,Lever.Visual.Translation,Lever.Visual.Scale,Collision2DType.Rectangular))
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
}
