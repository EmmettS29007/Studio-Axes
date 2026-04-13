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

namespace Project_Axes
{
    internal class DialogueManager
    {
        // File.IO Variables
        // List that contains the dialogue
        private List<string> dialogue;
        // The name and location of the file that contains the dialogue
        private string filename;
        // The current line of dialogue being drawn to the screen
        private string currentLine;

        // Variables used for drawing to the screen & positioning
        private SpriteFont font;
        private Texture2D dialogueSprite;
        private Rectangle dialoguePosition;
        private int windowWidth;
        private int windowHeight;

        // Used for checking if [ENTER] was pressed, which progresses the dialogue
        private KeyboardState kbState;
        private KeyboardState kbPrevState;
        // Used for checking if the game is still in a "dialogue" state
        private bool dialogueActivated;

        /// <summary>
        /// Private variable used for dialogue line counting
        /// </summary>
        private int i;

        /// <summary>
        /// The name and location of the file to read and save dialogue
        /// </summary>
        public string FileName
        {
            get { return filename; }
            set { filename = value; }
        }

        /// <summary>
        /// Parametized constructor of the Dialogue Manager class.
        /// Takes in a SpriteFont and sets the font used.
        /// Takes in a SpriteBatch to create the dialogue boxes
        /// </summary>
        public DialogueManager(SpriteFont font, Texture2D dialogueSprite, int windowWidth, int windowHeight)
        {
            this.font = font;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.dialogueSprite = dialogueSprite;
            dialoguePosition = new Rectangle(((windowWidth - dialogueSprite.Width) / 2),
                (windowHeight - dialogueSprite.Height),
                dialogueSprite.Width,
                dialogueSprite.Height);
            dialogue = new List<string>();
            filename = null;
            currentLine = null;
            dialogueActivated = false;
        }

        /// <summary>
        /// Takes the dialogue from a file and saves it to the "dialogue" list.
        /// </summary>
        /// <param name="filename">The file to search for</param>
        /// <exception cref="Exception">If the file can not be found, an Exception is thrown</exception>
        private void ReadDialogue(string filename)
        {
            if (!File.Exists(filename))
            {
                System.Diagnostics.Debug.WriteLine($"Error: Cannot find file '{filename}' in the output directory!");
                System.Diagnostics.Debug.WriteLine($"       Did you remember to add the file to the MGCB Editor AND set its Build Action to 'Copy'?");
                System.Diagnostics.Debug.WriteLine($"       Remember to format the file correctly as well according to the guide document!");

            }
            else
            {
                StreamReader reader = null;
                string line = "";
                try
                {
                    reader = new StreamReader(filename);
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("//"))
                        {
                            continue;
                        }
                        else
                        {
                            dialogue.Add(line);
                        }
                    }
                }
                catch
                {
                    throw new Exception("There was an error retrieving the file. " +
                        "Did you remember to add the file to the MGCB Editor AND set its Build Action to 'Copy'?");
                }
                finally
                {
                    // If the reader was opened, close it and reset the filename to null
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }

        }

        /// <summary>
        /// Private method used for exiting dialogue
        /// </summary>
        private void ExitDialogue()
        {
            // Empties the dialogue lst
            // The filename is nulled, and thus needs to be reassigned

            // Resets the dialogue List by resetting the variable
            dialogue = null;
            dialogue = new List<string>();
            currentLine = null;
            dialogueActivated = false;
            i = 0;

        }

        /// <summary>
        /// Upon pressing the "ENTER" key, the dialogue progresses
        /// by one line
        /// </summary>
        /// <param name="gt">Current time in the game</param>
        public void Update(GameTime gt, bool dialogueConditional)
        {
            // Used for singular key input checking 
            kbPrevState = kbState;
            kbState = Keyboard.GetState();

            // Activates dialogue depending on the external conditional, ideally 
            // changed in Game1
            dialogueActivated = dialogueConditional;

            // If the file is null, the dialogue is added to dialogue list.
            if (filename != null)
            {
                ReadDialogue(filename);
                filename = null;
            }
            if (dialogueActivated)
            {
                if (dialogue.Count != 0 && i < dialogue.Count)
                {
                    currentLine = dialogue[i];
                }
                if ((kbPrevState.IsKeyDown(Keys.Enter) && kbState.IsKeyUp(Keys.Enter)))
                {
                    i++;
                }
                // If the dialogue is at it's maximum, and the player presses enter again, the dialogue is exited
                else if ((i > dialogue.Count - 1))
                {
                    dialogueConditional = false;
                    ExitDialogue();
                }
            }

        }

        /// <summary>
        /// Draws the current dialogue line and text box to the window
        /// </summary>
        /// <param name="sb">The spritebatch to take the font and text box from</param>
        public void Draw(SpriteBatch sb)
        {
            //sb.Begin();
            if (dialogueActivated)
            {
                sb.Draw(dialogueSprite, dialoguePosition, Color.White);

                // Alligns the text to the center of the dialogue box
                sb.DrawString(font,
                    currentLine,
                    new Vector2(((dialogueSprite.Width - font.MeasureString(currentLine).X) / 2) + dialoguePosition.X,
                    dialogueSprite.Height / 2 + dialoguePosition.Y),
                    Color.White);
            }

            //sb.End();
        }

    }
}
