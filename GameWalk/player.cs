using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace GameWalk
{
    public class player 
    {
        public int playerX { get; set; }
        public int playerY { get; set; }
        public int health;
        public int maxhealth = 200;
        public int damage { get; set; }
        public delegate void PlayerDiedEventHandler();
        public event PlayerDiedEventHandler PlayerDied;

        public player()
        {

            playerX = 200;
            playerY = 200;
            damage = 20;
            health = maxhealth;
            damage = 300;
        }
        public void Attack(Enemy e)
        {
            if (e.IsAlive)
            {
                double distance = Math.Sqrt(Math.Pow(playerX - e.X, 2) + Math.Pow(playerY - e.Y, 2));
                if (distance < 20) e.TakeDamage(damage);
            }
        }
        public void Attack(Turret t)
        {
            if (t.IsAlive)
            {
                double distance = Math.Sqrt(Math.Pow(playerX - t.X, 2) + Math.Pow(playerY - t.Y, 2));
                if (distance < 20) t.TakeDamage(damage);
            }
        }
        protected virtual void OnPlayerDied()
        {
            PlayerDied?.Invoke();
        }
        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health < 0) OnPlayerDied();
        }
        public void PickUpBonus(Bonus bonus)
        {
            double distance = Math.Sqrt(Math.Pow(playerX - bonus.X, 2) + Math.Pow(playerY - bonus.Y, 2));

            if (distance < 30 && bonus.IsExist)
            {
                health += bonus.HealthValue;
                bonus.IsExist = false;
            }
        }
        public void Save(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(player));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, this);
            }
        }

        public player Load(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(player));
            using (StreamReader reader = new StreamReader(filePath))
            {
                return (player)serializer.Deserialize(reader);
            }
        }
    }
}

