using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AXES_Player
{
    internal class Camera
    {
        private Player player;
        private List<Tile> tile;
        private Rectangle cameraRect;
        private int width;
        private int height;
        private int loaded;
        public Camera(Player player, List<Tile> tile,int width, int height)
        {
            this.player = player;
            this.tile = tile;
            this.width = width;
            this.height = height;
            cameraRect = new Rectangle(0, 0, width, height);
            loaded = 1;
        }

        public Rectangle CameraRect { get { return cameraRect; } set { cameraRect = value; } }

        public void Update()
        {
            
            if (player.Position.X <= cameraRect.X + width / 4)
            {
                foreach(Tile tiles in tile)
                {
                    tiles.Push(8, 0);
                }
                player.Push(8, 0);
            }

            if (player.Position.X + player.Position.Width >= cameraRect.X + width - width/4)
            {
                foreach (Tile tiles in tile)
                {
                    tiles.Push(-8, 0);
                }
                player.Push(-8, 0);

            }
            CheckInCamera();
            System.Diagnostics.Debug.WriteLine(loaded + " objects loaded");
            loaded = 1;
        }

        public void CheckInCamera()
        {
            foreach(Tile tiles in tile)
            {
                if (tiles.Position.Intersects(cameraRect))
                {
                    tiles.InCamera = true;
                    loaded += 1;
                } else
                {
                    tiles.InCamera = false;
                }
            }
        }

    }
}
