using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AXES_Player
{
    internal class Tile : ICollidable
    {
        Rectangle rect;
        public Tile(Rectangle rect)
        {
            this.rect = rect;
        }

        /// <summary>
        /// Gets the positioning of the rectangle
        /// </summary>
        public Rectangle Position { get { return rect; } }
        public void Draw(SpriteBatch sb)
        {
            DebugLib.DrawRectFill(sb, rect, Color.Red);
        }

        /// <summary>
        /// Detects any Collisions with another Collidable object, Calls Push if Needed
        /// </summary>
        /// <param name="other">The other collidable</param>
        public void DetectCollision(ICollidable other)
        {
        }

        /// <summary>
        /// Would normally change the x and y based off being collided with, used for camera
        /// </summary>
        /// <param name="xAmt">The X axis change</param>
        /// <param name="yAmt">The Y axis change</param>
        public void Push(int xAmt, int yAmt)
        {
            rect.X += xAmt;
            rect.Y += yAmt;
        }


    }
}