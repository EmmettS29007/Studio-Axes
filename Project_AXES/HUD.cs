using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_AXES
{
    public class HUD
    {
        private Texture2D heart;
        private int hearts;
        private Player player;
        private int taskIndex;
        private string[] tasks;
        private SpriteFont arial32;


        public HUD(Texture2D heart, string[] tasks, SpriteFont arial32, Player player)
        {
            this.heart = heart;
            this.tasks = tasks;
            this.player = player;
            this.arial32 = arial32;
        }

        public void Update()
        {
            taskIndex++;
        }

        public void Reset()
        {
            taskIndex = 0;
        }

        public void Draw(SpriteBatch sb, int y)
        {
            // Draw hearts in top left, use Player health int for how many hearts,
            // int hearts = player.health;
            for (int i = 0; i < player.Health; i++)
            {
                sb.Draw(heart,
                    new Rectangle(
                        5 + i * 45,
                        10,
                        60,
                        60),
                    Color.Red);
            }

            sb.DrawString(arial32, "Current Task: "+ tasks[taskIndex], new Vector2(20, 65), Color.Cyan);
        }
    }
}
