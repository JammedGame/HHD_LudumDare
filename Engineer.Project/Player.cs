using Engineer.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Project
{
    class Player:DrawnSceneObject
    {
        private int _Heat;                    

        public int Heat
        {
            get { return _Heat; }
            set { _Heat = value; }
        }
    }
}
