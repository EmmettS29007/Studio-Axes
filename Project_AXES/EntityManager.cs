using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_AXES
{
    public class EntityManager
    {
        // Field

        // Enemies
        private List<Enemy> enemies;
        private List<Vector2> startingPositions;
        private Enemy milk;

        // Player
        private Player player;

        // Win (?)
        private bool win;


        /// <summary>
        /// Constructor for EntityManager, taking 3 variables
        /// </summary>
        /// <param name="enemies"> A list of enemies to manage </param>
        /// <param name="milkSprite"> The milk's sprite </param>
        /// <param name="player"> The player </param>
        public EntityManager(List<Enemy> enemies, Texture2D milkSprite, Player player)
        {
            // Set up the local list of enemies
            this.enemies = enemies;

            // Initialize the startingPositions
            startingPositions = new List<Vector2>();

            // For each enemy in the enemies List...
            foreach (Enemy enemy in this.enemies)
            {
                // Add that enemy's starting position to startingPositions
                startingPositions.Add(new Vector2(enemy.Position.X, enemy.Position.Y));
            }

            // Set up the milk
            milk = new Enemy(milkSprite, 1, new Vector2(500, 500), 0);
            
            // Set up the player
            this.player = player;
        }

        /// <summary>
        /// Update method for EntityManager, allows for real-time updates for the entities being managed
        /// </summary>
        /// <param name="gameTime"> The elapsedTime</param>
        public void Update(GameTime gameTime)
        {
            // For the amount of enemies in enemies...
            for (int i = 0; i < enemies.Count; i++)
            {
                // If an enemy's current x coord equals their starting x coord...
                if (enemies[i].X == startingPositions[i].X)
                {
                    // Tell the enemy to start moving right
                    enemies[i].Moving = true;
                }

                // If the enemy has reached their range...
                if (enemies[i].X == startingPositions[i].X + enemies[i].Range)
                {
                    // Tell the enemy to start moving left
                    enemies[i].Moving = false;
                }

                // If the enemy should be moving right...
                if (enemies[i].Moving == true)
                {
                    // Add 2 to their x coord to move them right
                    enemies[i].X += 2;
                }

                // If not...
                else
                {
                    // Subtract 2 to their x coord to move them left
                    enemies[i].X -= 2;
                }

                // If the enemy isn't dead...
                if (enemies[i].isDead == false)
                {
                    // If the player is touching the enemy...
                    if (player.Position.Intersects(enemies[i].Position))
                    {
                        // If the player's health is greater than 0...
                        if (player.Health > 0)
                        {
                            // Subtract one from the player's health
                            player.Health--;
                        }

                        // Otherwise...
                        else
                        {
                            // The player should die
                            player.Die();
                        }
                    }

                    // If the player's attack intersects the enemy...
                    if (player.Attack.Intersects(enemies[i].Position))
                    {
                        // That enemy should take damage
                        enemies[i].TakeDamage();
                    }
                }

            }

            // If the player touches the milk...
            if (player.Position.Intersects(milk.Position))
            {
                // Kill da milk
                milk.Die();
            }
        }

        /// <summary>
        /// Allows for most entities in entityManager to be drawn to the screen
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            // For each enemy in the enemies List...
            foreach (Enemy enemy in enemies)
            {
                // If the enemy isn't dead...
                if (enemy.isDead == false)
                {
                    // Draw the enemy to the screen
                    sb.Draw(enemy.getSprite, enemy.Position, Color.White);
                }
            }

            // If the milk isn't dead...
            if (milk.isDead == false)
            {
                // Draw the milk to the screen
                sb.Draw(milk.getSprite, milk.Position, Color.White);
            }
        }

    }
}
