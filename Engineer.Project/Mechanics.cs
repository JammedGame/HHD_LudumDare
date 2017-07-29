using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Project
{
    class Mechanics
    {                
        public int Distance(Player Player1, Player Player2)
        {
            return Convert.ToInt32(Math.Sqrt(Math.Pow(Convert.ToInt32(Player1.Visual.Translation.X-Player2.Visual.Translation.X),2.0f)+ Math.Pow(Convert.ToInt32(Player1.Visual.Translation.Y - Player2.Visual.Translation.Y), 2.0f)));
        }

    }
}
