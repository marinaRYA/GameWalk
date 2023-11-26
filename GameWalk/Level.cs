using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace GameWalk
{
    public class Level
    {
         
        public int levelnumber = 1;
        public List<Enemy> Enemies { get; private set; }
        public List<Turret> Turrets { get; private set; }

        public Bonus bonus { get; set; }
        public Level()
        {
            
            Enemies = new List<Enemy>();
            Turrets = new List<Turret>();
            bonus = new Bonus();
        }
        public void SetLevel(int levelNumber)
        {
            this.levelnumber = levelNumber;
            Enemies = new List<Enemy>();
            Turrets = new List<Turret>();

            if (levelNumber == 1)
            {
                Enemies.Add(new Enemy(100, 100, 3, 40, 10));
                Enemies.Add(new Enemy(250, 250, 3, 40, 10));
                bonus = new Bonus(300, 300, 30, false);
            }
            else if (levelNumber == 2)
            {
                Enemies.Clear();
                Enemies.Add(new Enemy(100, 100, 3, 50, 10));
                Enemies.Add(new Enemy(250, 10, 3, 70, 20));
                Turrets.Add(new Turret(220, 220, 50, 30, 40, 2, 3));
                bonus = new Bonus(300, 300, 30, true);
            }
            else if (levelNumber == 3)
            {
                Enemies.Clear();
                Turrets.Clear();
                Turrets.Add(new Turret(10, 10, 50, 30, 50, 2, 6));
                Turrets.Add(new Turret(350, 350, 50, 30, 50, 2, 6));
                Enemies.Add(new Enemy(300, 300, 3, 100, 30));
                bonus = new Bonus(300, 300, 30, true);
            }


        }
    
       public void Save(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Level));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, this);
            }
        }

        public Level LoadLevel(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Level));
            using (StreamReader reader = new StreamReader(filePath))
            {
                return (Level)serializer.Deserialize(reader);
            }

        }
    }
}
