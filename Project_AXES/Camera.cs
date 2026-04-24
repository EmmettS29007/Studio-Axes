using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project_AXES
{
    internal class Camera
    {
        //The player the camera follows
        private Player player;

        //Entities
        private List<Enemy> enemies;
        private Enemy milk;
        private NPC npc;

        //The List of Tiles and #Loaded
        private Tile[,] tile;
        private int loaded;

        //The Camera's Rectangle and Width/Height along with it's border
        private Rectangle cameraRect;
        private int width;
        private int height;
        private int leftBorder;
        private int rightBorder;
        private int bottomBorder;

        /// <summary>
        /// Constructor for the Camera
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="tile">The list of tiles</param>
        /// <param name="width">The width of the screen</param>
        /// <param name="height">the height of the screen</param>
        public Camera(Player player, List<Enemy> enemies, Enemy milk, NPC npc, Tile[,] tile, int width, int height)
        {
            this.player = player;
            this.enemies = enemies;
            this.milk = milk;
            this.npc = npc;
            this.tile = tile;
            this.width = width;
            this.height = height;
            cameraRect = new Rectangle(0, 0, width, height);
            loaded = 1;
            GetCameraBorder();
        }

        /// <summary>
        /// A get/set method for the Camera's rectangle
        /// </summary>
        private Rectangle CameraRect { get { return cameraRect; } set { cameraRect = value; } }

        /// <summary>
        /// An update method that moves all objects properly with the camera
        /// </summary>
        public void Update()
        {

            //Gets the player's left position and moves it with the camera's border
            if (player.Position.X <= cameraRect.X + width / 4 && leftBorder < 1)
            {
                foreach (Tile tiles in tile)
                {
                    tiles.Push(8, 0);
                }
                leftBorder += 8;
                rightBorder += 8;
                player.Push(8, 0);

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Push(8, 0);
                }

                milk.Push(8, 0);
                npc.X += 8; //moves npc out of way as well
            }

            //Gets the player's right position and moves it with the camera's border
            if (player.Position.X + player.Position.Width >= cameraRect.X + width - width / 4 && rightBorder > cameraRect.Width)
            {
                foreach (Tile tiles in tile)
                {
                    tiles.Push(-8, 0);
                }
                leftBorder -= 8;
                rightBorder -= 8;
                player.Push(-8, 0);

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Push(-8, 0);
                }

                milk.Push(-8, 0);
                npc.X -= 8;  //moves npc out of way as well
            }

            //Bottom Border
            if (bottomBorder < cameraRect.Height)
            {
                System.Diagnostics.Debug.WriteLine(bottomBorder + " " + cameraRect.Height);
                foreach (Tile tiles in tile)
                {
                    tiles.Push(0, 8);
                }
                bottomBorder += 8;
                player.Push(0, 8);
                npc.Push(0, 8);
            }


            //Check all Tiles
            CheckInCamera();
            System.Diagnostics.Debug.WriteLine(loaded + " objects loaded");
            loaded = 1;
        }

        /// <summary>
        /// This method looks at all tiles, finds the tiles that are in the camera, and loads them
        /// </summary>
        private void CheckInCamera()
        {
            foreach (Tile tiles in tile)
            {
                //If the tiles are intersecting with the camera
                if (tiles.Position.Intersects(cameraRect))
                {
                    //Then they are in the camera, and are 'loaded'
                    tiles.InCamera = true;
                    loaded += 1;
                }
                else
                {
                    tiles.InCamera = false;
                }
            }
        }


        /// <summary>
        /// This method gets the list of tiles and finds out the furthest tile on either side
        /// making sure that the camera doesn't move past the out-of-bounds window
        /// </summary>
        private void GetCameraBorder()
        {
            leftBorder = int.MaxValue;
            rightBorder = int.MinValue;
            bottomBorder = int.MinValue;
            foreach (Tile tiles in tile)
            {
                if(tiles.Position.X < leftBorder)
                {
                    leftBorder = tiles.Position.X;
                }
                if(tiles.Position.X + tiles.Position.Width > rightBorder)
                {
                    rightBorder = tiles.Position.X + tiles.Position.Width;
                }
                if(tiles.Position.Y + tiles.Position.Height > bottomBorder)
                {
                    bottomBorder = tiles.Position.Y + tiles.Position.Height;
                }
            }
        }

    }
}