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

        // Utilized for button positioning and drawing
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
                buttonList.Add(new Rectangle(((windowWidth - controls.Width) / 2),
                        ((windowHeight + controls.Height)) / 4 + (buttonOffset * i),
                        button.Width, 
                        button.Height));
            }
        }

        /// <summary>
        /// Updates the menu and gameState based on the given input
        /// </summary>
        /// <returns>A new value for gameState depending on the button pressed</returns>
        public GameState Update()
        {
            previousMouse = mouse;
            mouse = Mouse.GetState();

            // Checks if the LMB was pressed once...
            if ((previousMouse.LeftButton == ButtonState.Pressed
                    && mouse.LeftButton == ButtonState.Released))
            {
                // If the start button was pressed...
                if (buttonList[0].Contains(mouse.X, mouse.Y))
                {
                    // Start the game!
                    return GameState.Game;
                }
                // If the control rectangle does not exist, and the mouse is inside of the help button
                if (controlsGuide.IsEmpty && buttonList[1].Contains(mouse.X, mouse.Y))
                {
                    // Create the controls guide rectangle
                    controlsGuide = new Rectangle(((windowWidth - controls.Width) / 2),
                        ((windowHeight - controls.Height)) / 2,
                        controls.Width,
                        controls.Height); 
                    // And stay in the menu
                    return GameState.Menu;
                }
                // If the control rectangle exists , and the mouse is outside of the guide
                if (!controlsGuide.IsEmpty
                    && !(controlsGuide.Contains(mouse.X, mouse.Y)))
                {
                    // Leave the controls guide and stay in the menu
                    controlsGuide = new Rectangle(0, 0, 0, 0);
                    return GameState.Menu;
                }
            }
            // Else, stay in the menu when doing nothing
            return GameState.Menu;
        }

        /// <summary>
        /// Draws the buttons and text for the main menu
        /// </summary>
        /// <param name="sb">The spritebatch which holds the textures and fonts</param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(title, 
                new Rectangle(0,0, windowWidth, windowHeight), 
                Color.White);

            for (int i = 0; i < buttonList.Count; i++)
            {
                sb.Draw(button, buttonList[i], Color.CornflowerBlue);
            }

            // Draws the text for the start button
            sb.DrawString(font,
            "START GAME",
            new Vector2(((button.Width - font.MeasureString("START GAME").X) / 2) + buttonList[0].X,
            button.Height / 2 + buttonList[0].Y),
            Color.IndianRed);

            // Draws the text for the help button
            sb.DrawString(font,
            "CONTROLS / HELP",
            new Vector2(((button.Width - font.MeasureString("CONTROLS / HELP").X) / 2) + buttonList[1].X,
            button.Height / 2 + buttonList[1].Y),
            Color.IndianRed);

            if (!controlsGuide.IsEmpty)
            {
                sb.Draw(controls, controlsGuide, Color.IndianRed);
            }
        }
    }
}
