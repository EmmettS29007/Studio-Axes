using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemy_EntityManager_HUD
{
    public class HUD
    {
        private Texture2D heart;
        // private Player player;
        private string task;
        private SpriteFont arial32;

        public HUD (Texture2D heart, string task, SpriteFont arial32) // Player player
        {
            this.heart = heart;
            this.task = task;
            // this.player = player;
            this.arial32 = arial32;
        }

        public void Update(GameTime gameTime)
        {
            // If task completed, change the current task
        }

        public void Draw(SpriteBatch sb)
        {
            // Draw hearts in top left, use Player health int for how many hearts,
            // int hearts = player.health;
            int hearts = 3;
            for (int i =  0; i < hearts; i++)
            {
                sb.Draw(heart,
                    new Rectangle(
                        5 + i * 45,
                        10,
                        60,
                        60),
                    Color.Red);
            }

            sb.DrawString(arial32, "Current Task: ", new Vector2(20, 480 - 95), Color.Black);
            sb.DrawString(arial32, task, new Vector2(20, 480 - 50), Color.Black);
        }
    }
}
