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
        public TileManager(Texture2D spriteSheet, SpriteBatch spriteBatch)
        {
            this.spriteSheet = spriteSheet;
            this.spriteBatch = spriteBatch;
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
                int tileWidth = int.Parse(splitData[0]);
                int tileHeight = int.Parse(splitData[1]);

                // Line 2: How many tiles are there?
                line = reader.ReadLine();
                splitData = line.Split(',');
                int tilesetColumns = int.Parse(splitData[0]);
                int tilesetRows = int.Parse(splitData[1]);

                // Initialize the tileSet array to the correct size
                tileList = new Tile[tilesetColumns, tilesetRows];

                // ***READ TILE TEXTURE INFORMATION TO GENERATE LEVELTILES ***
                // ------------------------------------------------------------------------------------
                // Read data line by line for tiles
                while ((line = reader.ReadLine()) != null)
                {
                    // Get the upper left corner of the source rectangle in the spritesheet
                    splitData = line.Split(',');
                    int upperLeftX = int.Parse(splitData[0]);
                    int upperLeftY = int.Parse(splitData[1]);

                    // For each of the tiles across a row...
                    for (int c = 0; c < tilesetColumns; c++)
                    {
                        //tile is placed in 2D tileList array at correct placement
                        Tile myTile = new Tile(
                            spriteSheet,
                            new Rectangle(c*tileWidth, currentRow*tileWidth, tileWidth, tileHeight),
                            new Rectangle(upperLeftX,upperLeftY,tileWidth,tileHeight),
                            spriteBatch);

                        tileList[c, currentRow] = myTile;
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
