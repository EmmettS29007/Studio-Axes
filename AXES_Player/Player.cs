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
        private CollisionTypes yTopCollision;
        private CollisionTypes yBottomCollision;
        private bool canJump;
        private bool yesFloor;
        private int collisionChange;
        private int xSpeed;

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
            xSpeed = 8;
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
            } else //If else: 
            {
                //Checks if the player is moving down
                if (currentYSpeed >= 0)
                {
                    //if it is: it's falling and landing on a floor
                    currentYSpeed = 0;
                }
                else
                {
                    //if not: it's moving up and/or jumping so it should not be able to double jump
                    canJump = false;
                }
                
            }

            //If collides with the top: stop and move the other way
            if(yTopCollision == CollisionTypes.Ceiling)
            {
                currentYSpeed = 4;
            }

            //If D & Not colliding with a right wall, move right
            if (keyboard.IsKeyDown(Keys.D))
            { destination.X += xSpeed;}

            //If A & Not colliding with a left wall, move left
            if (keyboard.IsKeyDown(Keys.A))
            { destination.X -= xSpeed;}

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

            if (destination.Intersects(otherObject.Position))
            {
                if (bottom.Intersects(otherObject.Position))
                {
                    yesFloor = true;
                }

                int yPush = 0;
                int xPush = 0;

                //Gains an overlap
                Rectangle overlap = Rectangle.Intersect(destination, otherObject.Position);
                if (overlap.Width >= overlap.Height) //Height overlap
                {
                    yPush = otherObject.Position.Y - destination.Y;
                    if (yPush < 0) //if it overlaps upwards
                    {
                        yPush = overlap.Height;
                        yTopCollision = CollisionTypes.Ceiling;
                    }
                    else if (yPush > 0) //if it overlaps downwards
                    {
                        yPush = -overlap.Height + 1; 
                        yBottomCollision = CollisionTypes.Floor;
                        canJump = true;
                    }        
                }
                else //Side overlap
                {
                    xPush = otherObject.Position.X - destination.X;
                    if (xPush < 0) {
                        xPush = overlap.Width;
                    } else if (xPush > 0) {
                        xPush = -overlap.Width;
                    }
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
            yBottomCollision = CollisionTypes.None;
            collisionChange = 0;
            yesFloor = false;
            xSpeed = 8;
        }
    }
}
