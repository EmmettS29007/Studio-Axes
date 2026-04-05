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
        private SpriteBatch sb;

        //---CONSTRUCTOR---
        public Tile(Texture2D spriteSheet, Rectangle sourceRect, Rectangle spriteSheetRect, SpriteBatch sb)
        {
            this.spriteSheet = spriteSheet;
            this.sb = sb;
            this.sourceRect = sourceRect;
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
