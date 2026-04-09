using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Project_AXES
{
    public interface ICollidable
    {
        /// <summary>
        /// Gets the positioning of the rectangle
        /// </summary>
        public Rectangle Position { get; }

        /// <summary>
        /// Detects any Collisions with another Collidable object, Calls Push if Needed
        /// </summary>
        /// <param name="other">The other collidable</param>
        public void DetectCollision(ICollidable other);

        /// <summary>
        /// The amount that the object moves due to the collision
        /// </summary>
        /// <param name="xAmt">The amount on the X axis</param>
        /// <param name="yAmt">The amount on the Y axis</param>
        public void Push(int xAmt, int yAmt);
    }
}