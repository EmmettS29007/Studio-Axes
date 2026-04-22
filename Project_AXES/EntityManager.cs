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
        private List<Enemy> enemies;
        private List<Vector2> startingPositions;
        private Enemy milk;

        // Instance of player 
        private Player player;

        // Bool to store whether or not the player has won
        private bool win;

        /// <summary>
        /// Returns the win bool so the game can stop and let the player win
        /// </summary>
        public bool Win
        {
            get { return win; }
        }

        /// <summary>
        /// Constructor for EntityManager
        /// </summary>
        /// <param name="enemies"> A list of enemies for the class to "manage" </param>
        /// <param name="milkSprite"> The sprite for milk </param>
        /// <param name="player"> The player that the manager will use for damaging enemies and said player </param>
        public EntityManager(List<Enemy> enemies, Texture2D milkSprite, Player player)
        {
            this.enemies = enemies;

            // For the amount of enemies in enemies...
            for (int i = 0; i < enemies.Count; i++)
            {
                // Set their health to 3
                enemies[i].Health = 3;
            }

            // Instantiate the startingPositions list
            startingPositions = new List<Vector2>();

            // For each enemy in enemies...
            foreach (Enemy enemy in this.enemies)
            {
                // Add their starting position to startingPositions
                startingPositions.Add(new Vector2(enemy.Position.X, enemy.Position.Y));
            }

            // Put all necessary data into milk
            milk = new Enemy(milkSprite, 1, new Vector2(500, 500), 0);

            // Set this player to the provided player
            this.player = player;
        }

        /// <summary>
        /// Allows for frame-by-frame updates to EntityManager
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // For the amount of enemies in enemies...
            for (int i = 0; i < enemies.Count; i++)
            {
                // If their current x equals their starting x...
                if (enemies[i].X == startingPositions[i].X)
                {
                    // Tell them they can move forward
                    enemies[i].Moving = true;
                }

                // If their current X is greater than or equal to their startingPosition + range...
                if (enemies[i].X >= startingPositions[i].X + enemies[i].Range)
                {
                    // Tell them they should be moving backwards
                    enemies[i].Moving = false;
                }

                // If the enemy should be moving...
                if (enemies[i].Moving == true)
                {
                    // Move them forward!
                    enemies[i].X += 2;
                }

                // Otherwise...
                else
                {
                    // Move them backwards!
                    enemies[i].X -= 2;
                }

                // If the player intersects with an enemy...
                if (player.Position.Intersects(enemies[i].Position))
                {
                    // Attack the player
                    enemies[i].Attack(player);
                }

                // If the player's attack intersects with an enemy...
                if (player.Attack.Intersects(enemies[i].Position))
                {
                    // Damage that enemy
                    enemies[i].TakeDamage();
                }

            }

            // If the player touches the milk...
            if (player.Position.Intersects(milk.Position))
            {
                // "Kill" the milk
                milk.Die();
            }

            // If the milk is dead...
            if (milk.isDead == true)
            {
                // Set win to true!
                win = true;
            }
        }

        /// <summary>
        /// Allows for the enemies and the milk to be drawn to the screen
        /// </summary>
        /// <param name="sb"> The spritebatch needed to draw </param>
        public void Draw(SpriteBatch sb)
        {
            // For each enemy in enemies...
            foreach (Enemy enemy in enemies)
            {
                // If they're not dead...
                if (enemy.isDead == false)
                {
                    // Draw them to the screen
                    sb.Draw(enemy.getSprite, enemy.Position, Color.White);
                }
            }

            // If the milk isn't dead...
            if (milk.isDead == false)
            {
                // Draw the milk to the screen.
                sb.Draw(milk.getSprite, milk.Position, Color.White);
            }
        }

    }
}
