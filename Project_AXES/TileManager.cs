using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_AXES
{
    //This class manages tiles
    //**NOTES

    // https://docs.google.com/spreadsheets/d/1L_lAuEy0PqPj4w9MGE8k5RVYLyzOUSqi7vtp6aE9b_U/edit?gid=0#gid=0
    // Link to google sheet level builder
    // Copy paste section that it tells you to into textureMappingData.txt

    //Game window is about 30x14 tiles,
    //overall this will change as we expand the level i would assume

    public class TileManager
    {
        //---FIELDS---
        //needed for drawing
        private SpriteBatch spriteBatch;

        //sprite sheet
        private Texture2D spriteSheet;

        //Tileset list
        private Tile[,] tileList;

        //Initializes a collideable tile list
        //this is needed so we can have collision checks
        List<CollisionTile> collideableTiles = new List<CollisionTile>();

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
                // --- INITIAL DATA READING ---
                StreamReader reader = new StreamReader(filepath);
                string line = "";
                string[] splitData = null;

                // Skip past the informational line
                reader.ReadLine();

                // Tile size reasding
                line = reader.ReadLine();
                splitData = line.Split(',');
                int tileWidth = int.Parse(splitData[0]);
                int tileHeight = int.Parse(splitData[1]);

                // Skip past the informational line
                reader.ReadLine();

                // Tile ct reading
                line = reader.ReadLine();
                splitData = line.Split(',');
                int tilesetColumns = int.Parse(splitData[0]);
                int tilesetRows = int.Parse(splitData[1]);

                // Skip past the informational line
                reader.ReadLine();

                // Sprite sheet tile size reading
                line = reader.ReadLine();
                splitData = line.Split(',');
                int sheetTileWidth = int.Parse(splitData[0]);
                int sheetTileHeight = int.Parse(splitData[1]);

                // Skip past the informational line
                reader.ReadLine();

                // Initialize the tileSet array to the correct size
                tileList = new Tile[tilesetColumns, tilesetRows];
                
                // --- INITIAL DATA FOR TILE ASSIGNMENT ---
                //lines
                int upperLeftX;
                int upperLeftY;

                //for keeping track of columns an rows
                int currentColumn = 0;
                int currentRow = 0;

                // --- TILE SORTING AND READING ---

                //sorts tiles into collideable and not collideable
                //places them into arrays and lists where appropriate
                //until the row and column hit max
                while ((currentRow != tilesetRows)&&(currentColumn!= tilesetColumns))
                {
                    //reads line and splits data correctly
                    line = reader.ReadLine(); //next line
                    splitData = line.Split(','); //gets data

                    // Get the upper left corner of the source rectangle in the spritesheet
                    upperLeftX = int.Parse(splitData[0]);
                    upperLeftY = int.Parse(splitData[1]);

                    //sorts b/t collideable and non colliudeable tiles
                    if (splitData.Length == 3) // this indicates that this is a background tile
                    {
                        //tile is placed in 2D tileList array at correct placement
                        Tile myTile = new Tile(
                            spriteSheet,
                            new Rectangle //drawn rectangle
                            (currentColumn * tileWidth, 
                            (currentRow * tileHeight)+75, //100 is offset from top
                            tileWidth, tileHeight),
                            new Rectangle //rectangle taken from sprite sheet
                            (upperLeftX, upperLeftY,
                            sheetTileWidth, sheetTileHeight),
                            spriteBatch);

                        //adds to array
                        tileList[currentColumn, currentRow] = myTile;
                    }
                    else //this is NOT a background tile and needs collision
                    {
                        CollisionTile myTile = new CollisionTile(
                            spriteSheet,
                            new Rectangle //drawn rectangle
                            (currentColumn * tileWidth,
                            (currentRow * tileHeight)+75, //100 is offset from top
                            tileWidth, tileHeight),
                            new Rectangle //rectangle taken from sprite sheet
                            (upperLeftX, upperLeftY,
                            sheetTileWidth, sheetTileHeight),
                            spriteBatch);

                        //adds to array AND collision list for collision detection
                        tileList[currentColumn, currentRow] = myTile;
                        collideableTiles.Add(myTile);
                    }

                    //resets column counter if it gets to the end of a row
                    if (currentColumn == tilesetColumns-1)
                    {
                        currentColumn = 0;
                        currentRow++;
                    }
                    else
                    {
                        currentColumn++;
                    }
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

        /// <summary>
        /// checks collision against player
        /// </summary>
        /// <param name="player"></param>
        public void CollisionCheck(Player player)
        {
            //uses a collisiontile list for simplicity
            //also that way you dont have to go through
            //every single tile~
            foreach (CollisionTile tile in collideableTiles)
            {
                player.DetectCollision(tile);
            }
        }
    }
}
