using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace GameWalk
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
       

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            player p = new player();
            Level level = new Level();
            level.SetLevel(1);
            GameWindow gameWindow = new GameWindow(p,level);
            gameWindow.Show();
            this.Close();

        }

        private void ContinueGame_Click(object sender, RoutedEventArgs e)
        {
            // Level loadedLevel = LoadLevel("levelData.xml");
            //  player loadedPlayer = LoadPlayer("playerData.xml");
            Level loadedLevel = LoadLevel("levelData.xml");
            player loadedPlayer = LoadPlayer("playerData.xml");
            GameWindow gameWindow = new GameWindow(loadedPlayer, loadedLevel);
            gameWindow.Show();
            this.Close();

            
          

        }
        private Level LoadLevel(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Level));
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return (Level)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке уровня: {ex.Message}");
                return null;
            }
        }

        private player LoadPlayer(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(player));
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return (player)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных игрока: {ex.Message}");
                return null;
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            
            Application.Current.Shutdown();
        }
    }
}

