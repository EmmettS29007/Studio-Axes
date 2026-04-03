using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileManager
{
    public class Tile
    {
        //---FIELDS---
        private Texture2D spriteSheet;
        private Rectangle sourceRect;

        //---CONSTRUCTOR---
        public Tile(Texture2D spriteSheet)
        {
            this.spriteSheet = spriteSheet;
        }
        //---METHODS---

        /// <summary>
        /// Render this LevelTile to the game window.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteSheet, sourceRect, Color.White);
        }
    }
}
