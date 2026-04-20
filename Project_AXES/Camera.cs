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

        //The List of Tiles and #Loaded
        private Tile[,] tile;
        private int loaded;

        //List of Enemies
        private List<Enemy> enemy;

        //The Camera's Rectangle and Width/Height
        private Rectangle cameraRect;
        private int width;
        private int height;

        /// <summary>
        /// Constructor for the Camera
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="tile">The list of tiles</param>
        /// <param name="width">The width of the screen</param>
        /// <param name="height">the height of the screen</param>
        public Camera(Player player, Tile[,] tile, List<Enemy> enemy, int width, int height)
        {
            this.player = player;
            this.enemy = enemy;
            this.tile = tile;
            this.width = width;
            this.height = height;
            cameraRect = new Rectangle(0, 0, width, height);
            loaded = 1;
        }

        /// <summary>
        /// A get/set method for the Camera's rectangle
        /// </summary>
        public Rectangle CameraRect { get { return cameraRect; } set { cameraRect = value; } }

        /// <summary>
        /// An update method that moves all objects properly with the camera
        /// </summary>
        public void Update()
        {
            //Gets the player's left position and moves it and tiles with the camera's border
            if (player.Position.X <= cameraRect.X + width / 4)
            {
                foreach (Tile tiles in tile)
                {
                    tiles.Push(8, 0);
                }
                foreach (Enemy enemies in enemy)
                {
                    enemies.Push(8, 0);
                }

                player.Push(8, 0);
            }

            //Gets the player's right position and moves it and tiles with the camera's border
            if (player.Position.X + player.Position.Width >= cameraRect.X + width - width / 4)
            {
                foreach (Tile tiles in tile)
                {
                    tiles.Push(-8, 0);
                }
                foreach (Enemy enemies in enemy)
                {
                    enemies.Push(-8, 0);
                }
                player.Push(-8, 0);

            }



            //Check all Tiles
            CheckInCamera();
            System.Diagnostics.Debug.WriteLine(loaded + " objects loaded");
            loaded = 1;
        }

        /// <summary>
        /// This method looks at all tiles, finds the tiles that are in the camera, and loads them
        /// </summary>
        public void CheckInCamera()
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

    }
}