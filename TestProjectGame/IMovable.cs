using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWalk
{
    public interface IMovable
    {
        int X { get; set; }
        int Y { get; set; }
        void Move(player p);
    }
}
