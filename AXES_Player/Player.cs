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
    enum CollisionTypes
    {
        Floor,
        LeftWall,
        RightWall,
        Ceiling,
        None,
    }
    internal class Player : ICollidable
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
        private CollisionTypes xLeftCollision;
        private CollisionTypes yTopCollision;
        private CollisionTypes xRightCollision;
        private CollisionTypes yBottomCollision;
        private bool canJump;
        private int collisionChange;

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
            destination = new Rectangle((int)position.X, (int)position.Y - 400, 140, 180);
            spriteRectangle = new Rectangle(44 * 17 + 14, playerTexture.Height - 36, 28, 36);

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
        /// Gets the positioning of the rectangle
        /// </summary>
        public Rectangle Position { get { return destination; } }

        /// <summary>
        /// Tests for movement
        /// </summary>
        public void Movement()
        {
            //If BottomCollision is Nothing, Gravity
            if(yBottomCollision == CollisionTypes.None)
            {
                if (currentYSpeed + gravity < maxYSpeed)
                {
                    currentYSpeed += gravity;
                }
                canJump = false;
            } else //If else: it must be touching floor
            {
                canJump = true;
                currentYSpeed = 0;
            }

            //If D & Not colliding with a right wall, move right
            if (keyboard.IsKeyDown(Keys.D) && xRightCollision == CollisionTypes.None)  
            { destination.X += 8; }

            //If A & Not colliding with a left wall, move left
            if (keyboard.IsKeyDown(Keys.A) && xLeftCollision == CollisionTypes.None) 
            
            { destination.X -= 8; }

            //If W & can jump, Jump         
            if (keyboard.IsKeyDown(Keys.W) && canJump )
            { currentYSpeed = -32; }
            destination.Y += (int)currentYSpeed;
        }

        /// <summary>
        /// Detects any Collisions with another Collidable object
        /// </summary>
        /// <param name="other">The other collidable</param>
        /// <returns>Collisions vs not</returns>
        public void DetectCollision(ICollidable otherObject)
        {
            //New Bounding Box For Tile
            Rectangle newRect = new Rectangle(
                otherObject.Position.X -1,
                otherObject.Position.Y -1,
                otherObject.Position.Width +1,
                otherObject.Position.Height +1);

            //Anti-Corner X
            Rectangle middleRectX = new Rectangle(
                destination.X,
                destination.Y + destination.Height / 5,
                destination.Width,
                destination.Height / 5 * 3);
            //Anti-Corner Y
            Rectangle middleRectY = new Rectangle(
                destination.X + destination.Width / 10,
                destination.Y,
                destination.Width / 10 * 8,
                destination.Height);
            
            //If it intersects
            if (destination.Intersects(newRect))
            {
                //Player's Bottom Y is further downscreen than the object's Top
                if (destination.Y + destination.Height >= newRect.Y &&
                    //Player's Top is Higher than the object's top
                    destination.Y <= newRect.Y &&
                    //Player's Left Side is Left of the object's right side
                    destination.X <= newRect.X + newRect.Width - 6 &&
                    //Player's Right side is Right of the object's left side
                    destination.X + destination.Width >= newRect.X + 6 &&
                    !middleRectX.Intersects(newRect))
                {
                    Push(0, (otherObject.Position.Y - destination.Y - destination.Height));
                    
                    yBottomCollision = CollisionTypes.Floor;
                }

                //Player's Top is Lower than Object's Top
                if(destination.Y >= newRect.Y &&
                //Player's Left Side is Left of the object's right side
                destination.X <= newRect.X + newRect.Width - 6 &&
                //Player's Right side is Right of the object's left side
                destination.X + destination.Width >= newRect.X + 6 &&
                !middleRectX.Intersects(newRect))
                {
                    Push(0, 24);
                    currentYSpeed = 4;
                    yTopCollision = CollisionTypes.Ceiling;
                }

                //Player's Left Side is Right of the Object's Left Side
                if (destination.X >= otherObject.Position.X &&
                //Player's Top is Higher than Other Object's Bottom
                destination.Y <= newRect.Y + newRect.Height &&
                //Player's Bottom is Lower than Other Object's Top
                destination.Y + destination.Height >= newRect.Y &&
                !middleRectY.Intersects(newRect))
                {
                    //Push(otherObject.Position.X + otherObject.Position.Width - destination.X - 1, 0);
                    xLeftCollision = CollisionTypes.LeftWall;
                }

                //Player's Right Side is Left of the Object's Right Side
                if (destination.X + destination.Width <= otherObject.Position.X + 
                    otherObject.Position.Width &&
                //Player's Top is Higher than Other Object's Bottom
                destination.Y <= newRect.Y + newRect.Height &&
                //Player's Bottom is Lower than Other Object's Top
                destination.Y + destination.Height >= newRect.Y &&
                !middleRectY.Intersects(newRect))
                {
                    xRightCollision = CollisionTypes.RightWall;
                }

            }
        }

        /// <summary>
        /// The amount that the object moves due to the collision
        /// </summary>
        /// <param name="xAmt">The amount on the X axis</param>
        /// <param name="yAmt">The amount on the Y axis</param>
        public void Push(int xAmt, int yAmt)
        {
            destination.X += xAmt;
            destination.Y += yAmt;
        }

        /// <summary>
        /// Resets all collision pieces before each run
        /// </summary>
        public void PreCollision()
        {
            yTopCollision = CollisionTypes.None;
            xLeftCollision = CollisionTypes.None;
            yBottomCollision = CollisionTypes.None;
            xRightCollision = CollisionTypes.None;
            collisionChange = 0;
        }
    }
}
