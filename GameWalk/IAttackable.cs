using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWalk
{
    public interface IAttackable
    {       
        void Attack(player p);
        void TakeDamage(int damage);
    }
}
