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
        private Rectangle spriteSheetRect;

        //---CONSTRUCTOR---

        /// <summary>
        /// Ctor for tiles
        /// </summary>
        /// <param name="spriteSheet">sprite sheet</param>
        /// <param name="sourceRect">rectangle this is printed</param>
        /// <param name="spriteSheetRect">rectangle that is taken from the sprite sheet</param>
        /// <param name="sb"></param>
        public Tile(Texture2D spriteSheet, Rectangle sourceRect, Rectangle spriteSheetRect, SpriteBatch sb)
        {
            this.spriteSheet = spriteSheet;
            this.sb = sb;
            this.sourceRect = sourceRect;
            this.spriteSheetRect = spriteSheetRect;
        }
        //---METHODS---

        /// <summary>
        /// Render this LevelTile to the game window.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteSheet, sourceRect, spriteSheetRect, Color.White);
        }
    }
}
