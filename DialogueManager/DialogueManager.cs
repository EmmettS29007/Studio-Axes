using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DialogueManager
{
    internal class DialogueManager
    {
        // Variable Block
        private List<string> dialogue;
        private string filename;
        private SpriteFont font;
        private Texture2D pixel;

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
        public DialogueManager(SpriteFont font, SpriteBatch sb) 
        {
            this.font = font;
            dialogue = new List<string>();
            filename = null;

            // Creation of a "pixel" referenced code from MonoGame Community and DebugLib
            // https://community.monogame.net/t/whats-the-simplest-way-to-draw-a-rectangular-outline-without-generating-the-texture/7818/4
            // https://github.com/not-phoeniix/DebugLibDemo
            pixel = new Texture2D(sb.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
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
                        "Did you remember to add the file to the MGCB Editor AND set its Build Action to 'Copy'? ");
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
        // Read dialogue with File.IO and save it to list.
        // ^ List b/c we do not know how many lines the dialogue will be.
        // First, need a text file.
        // Then, take the file and read it with Streamreader 
        // If the line starts with "//" then skip the line.
        public void Update(GameTime gt)
        {
            if (filename != null)
            {
                ReadDialogue(filename);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            
        }

    }
}
