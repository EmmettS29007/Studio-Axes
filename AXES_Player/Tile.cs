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
    internal class Tile
    {
        Rectangle rect;
        public Tile(Rectangle rect)
        {
            this.rect = rect;
        }

        public Rectangle Rect { get { return rect; } }

        public void Draw(SpriteBatch sb)
        {
            DebugLib.DrawRectFill(sb, rect, Color.Red);
        }


    }
}