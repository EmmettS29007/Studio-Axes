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
        private bool yesFloor;
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
            canJump = false;
            yesFloor = false;
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
                if (!yesFloor) { canJump = false; }
                if (currentYSpeed + gravity < maxYSpeed && !canJump)
                {
                    currentYSpeed += gravity;
                }
            } else //If else: it must be touching floor
            {
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
            { currentYSpeed = -32; canJump = false; }
            destination.Y += (int)currentYSpeed;
        }

        /// <summary>
        /// Detects any Collisions with another Collidable object
        /// </summary>
        /// <param name="other">The other collidable</param>
        /// <returns>Collisions vs not</returns>
        public void DetectCollision(ICollidable otherObject)
        {
            Rectangle bottom = new Rectangle(
                destination.X,
                destination.Y + destination.Height - 2,
                destination.Width,
                10);

            /*

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

            bool floor = false;
            bool wall = false;

            //If it intersects
            if (bottom.Intersects(otherObject.Position))
            {
                yesFloor = true;
            }

            
            if (destination.Intersects(otherObject.Position))
            {
                
                //Player's Bottom Y is further downscreen than the object's Top
                if (destination.Y + destination.Height >= otherObject.Position.Y &&
                    //Player's Top is Higher than the object's top
                    destination.Y <= otherObject.Position.Y &&
                    //Player's Left Side is Left of the object's right side
                    destination.X <= otherObject.Position.X + otherObject.Position.Width - 6 &&
                    //Player's Right side is Right of the object's left side
                    destination.X + destination.Width >= otherObject.Position.X + 6 &&
                    !middleRectX.Intersects(otherObject.Position))
                {
                    if(canJump == false)
                    {
                        Push(0, (otherObject.Position.Y - destination.Y - destination.Height));
                        canJump = true;
                    }
                    
                    yBottomCollision = CollisionTypes.Floor;
                    floor = true;
                }

                //Player's Left Side is Right of the Object's Left Side
                if (destination.X >= otherObject.Position.X &&
                //Player's Top is Higher than Other Object's Bottom
                destination.Y <= otherObject.Position.Y + otherObject.Position.Height &&
                //Player's Bottom is Lower than Other Object's Top
                destination.Y + destination.Height >= otherObject.Position.Y &&
                !floor &&
                !middleRectY.Intersects(otherObject.Position))
                {
                    //Push(otherObject.Position.X + otherObject.Position.Width - destination.X - 1, 0);
                    xLeftCollision = CollisionTypes.LeftWall;
                    wall = true;
                }

                //Player's Right Side is Left of the Object's Right Side
                if (destination.X + destination.Width <= otherObject.Position.X + 
                    otherObject.Position.Width &&
                //Player's Top is Higher than Other Object's Bottom
                destination.Y <= otherObject.Position.Y + otherObject.Position.Height &&
                //Player's Bottom is Lower than Other Object's Top
                destination.Y + destination.Height >= otherObject.Position.Y &&
                !floor&&
                !middleRectY.Intersects(otherObject.Position))
                {
                    xRightCollision = CollisionTypes.RightWall;
                    wall = true;
                }

                //Player's Top is Lower than Object's Top
                if (destination.Y >= otherObject.Position.Y &&
                //Player's Left Side is Left of the object's right side
                destination.X <= otherObject.Position.X + otherObject.Position.Width - 6 &&
                //Player's Right side is Right of the object's left side
                destination.X + destination.Width >= otherObject.Position.X + 6 &&
                !wall &&
                !middleRectX.Intersects(otherObject.Position))
                {
                    Push(0, 24);
                    currentYSpeed = 4;
                    yTopCollision = CollisionTypes.Ceiling;
                }

            }*/

            if (destination.Intersects(otherObject.Position))
            {
                if (bottom.Intersects(otherObject.Position))
                {
                    yesFloor = true;
                }

                int yPush = 0;
                int xPush = 0;
                Rectangle overlap = Rectangle.Intersect(destination, otherObject.Position);
                if (overlap.Width >= overlap.Height)
                {
                    yPush = otherObject.Position.Y - destination.Y;
                    if (yPush < 0) 
                    { 
                        yPush = overlap.Height;
                        Push(0, 4);
                        yTopCollision = CollisionTypes.Ceiling;
                    } 
                    else if (yPush > 0) 
                    {
                        yPush = -overlap.Height + 1;
                        yBottomCollision = CollisionTypes.Floor;
                        canJump = true;
                    }
                    
                    
                    
                }
                else
                {
                    xPush = otherObject.Position.X - destination.X;
                    if (xPush < 0) {
                        xPush = overlap.Width; xLeftCollision = CollisionTypes.LeftWall; 
                    } else if (xPush > 0) {
                        xPush = -overlap.Width; xRightCollision = CollisionTypes.RightWall; }
                }
                Push(xPush, yPush);
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
            yesFloor = false;
        }
    }
}
