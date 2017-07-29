using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Mathematics
{
    public enum Collision2DType
    {
        Radius,
        Rectangular,
        Focus,
        Vertical
    }
    public class Collision2D
    {
        public static int Offset = 10;
        public static bool Check(Vertex Position, Vertex Scale, Vertex ColliderPosition, Vertex ColliderScale, Collision2DType Type)
        {
            if (Type == Collision2DType.Radius) return Collision2D.CheckRadiusCollision(Position, Scale, ColliderPosition, ColliderScale);
            else if (Type == Collision2DType.Rectangular) return Collision2D.CheckRectangularCollision(Position, Scale, ColliderPosition, ColliderScale);
            else if (Type == Collision2DType.Focus) return Collision2D.CheckFocusCollision(Position, Scale, ColliderPosition, ColliderScale);
            else if (Type == Collision2DType.Vertical) return Collision2D.CheckVerticalCollision(Position, Scale, ColliderPosition, ColliderScale);
            return false;
        }
        private static bool CheckRadiusCollision(Vertex Position, Vertex Scale, Vertex ColliderPosition, Vertex ColliderScale)
        {
            VertexBuilder Center = new VertexBuilder(Position.X + Scale.X / 2, Position.Y + Scale.Y / 2, 0);
            VertexBuilder ColliderCenter = new VertexBuilder(ColliderPosition.X + ColliderScale.X / 2, ColliderPosition.Y + ColliderScale.Y / 2, 0);
            double Distance = Math.Abs((Center - ColliderCenter).Length());
            double HalfSize = 0;
            if (Scale.X > Scale.Y) HalfSize = Scale.X / 2;
            else HalfSize = Scale.Y / 2;
            double ColliderHalfSize = 0;
            if (ColliderScale.X > ColliderScale.Y) ColliderHalfSize = ColliderScale.X / 2;
            else ColliderHalfSize = ColliderScale.Y / 2;
            return Distance < HalfSize + ColliderHalfSize;
        }
        private static bool CheckRectangularCollision(Vertex Position, Vertex Scale, Vertex ColliderPosition, Vertex ColliderScale)
        {
            bool XCollision = Position.X <= ColliderPosition.X && Position.X + Scale.X >= ColliderPosition.X;
            XCollision = XCollision || ColliderPosition.X <= Position.X && ColliderPosition.X + ColliderScale.X >= Position.X;
            bool YCollision = Position.Y <= ColliderPosition.Y && Position.Y + Scale.Y >= ColliderPosition.Y;
            YCollision = YCollision || ColliderPosition.Y <= Position.Y && ColliderPosition.Y + ColliderScale.Y >= Position.Y;
            return XCollision && YCollision;
        }
        private static bool CheckFocusCollision(Vertex Position, Vertex Scale, Vertex ColliderPosition, Vertex ColliderScale)
        {
            if (Position.Y > ColliderPosition.Y + Offset) return false;
            return CheckPointRectangularCollision(new Vertex(Position.X + Scale.X / 2, Position.Y + Scale.Y, 0), ColliderPosition, ColliderScale);
        }
        private static bool CheckVerticalCollision(Vertex Position, Vertex Scale, Vertex ColliderPosition, Vertex ColliderScale)
        {
            return CheckPointRectangularCollision(new Vertex(Position.X + Scale.X / 2, Position.Y + Scale.Y, 0), ColliderPosition, ColliderScale) ||
                CheckPointRectangularCollision(new Vertex(Position.X + Scale.X / 2, Position.Y, 0), ColliderPosition, ColliderScale);
        }
        private static bool CheckPointRectangularCollision(Vertex Point, Vertex ColliderPosition, Vertex ColliderScale)
        {
            return Point.X >= ColliderPosition.X && Point.X <= ColliderPosition.X + ColliderScale.X && Point.Y >= ColliderPosition.Y && Point.Y <= ColliderPosition.Y + ColliderScale.Y;
        }
    }
}
