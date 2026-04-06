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
            // Draw task in bottom left, with the words "Current task" above the current task!
        }
    }
}
