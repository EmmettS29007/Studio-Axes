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
    public class CollisionTile : Tile ,ICollidable
    {
        //---CONSTRUCTOR---

        /// <summary>
        /// Ctor for tiles
        /// </summary>
        /// <param name="spriteSheet">sprite sheet</param>
        /// <param name="sourceRect">rectangle this is printed</param>
        /// <param name="spriteSheetRect">rectangle that is taken from the sprite sheet</param>
        /// <param name="sb"></param>

        public CollisionTile(Texture2D spriteSheet, Rectangle sourceRect, Rectangle spriteSheetRect, SpriteBatch sb):
            base (spriteSheet, sourceRect, spriteSheetRect,sb)
        {
            this.spriteSheet = spriteSheet;
            this.sb = sb;
            this.sourceRect = sourceRect;
            this.spriteSheetRect = spriteSheetRect;
        }

        //---METHODS---

        // just adds collision stuff

        /// <summary>
        /// Detects any Collisions with another Collidable object, Calls Push if Needed
        /// </summary>
        /// <param name="other">The other collidable</param>
        public void DetectCollision(ICollidable other)
        {
        }

        /// <summary>
        /// Would normally change the x and y based off being collided with,
        /// </summary>
        /// <param name="xAmt">The X axis change</param>
        /// <param name="yAmt">The Y axis change</param>
        public void Push(int xAmt, int yAmt)
        {
            //This method doesn't need to do anything
        }


    }
}