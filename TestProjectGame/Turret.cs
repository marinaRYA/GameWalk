using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWalk
{
    public class Turret : IAttackable
    {
        public int X { get; }
        public int Y { get; }
        public int Damage { get; set; }
        public int Health { get; set; }
        public int Range { get; set; }
        public int ProjectileSpeed { get; set; }

        public List<TurretProjectile> projectiles = new List<TurretProjectile>();

        public bool IsAlive = true;

        public Turret(int x, int y, int damage, int range, int health, int speed, int count)
        {
            X = x;
            Y = y;
            Damage = damage;
            Health = health;
            Range = range;
            ProjectileSpeed = speed;
            set_proj(count);
        }
        private void set_proj(int count)
        {
            for (int i = 0; i < count; i++)
            {
                double deltaX = ProjectileSpeed * Math.Cos(360 / (i+1));
                double deltaY = ProjectileSpeed * Math.Sin(360 / (i + 1)); 
                TurretProjectile pr = new TurretProjectile(X, Y, deltaX, deltaY, Damage);
                projectiles.Add(pr);
            }
        }
        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                IsAlive = false;
            }
        }
        public void Update()
        {
            if (IsAlive)
            {
                foreach (TurretProjectile pr in projectiles) pr.set_XY(X, Y);
            }
        }
        public void Attack(player p)
        {
            if (IsAlive)
            {
                foreach (TurretProjectile pr in projectiles) pr.Move(p);
            }
        }
    }
}
