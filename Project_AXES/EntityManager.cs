using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_AXES
{
    public class EntityManager
    {
        private List<Enemy> enemies;
        private List<Vector2> startingPositions;

        public EntityManager(List<Enemy> enemies)
        {
            this.enemies = enemies;
            startingPositions = new List<Vector2>();
            foreach (Enemy enemy in this.enemies)
            {
                startingPositions.Add(new Vector2(enemy.Position.X, enemy.Position.Y));
            }
        }

        public void Update(GameTime gameTime, Player player)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].X == startingPositions[i].X)
                {
                    enemies[i].Moving = true;
                }

                if (enemies[i].X >= startingPositions[i].X + enemies[i].Range)
                {
                    enemies[i].Moving = false;
                }

                if (enemies[i].Moving == true)
                {
                    enemies[i].X += 2;
                }

                else
                {
                    enemies[i].X -= 2;
                }

                /* if (player's attack collides with enemy)
                 * - TakeDamage()
                 * 
                */

                if (player.Position.Intersects(enemies[i].Position))
                {
                    player.Health -= 1;
                }

            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.isDead == false)
                {
                    sb.Draw(enemy.getSprite, enemy.Position, Color.White);
                }
            }
        }

    }
}
