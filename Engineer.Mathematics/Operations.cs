using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Mathematics
{
    public class Operations
    {
        public static int LongestByDoubles(List<double[]> Items)
        {
            int Max = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Max < Items[i].Length) Max = Items[i].Length;
            }
            return Max;
        }
        public static int LongestBySingles(List<float[]> Items)
        {
            int Max = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Max < Items[i].Length) Max = Items[i].Length;
            }
            return Max;
        }
        public static bool IsNaN(float d)
        {
            if (d < 0d || d >= 0d) return false;
            return true;
        }
        public static double GetDistance(Vertex V1, Vertex V2)
        {
            return Math.Sqrt((V1.X - V2.X) * (V1.X - V2.X) + (V1.Y - V2.Y) * (V1.Y - V2.Y) + (V1.Z - V2.Z) * (V1.Z - V2.Z));
        }
    }
}
