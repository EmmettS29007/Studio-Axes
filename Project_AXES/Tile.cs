using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project_AXES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Project_AXES
{
    public class Tile
    {
        //NON COLLIEDABLE TILE
        //Used for background
        //Collideable tiles inherit from here

        //---FIELDS---
        protected Texture2D spriteSheet;
        protected Rectangle sourceRect;
        protected SpriteBatch sb;
        protected Rectangle spriteSheetRect;
        protected bool inCamera;

        public bool InCamera { get { return inCamera; } set { inCamera = value; } }
        public Rectangle Position { get { return sourceRect; } }

        //---CONSTRUCTORS---
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
            inCamera = false;
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

        /// <summary>
        /// The amount that the object moves due to the Camera
        /// </summary>
        /// <param name="xAmt">The amount on the X axis</param>
        /// <param name="yAmt">The amount on the Y axis</param>
        public void Push(int xAmt, int yAmt)
        {
            sourceRect.X += xAmt;
            sourceRect.Y += yAmt;
        }
    }
}