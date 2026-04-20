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
        private List<Enemy> enemies;
        private List<Vector2> startingPositions;
        private Enemy milk;
        private bool win;

        public EntityManager(List<Enemy> enemies, Texture2D milkSprite)
        {
            this.enemies = enemies;
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Health = 3;
            }

            startingPositions = new List<Vector2>();
            foreach (Enemy enemy in this.enemies)
            {
                startingPositions.Add(new Vector2(enemy.Position.X, enemy.Position.Y));
            }

            milk = new Enemy(milkSprite, 1, new Vector2(500, 500), 0);
            milk.Health = 1;
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
                    if (player.Health > 0)
                    {
                        player.Health--;
                    }

                    if (player.Health <= 0)
                    {
                        player.Die();
                    }
                }

            }

            if (player.Position.Intersects(milk.Position))
            {
                milk.Die();
            }

            if (milk.isDead == true)
            {
                win = true;
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
            if (milk.isDead == false)
            {
                sb.Draw(milk.getSprite, milk.Position, Color.White);
            }
        }

    }
}
