using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;
using System.Diagnostics.Metrics;

namespace Project_AXES
{
    internal class Menu
    {
        // Drawing the menu to the screen
        private SpriteFont font;
        private Texture2D title;
        private Texture2D button;
        private Texture2D controls;

        // Utilized for button positioning
        private int buttonOffset;
        private int windowWidth;
        private int windowHeight;
        private List<Rectangle> buttonList;
        private Rectangle controlsGuide;

        //  Utilized for user input
        private MouseState mouse;
        private MouseState previousMouse;
        public Menu(SpriteFont font, Texture2D button, Texture2D title, Texture2D controls, int windowWidth, int windowHeight)
        {
            this.title = title;
            this.button = button;
            this.font = font;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.controls = controls;
            buttonList = new List<Rectangle>();
            buttonOffset = button.Height + 50;

            // Each element of the list is a new button
            // buttonList[0] is the start button
            // buttonList[1] is the help button
            for (int i = 0; i < 2; i++)
            {
                buttonList.Add(new Rectangle(0, 0 + (buttonOffset * i), button.Width, button.Height));
            }
        }
        public void Update(GameState gs)
        {
            previousMouse = mouse;
            mouse = Mouse.GetState();

            if (buttonList[0].Contains(mouse.X, mouse.Y) 
                && (previousMouse.LeftButton == ButtonState.Released 
                    && mouse.LeftButton == ButtonState.Pressed))
            {
                gs = GameState.Game;
            }
            // If the control rectangle does not exist, and the mouse is inside of the help buttons
            // and the left mouse button is pressed once...
            else if (controlsGuide.IsEmpty && buttonList[1].Contains(mouse.X, mouse.Y)
                && (previousMouse.LeftButton == ButtonState.Released
                    && mouse.LeftButton == ButtonState.Pressed))
            {
                // Create the controls guide rectangle
                controlsGuide = new Rectangle(((windowWidth - controls.Width) / 2),
                    (windowHeight - controls.Height),
                    controls.Width,
                    controls.Height);
            }
            // If the control rectangle exists , and the mouse is outside of the guide
            // and the left mouse button is pressed once...
            else if (!controlsGuide.IsEmpty 
                && !(controlsGuide.Contains(mouse.X, mouse.Y))
                && (previousMouse.LeftButton == ButtonState.Released
                    && mouse.LeftButton == ButtonState.Pressed))
            {
                // Leave the controls guide
                controlsGuide = new Rectangle(0, 0, 0, 0);
            }
        }
        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < buttonList.Count; i++)
            {
                sb.Draw(button, buttonList[i], Color.White);
            }
        }
    }
}
