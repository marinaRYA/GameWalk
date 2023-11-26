using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWalk
{
    public class Enemy : IMovable, IAttackable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public bool IsAlive = true;
        public Enemy(int x, int y, int speed, int health, int damage)
        {
            X = x;
            Y = y;
            Speed = speed;
            Health = health;
            Damage = damage;
        }


        public void Move(player p)
        {

            double distance = Math.Sqrt(Math.Pow(p.playerX - X, 2) + Math.Pow(p.playerY - Y, 2));
            if (distance < 7) Attack(p);
            else
            {

                double vx = (p.playerX - X) / distance * Speed;
                double vy = (p.playerY - Y) / distance * Speed;
                X += (int)vx;
                Y += (int)vy;
            }

        }


        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0) IsAlive = false;

        }
        public void Attack(player p)
        {
            if (IsAlive) p.TakeDamage(p.damage);

        }
    }
}
