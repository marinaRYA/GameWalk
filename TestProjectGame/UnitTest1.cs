
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using System;
using System.Reflection;

namespace GameWalk
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TurretProjectile_Move_SetsNewCoordinates()
        {
            // Arrange
            TurretProjectile projectile = new TurretProjectile(0, 0, 1, 1, 10);

            // Act
            projectile.Move(new player { playerX = 5, playerY = 5 });

            // Assert
            Assert.AreNotEqual(0, projectile.X);
            Assert.AreNotEqual(0, projectile.Y);
        }

        [TestMethod]
        public void TurretProjectile_Attack_ReducesPlayerHealth()
        {
            // Arrange
            TurretProjectile projectile = new TurretProjectile(0, 0, 1, 1, 10);
            player player = new player();

            // Act
            projectile.Attack(player);

            // Assert
            Assert.IsTrue(player.health < 200);
        }

        [TestMethod]
        public void Turret_TakeDamage_ReducesTurretHealth()
        {
            // Arrange
            Turret turret = new Turret(0, 0, 10, 50, 100, 5, 5);

            // Act
            turret.TakeDamage(20);

            // Assert
            Assert.AreEqual(80, turret.Health);
        }

        [TestMethod]
        public void Enemy_Attack_ReducesPlayerHealth()
        {
            // Arrange
            Enemy enemy = new Enemy(0, 0, 5, 50, 10);
            player player = new player();

            // Act
            enemy.Attack(player);

            // Assert
            Assert.IsTrue(player.health < 100);
        }
        [TestMethod]
        public void Player_AttackEnemy_EnemyHealthReduced()
        {
            // Arrange
            player player = new player();
            Enemy enemy = new Enemy(200, 200, 3, 50, 10);

            // Act
            player.Attack(enemy);

            // Assert
            Assert.IsTrue(enemy.Health < 50);
        }

        

        [TestMethod]
        public void Player_TakeDamage_PlayerDiedEventFired()
        {
            // Arrange
            player player = new player();
            bool playerDiedEventFired = false;
            player.PlayerDied += () => playerDiedEventFired = true;

            // Act
            player.TakeDamage(player.health + 1);

            // Assert
            Assert.IsTrue(playerDiedEventFired);
        }

        [TestMethod]
        public void Player_PickUpBonus()
        {
            // Arrange
            Bonus bonus = new Bonus(200, 200, 10, true);
            player player = new player();

            // Act
            player.PickUpBonus(bonus);

            // Assert
            Assert.IsFalse(bonus.IsExist);
        }
        [TestMethod]
        public void SetLevel_Level1_CreatesEnemiesAndBonus()
        {
            // Arrange
            Level level = new Level();

            // Act
            level.SetLevel(1);

            // Assert
            Assert.IsNotNull(level.Enemies);
            Assert.AreEqual(2, level.Enemies.Count);
            Assert.IsNotNull(level.bonus);
            Assert.IsFalse(level.bonus.IsExist);
        }

        [TestMethod]
        public void SetLevel_Level2_CreatesEnemiesTurretsAndBonus()
        {
            // Arrange
            Level level = new Level();

            // Act
            level.SetLevel(2);

            // Assert
            Assert.IsNotNull(level.Enemies);
            Assert.AreEqual(2, level.Enemies.Count);
            Assert.IsNotNull(level.Turrets);
            Assert.AreEqual(1, level.Turrets.Count);
            Assert.IsNotNull(level.bonus);
            Assert.IsTrue(level.bonus.IsExist);
        }

        [TestMethod]
        public void SetLevel_Level3_CreatesTurretsAndBonus()
        {
            // Arrange
            Level level = new Level();

            // Act
            level.SetLevel(3);

            // Assert
            Assert.IsNotNull(level.Turrets);
            Assert.AreEqual(2, level.Turrets.Count);
            Assert.IsNotNull(level.bonus);
            Assert.IsTrue(level.bonus.IsExist);
        }

        
    }
}