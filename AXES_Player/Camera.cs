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
        private List<Player> player;
        private List<Tile> tile;
        private Rectangle cameraRect;
        private int width;
        private int height;
        public Camera(List<Player> player, List<Tile> tile,int width, int height)
        {
            this.player = player;
            this.tile = tile;
            this.width = width;
            this.height = height;
            cameraRect = new Rectangle(0, 0, width, height);
        }

        public Rectangle CameraRect { get { return cameraRect; } set { cameraRect = value; } }

        public void Update()
        {
            if (player[0].Position.X <= cameraRect.X + width / 4)
            {
                foreach(Tile tiles in tile)
                {
                    tiles.Push(8, 0);
                }
                player[0].Push(8, 0);
            }

            if (player[0].Position.X + player[0].Position.Width >= cameraRect.X + width - width/4)
            {
                foreach (Tile tiles in tile)
                {
                    tiles.Push(-8, 0);
                }
                player[0].Push(-8, 0);

            }
        }
    }
}
