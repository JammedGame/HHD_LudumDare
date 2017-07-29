using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Engine
{
    public class Background : DrawObject
    {
        public Background() : base()
        {
            this.Type = DrawObjectType.Background;
        }
        public Background(Background B) : base(B)
        {
        }
    }
}
