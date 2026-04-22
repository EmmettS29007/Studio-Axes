using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_AXES
{
    internal class Menu
    {
        // Drawing the menu to the screen
        private SpriteFont font;
        private Texture2D title;
        private Texture2D button;

        // Utilized for button positioning
        private int buttonOffset;
        private int windowWidth;
        private int windowHeight;

        //
        private List<Rectangle> buttonList;
        private MouseState mouse;
        private MouseState previousMouse;
        public Menu(SpriteFont font, Texture2D button, Texture2D title, int windowWidth, int windowHeight)
        {
            this.title = title;
            this.button = button;
            this.font = font;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

        }
        public void Draw()
        {
            
        }
    }
}
