using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace AXES_Player
{
    internal class Player
    {

        private Texture2D playerTexture;
        private int health;
        private Vector2 position;
        private Rectangle destination;
        private Rectangle spriteRectangle;
        private KeyboardState keyboard;
        private float gravity;
        private float maxYSpeed;
        private float currentYSpeed;
        private Color myColor;
        private bool canJump;

        /// <summary>
        /// The Constructor for the player
        /// </summary>
        /// <param name="playerTexture">The texture of the player</param>
        /// <param name="health">The player's health</param>
        /// <param name="position">The location of the player</param>
        public Player(Texture2D playerTexture, int health, Vector2 position)
        {
            this.playerTexture = playerTexture;
            this.health = health;
            this.position = position;
            destination = new Rectangle((int)position.X, (int)position.Y - 400, 140, 200);
            spriteRectangle = new Rectangle(44 * 17 + 14, playerTexture.Height - 40, 28, 40);

            gravity = .75f;
            currentYSpeed = 0;
            maxYSpeed = 24;
            myColor = Color.Black;
            canJump = true;
        }

        /// <summary>
        /// Updates the player
        /// </summary>
        public void Update()
        {
            keyboard = Keyboard.GetState();
            Movement();
        }

        /// <summary>
        /// Draws the player
        /// </summary>
        /// <param name="sb">The spritebatch</param>
        public void Draw(SpriteBatch sb)
        {

            sb.Draw(playerTexture, destination, spriteRectangle, Color.White);
            DebugLib.DrawRectOutline(sb, destination, 3, myColor);

        }

        /// <summary>
        /// Tests for movement
        /// </summary>
        public void Movement()
        {
            if (keyboard.IsKeyDown(Keys.D)) { destination.X += 8; }
            if (keyboard.IsKeyDown(Keys.A)) { destination.X -= 8; }
            if (keyboard.IsKeyDown(Keys.S)) { destination.Y += 8; }
            if (keyboard.IsKeyDown(Keys.W)) { currentYSpeed = -16; }
            destination.Y += (int)currentYSpeed;
        }

        public void CheckCollision(Tile otherObject)
        {
            if (destination.Intersects(otherObject.Rect))
            {
                destination.Y = otherObject.Rect.Y - destination.Height;
                currentYSpeed = 0;
                myColor = Color.White;
            }
            else
            {
                if (currentYSpeed + gravity < maxYSpeed)
                {
                    currentYSpeed += gravity;
                }
                myColor = Color.Black;
            }
        }





    }
}
