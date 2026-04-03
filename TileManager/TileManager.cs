using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileManager
{
    //This class manages tiles
    public class TileManager
    {
        //---FIELDS---
        //needed for drawing
        private SpriteBatch spriteBatch;

        //sprite sheet
        private Texture2D spriteSheet;

        //Tileset list
        private Tile[,] tileList;

        //NOTE: Game window is about 30x17 tiles

        //---CONSTRUCTOR---
        public TileManager()
        {
            
        }

        //---METHODS---

        /// <summary>
        /// Reads file with tiles and puts them in tile array
        /// </summary>
        /// <param name="filepath">file to read through</param>
        public void AssignTiles(string filepath)
        {
            try
            {
                // ***SETUP VARIABLES FOR FILE READING: ***
                // ------------------------------------------------------------------------------------
                StreamReader reader = new StreamReader(filepath);
                string line = "";
                string[] splitData = null;

                // Needed for determining individual tile data placement
                int currentRow = 0;

                // ***GET DATA FROM FIRST 2 LINES FOR TILE INFORMATION ***
                // ------------------------------------------------------------------------------------
                // The first 2 lines give information about this level:
                // Line 1: How large should the level tiles be?
                line = reader.ReadLine();
                splitData = line.Split(',');
                int tileWidth = int.Parse(splitData[1]);
                int tileHeight = int.Parse(splitData[2]);

                // Line 2: How many tiles are there?
                line = reader.ReadLine();
                splitData = line.Split(',');
                int tilesetColumns = int.Parse(splitData[1]);
                int tilesetRows = int.Parse(splitData[2]);

                // Initialize the tileSet array to the correct size
                tileSet = new LevelTile[tilesetColumns, tilesetRows];

                // ***READ TILE TEXTURE INFORMATION TO GENERATE LEVELTILES ***
                // ------------------------------------------------------------------------------------
                // Read data line by line for tiles
                while ((line = reader.ReadLine()) != null)
                {
                    // Get this line of tile data and split by comma.
                    // That gives us data like: "WATER-inner,GRASS-1,DIRT-2"
                    splitData = line.Split(',');

                    // For each of the tiles across a row...
                    for (int c = 0; c < splitData.Length; c++)
                    {
                        // ************************************************************************
                        // TODO: Create a new LevelTile object based on the information read from the file
                        // ^^ Position it correctly within the 2D array and with appropriate calculated positions within the game window
                        // TODO: Place tile in the 2D tileSet array at the correct row & column indices
                        LevelTile myTile = new LevelTile(
                            spriteSheet,
                            new Rectangle(64, 0, intendedSize, intendedSize),
                            c,
                            currentRow);

                        tileSet[c, currentRow] = myTile;


                        // ************************************************************************
                    }

                    // Increase the row
                    currentRow++;
                }

                // Close the stream
                reader.Close();
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine("FILE-READING ERROR UPON LOADING LEVEL!");
                System.Diagnostics.Debug.WriteLine(error.Message);
            }
        }

        /// <summary>
        /// Draw all LevelTiles to the game window.
        /// </summary>
        /// <param name="_spriteBatch">SpriteBatch object (passed in from Game1 Draw)</param>
        public void DisplayTiles()
        {
            // Iterate and draw all tiles in the 2D array of LevelTiles.
            for (int r = 0; r < tileList.GetLength(0); r++)
            {
                for (int c = 0; c < tileList.GetLength(1); c++)
                {
                    tileList[r, c].Draw(spriteBatch);
                }
            }
        }
    }
}
