using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWalk
{
    public class Bonus
    {
        public int X { get; set; } 
        public int Y { get; set; } 
        public int HealthValue { get; set; }

        public bool IsExist { get; set; }

        public Bonus(int x, int y, int healthValue, bool ex)
        {
            X = x;
            Y = y;
            HealthValue = healthValue;
            IsExist = ex;

        }

    }
}
