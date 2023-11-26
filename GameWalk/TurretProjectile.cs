using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameWalk
{
    public class TurretProjectile : IMovable
    {
        public int X { get; set; }
        public int Y { get; set; }
        double deltaX { get; set; }
        double deltaY { get; set; }

        int Damage { get; } // Урон снаряда турели
        public bool IsAlive = true;

        public TurretProjectile() { }
        public TurretProjectile(int x, int y, double dX, double dY, int damage)
        {
            X = x;
            Y = y;
            deltaX = dX;
            deltaY = dY;
            Damage = damage;
        }
        public void set_XY(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public void Attack(player p)
        {
            p.TakeDamage(Damage);
        }
        public void Move(player p)
        {

            X += (int)deltaX;
            Y += (int)deltaY;
            double distance = Math.Sqrt(Math.Pow(p.playerX - X, 2) + Math.Pow(p.playerY - Y, 2));
            if (distance < 7)
            {
                Attack(p);
                IsAlive = false;
            }

        }
    }
}
