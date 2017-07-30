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
        Vertical,
        RadiusToRectangular
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
            else if (Type == Collision2DType.RadiusToRectangular) return Collision2D.CheckRadiusToRectangularCollision(Position, Scale, ColliderPosition, ColliderScale);
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
        private static bool CheckRadiusToRectangularCollision(Vertex Position, Vertex Scale, Vertex ColliderPosition, Vertex ColliderScale)
        {
            bool InCollision = false;
            VertexBuilder Center = new VertexBuilder(Position.X + Scale.X / 2, Position.Y + Scale.Y / 2, 0);
            VertexBuilder ColliderPoint = new VertexBuilder(ColliderPosition.X, ColliderPosition.Y, 0);
            double Distance = Math.Abs((Center - ColliderPoint).Length());
            InCollision = InCollision || Distance < Scale.Y/2;
            ColliderPoint = new VertexBuilder(ColliderPosition.X + ColliderScale.X, ColliderPosition.Y, 0);
            Distance = Math.Abs((Center - ColliderPoint).Length());
            InCollision = InCollision || Distance < Scale.Y / 2;
            ColliderPoint = new VertexBuilder(ColliderPosition.X + ColliderScale.X, ColliderPosition.Y + ColliderScale.Y, 0);
            Distance = Math.Abs((Center - ColliderPoint).Length());
            InCollision = InCollision || Distance < Scale.Y / 2;
            ColliderPoint = new VertexBuilder(ColliderPosition.X, ColliderPosition.Y + ColliderScale.Y, 0);
            Distance = Math.Abs((Center - ColliderPoint).Length());
            InCollision = InCollision || Distance < Scale.Y / 2;
            InCollision = InCollision || (Math.Abs(ColliderPosition.X - Center.X) < Scale.Y / 2 && Center.X < ColliderPosition.X);
            InCollision = InCollision || (Math.Abs(ColliderPosition.X + ColliderScale.X - Center.X) < Scale.Y / 2 && Center.X > ColliderPosition.X + ColliderScale.X);
            InCollision = InCollision || (Math.Abs(ColliderPosition.Y - Center.X) < Scale.Y / 2 && Center.Y < ColliderPosition.Y);
            InCollision = InCollision || (Math.Abs(ColliderPosition.Y + ColliderScale.Y - Center.X) < Scale.Y / 2 && Center.Y > ColliderPosition.Y + ColliderScale.Y);
            return InCollision;
        }
        public static CollisionModel RadiusRectangularModel(Vertex Position, Vertex Scale, Vertex ColliderPosition, Vertex ColliderScale)
        {
            CollisionModel Model = new CollisionModel();
            if (!Collision2D.CheckRectangularCollision(Position, Scale, ColliderPosition, ColliderScale)) return Model;
            VertexBuilder Center = new VertexBuilder(Position.X + Scale.X / 2, Position.Y + Scale.Y / 2, 0);
            VertexBuilder ColliderPoint = new VertexBuilder(ColliderPosition.X, ColliderPosition.Y, 0);
            double Distance = Math.Abs((Center - ColliderPoint).Length());
            if(Distance < Scale.Y / 2)
            {
                if (Center.X > ColliderPosition.X) Model.Bottom = true;
                else if (Center.Y > ColliderPosition.Y) Model.Right = true;
                else
                {
                    Model.Right = true;
                    Model.Bottom = true;
                }
            }
            ColliderPoint = new VertexBuilder(ColliderPosition.X + ColliderScale.X, ColliderPosition.Y, 0);
            Distance = Math.Abs((Center - ColliderPoint).Length());
            if (Distance < Scale.Y / 2)
            {
                if (Center.X < ColliderPosition.X + ColliderScale.X) Model.Bottom = true;
                else if (Center.Y > ColliderPosition.Y) Model.Left = true;
                else
                {
                    Model.Left = true;
                    Model.Bottom = true;
                }
            }
            ColliderPoint = new VertexBuilder(ColliderPosition.X + ColliderScale.X, ColliderPosition.Y + ColliderScale.Y, 0);
            Distance = Math.Abs((Center - ColliderPoint).Length());
            if (Distance < Scale.Y / 2)
            {
                if (Center.X < ColliderPosition.X + ColliderScale.X) Model.Top = true;
                else if (Center.Y < ColliderPosition.Y + ColliderScale.Y) Model.Left = true;
                else
                {
                    Model.Left = true;
                    Model.Top = true;
                }
            }
            ColliderPoint = new VertexBuilder(ColliderPosition.X, ColliderPosition.Y + ColliderScale.Y, 0);
            Distance = Math.Abs((Center - ColliderPoint).Length());
            if (Distance < Scale.Y / 2)
            {
                if (Center.X > ColliderPosition.X) Model.Top = true;
                else if (Center.Y < ColliderPosition.Y + ColliderScale.Y) Model.Right = true;
                else
                {
                    Model.Right = true;
                    Model.Top = true;
                }
            }
            if (Math.Abs(ColliderPosition.X - Center.X) < Scale.Y / 2 && Center.X < ColliderPosition.X)
            {
                Model.Right = true;
            }
            if (Math.Abs((ColliderPosition.X + ColliderScale.X) - Center.X) < Scale.Y / 2 && Center.X > ColliderPosition.X + ColliderScale.X)
            {
                Model.Left = true;
            }
            if (Math.Abs(ColliderPosition.Y - Center.Y) < Scale.Y / 2 && Center.Y < ColliderPosition.Y)
            {
                Model.Bottom = true;
            }
            if (Math.Abs((ColliderPosition.Y + ColliderScale.Y) - Center.Y) < Scale.Y / 2 && Center.Y > ColliderPosition.Y + ColliderScale.Y)
            {
                Model.Top = true;
            }
            return Model;
        }
        public static CollisionModel RadiusModel(Vertex Position, Vertex Scale, Vertex ColliderPosition, Vertex ColliderScale)
        {
            CollisionModel Model = new CollisionModel();
            if (!Collision2D.CheckRadiusCollision(Position, Scale, ColliderPosition, ColliderScale)) return Model;
            Vertex Center = new Vertex(Position.X + Scale.X / 2, Position.Y + Scale.Y / 2, 0);
            Vertex ColliderCenter = new Vertex(ColliderPosition.X + ColliderScale.X / 2, ColliderPosition.Y + ColliderScale.Y / 2, 0);
            double Angle = VertexTransformer.Angle(Center, ColliderCenter);
            if(Angle < 22.5f)
            {
                Model.Left = true;
            }
            else if (Angle < 67.5f)
            {
                Model.Top = true;
                Model.Left = true;
            }
            else if (Angle < 112.5f)
            {
                Model.Top = true;
            }
            else if (Angle < 157.5f)
            {
                Model.Right = true;
                Model.Top = true;
            }
            else if (Angle < 202.5f)
            {
                Model.Right = true;
            }
            else if (Angle < 247.5f)
            {
                Model.Right = true;
                Model.Bottom = true;
            }
            else if (Angle < 292.5f)
            {
                Model.Bottom = true;
            }
            else if (Angle < 292.5f)
            {
                Model.Bottom = true;
                Model.Left = true;
            }
            else
            {
                Model.Left = true;
            }
            return Model;
        }
    }
    public class CollisionModel
    {
        public bool Left;
        public bool Right;
        public bool Top;
        public bool Bottom;
    }
}
