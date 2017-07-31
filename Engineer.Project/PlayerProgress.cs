using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Project
{
    public class PlayerProgress
    {
        public static int Read()
        {
            string Value = File.ReadAllText("Data/levels.dat");
            return Convert.ToInt32(Value);
        }
        public static void Write(int Value)
        {
            File.WriteAllText("Data/levels.dat", Value.ToString());
        }
    }
}
