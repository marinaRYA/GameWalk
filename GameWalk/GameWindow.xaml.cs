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
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace GameWalk
{
    /// <summary>
    /// Логика взаимодействия для GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        Level level;
        player p;
        private Dictionary<Enemy, Image> enemyImages = new Dictionary<Enemy, Image>();
        private Dictionary<Turret, Image> turretImages = new Dictionary<Turret, Image>();
        List<Ellipse> turretProjectiles = new List<Ellipse>();     
        private DispatcherTimer gameTimer;
        private DispatcherTimer shootTimer;
        private DispatcherTimer levelTimer;
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            gameTimer.Stop();
            shootTimer.Stop();
            levelTimer.Stop();
            if (p.health > 0 )
             {
                MessageBoxResult result = MessageBox.Show("Хотите сохранить игру перед закрытием?", "Сохранение игры", MessageBoxButton.YesNoCancel);

                if (result == MessageBoxResult.Yes)
                {
                    level.Save("levelData.xml");
                    p.Save("playerData.xml");
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return; 
                }
            }
            
        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (level.bonus.IsExist)
            {              
                Canvas.SetLeft(bImage, level.bonus.X);
                Canvas.SetTop(bImage, level.bonus.Y);
            }
            else Canvas.SetLeft(bImage, 1000);
            
            foreach (Enemy enemy in level.Enemies) 
                {
                   
                    if (enemy.IsAlive)
                    {
                        enemy.Move(p);
                    if (!enemyImages.ContainsKey(enemy))
                    {
                        // Если изображение врага не существует в словаре, создаем новое изображение
                        Image enemyImage = new Image();
                        enemyImage.Source = new BitmapImage(new Uri("pack://application:,,,/bat.png"));
                        enemyImage.Width = 25;
                        enemyImage.Height = 20;
                        gameCanvas.Children.Add(enemyImage); // Добавляем изображение врага на Canvas
                        enemyImages.Add(enemy, enemyImage); // Добавляем изображение в словарь
                    }

                   
                    Image image = enemyImages[enemy];
                    Canvas.SetLeft(image, enemy.X);
                    Canvas.SetTop(image, enemy.Y);
                    
                        }
                else
                {
                    
                    if (enemyImages.ContainsKey(enemy))
                    {
                        gameCanvas.Children.Remove(enemyImages[enemy]); // Удаляем изображение из Canvas
                        enemyImages.Remove(enemy); // Удаляем изображение из словаря
                    }
                }

            }
            foreach (Turret turret in level.Turrets)
            {
                 if (turret.IsAlive)
                {
                    if (turretProjectiles.Count != 0) turret.Attack(p);
                    if (!turretImages.ContainsKey(turret))
                    {                       
                        Image turretImage = new Image();
                        turretImage.Source = new BitmapImage(new Uri("pack://application:,,,/spider.png"));
                        turretImage.Width = 40;
                        turretImage.Height = 40;
                        Panel.SetZIndex(turretImage, 1);
                        gameCanvas.Children.Add(turretImage); 
                        turretImages.Add(turret, turretImage); 
                    }

                    int n = turret.projectiles.Count;
                    foreach (Ellipse tur in turretProjectiles)
                    {
                        n--;
                        Canvas.SetLeft(tur, turret.projectiles[n].X+5);
                        Canvas.SetTop(tur, turret.projectiles[n].Y+5);
                        if (n == 0) break;
                    }
                    Image image = turretImages[turret];
                    Canvas.SetLeft(image, turret.X);
                    Canvas.SetTop(image, turret.Y);
                   
                }
                else
                {
                   
                    if (turretImages.ContainsKey(turret))
                    {
                        gameCanvas.Children.Remove(turretImages[turret]); 
                        turretImages.Remove(turret); 
                    }
                }
            }

        }
        private void ShootTimer_Tick(object sender, EventArgs e)
        {
            int c = 0;
            foreach (Turret turret in level.Turrets) c+= turret.projectiles.Count();
            foreach (Turret turret in level.Turrets)
            {
               
                if (turret.IsAlive)
                {
                    turret.Update();

                   
                        foreach (TurretProjectile tr in turret.projectiles)
                        {
                            Ellipse turretProjectile = new Ellipse();
                            turretProjectile.Width = 10;
                            turretProjectile.Height = 10;
                            turretProjectile.Fill = Brushes.Red;
                            Canvas.SetLeft(turretProjectile, tr.X);
                            Canvas.SetTop(turretProjectile, tr.Y);
                            gameCanvas.Children.Add(turretProjectile);
                            turretProjectiles.Add(turretProjectile);
                        }
                    
                }
            }

        }
        private void LevelTimer_Tick(object sender, EventArgs e)
        {
            int l = level.levelnumber;
            
            if ((level.Enemies.All(enemy => !enemy.IsAlive) && level.Turrets.All(turret => !turret.IsAlive)))
            {
                foreach (Enemy enemy in level.Enemies)
                {
                    if (enemyImages.ContainsKey(enemy))
                    {
                        gameCanvas.Children.Remove(enemyImages[enemy]); // Удаляем изображение из Canvas
                        enemyImages.Remove(enemy); // Удаляем изображение из словаря
                    }
                }
               
                foreach (Ellipse el in turretProjectiles) gameCanvas.Children.Remove(el);
                turretProjectiles.Clear();
                
                level.SetLevel(++l);
                
            }
            if (l>3) MessageBox.Show("Игра пройдена!", "Поздравляем", MessageBoxButton.OK, MessageBoxImage.Information);
         }
        private void SetTimers()
        {
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(50);
            shootTimer = new DispatcherTimer();
            shootTimer.Interval = TimeSpan.FromMilliseconds(7500);
            levelTimer = new DispatcherTimer();
            levelTimer.Interval = TimeSpan.FromMilliseconds(5000);
            gameTimer.Tick += GameTimer_Tick;
            shootTimer.Tick += ShootTimer_Tick;
            levelTimer.Tick += LevelTimer_Tick;
            gameTimer.Start();
            shootTimer.Start();
            levelTimer.Start();
        }
        private void GameWindow_Loaded(object sender, RoutedEventArgs e) { SetTimers();  }
        
        private void StartNewGame()
        {
           
;           level = new Level();
            p = new player();          
            GameWindow gameWindow = new GameWindow(p,level); 
            gameWindow.Show();
            this.Close();
        }  

       
        private void ShowGameOverMessage()
        {

            gameTimer.Stop();
            shootTimer.Stop();
            levelTimer.Stop();

            var result = MessageBox.Show("Вы мертвы! Начать игру заново?", "Game Over", MessageBoxButton.YesNo, MessageBoxImage.Question);

            
            if (result == MessageBoxResult.Yes) StartNewGame();
            else Application.Current.Shutdown();
        }
        public GameWindow(player player, Level l)
        {
            InitializeComponent();
            level = l;
            p = player;
            this.KeyDown += GameWindow_KeyDown;
            Loaded += GameWindow_Loaded;
            
            p.PlayerDied += ShowGameOverMessage;
            DrawCirclesAroundPlayer(gameCanvas, p.playerX, p.playerY);

        }

        

        private void GameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F)
            {
                foreach(Enemy enemy in level.Enemies) p.Attack(enemy);
                foreach (Turret turret in level.Turrets) p.Attack(turret);
            }
            else if (e.Key == Key.Up && p.playerY > 0)
            {
                p.playerY -= 10;
                Canvas.SetTop(playerImage, p.playerY);
            }
            else if (e.Key == Key.Down && p.playerY < 355)
            {
                p.playerY += 10;
                Canvas.SetTop(playerImage, p.playerY);
            }
            else if (e.Key == Key.Left && p.playerX > 0)
            {
                p.playerX -= 10;
                Canvas.SetLeft(playerImage, p.playerX);
            }
            else if (e.Key == Key.Right && p.playerX < 385)
            {
                p.playerX += 10;
                Canvas.SetLeft(playerImage, p.playerX);
            }
            else if (e.Key == Key.Escape)
            {
                System.ComponentModel.CancelEventArgs cancelArgs = new System.ComponentModel.CancelEventArgs();
                MainWindow_Closing(this, cancelArgs);
            }
           DrawCirclesAroundPlayer(gameCanvas, p.playerX, p.playerY);
            p.PickUpBonus(level.bonus);
        }
        private void DrawCirclesAroundPlayer(Canvas canvas, int playerX, int playerY)
        {
            int circleRadius = 260;
            double adjustedX = playerX - circleRadius;
            double adjustedY = playerY - circleRadius;

            Canvas.SetLeft(circle1, adjustedX + circleRadius);
            Canvas.SetTop(circle1, adjustedY + circleRadius);
            Canvas.SetLeft(circle2, adjustedX + circleRadius);
            Canvas.SetTop(circle2, adjustedY - circleRadius);
            Canvas.SetLeft(circle3, adjustedX - circleRadius - 30);
            Canvas.SetTop(circle3, adjustedY + circleRadius);
            Canvas.SetLeft(circle4, adjustedX - circleRadius - 30);
            Canvas.SetTop(circle4, adjustedY - circleRadius);
        }
      
    }
   
}
