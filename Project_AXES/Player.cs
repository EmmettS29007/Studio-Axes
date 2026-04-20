//using DamageableInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project_AXES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Project_AXES
{
    enum CollisionTypes
    {
        Floor,
        LeftWall,
        RightWall,
        Ceiling,
        None,
    }

    /// <summary>
    /// Enumeration utilized for movement and attacking
    /// </summary>
    enum PlayerState
    {
        IdleLeft,
        IdleRight,
        FacingLeft,
        FacingRight,
        JumpLeft,
        JumpRight
    }
    public class Player : ICollidable , IDamageable
    {
        //Sprites/Textures
        private Texture2D playerTexture;
        private Texture2D playerSpriteSheet;
        private Rectangle spriteRectangle;

        //Position Data/Movement
        private Vector2 position;
        private Rectangle destination;
        private float currentYSpeed;
        private CollisionTypes yTopCollision;
        private CollisionTypes yBottomCollision;
        private bool canJump;
        private bool yesFloor;
        private int collisionChange;
        private int xSpeed;
        private PlayerState playerState;

        // Attacking data
        private Rectangle attack;
        private double attackDuration;

        // Time used for Attacking
        private double timerCurrent;

        //Player Input
        private KeyboardState keyboard;
        private KeyboardState previousKeyboard;
        private float gravity;
        private float maxYSpeed;
        
        //Basic Data
        private Color myColor;
        private int health;
        private Color playerColor;

        //Animation Fields
        private int playerCurrentFrame;
        private int widthOfSingleSprite = 90;


        /// <summary>
        /// The Constructor for the player
        /// </summary>
        /// <param name="playerTexture">The texture of the player</param>
        /// <param name="health">The player's health</param>
        /// <param name="position">The location of the player</param>
        public Player(Texture2D playerSpriteSheet, Texture2D playerTexture, int health, Vector2 position)
        {
            this.playerTexture = playerTexture;
            this.playerSpriteSheet = playerSpriteSheet;
            this.health = health;
            this.position = new Vector2(32,64);
            destination = new Rectangle((int)position.X, (int)position.Y - 400, 110, 150);
            spriteRectangle = new Rectangle(44 * 17 + 14, playerTexture.Height - 36, 28, 36);
            gravity = .75f;
            currentYSpeed = 0;
            maxYSpeed = 24;
            myColor = Color.Red;
            canJump = false;
            yesFloor = false;
            xSpeed = 8;
            playerColor = Color.White;
            attackDuration = 0.1;
        }

        public int Health { get { return health; } set { health = value; } }

        /// <summary>
        /// The 
        /// </summary>
        public Rectangle Attack { get { return attack; } }

        /// <summary>
        /// Updates the player
        /// </summary>
        public void Update(GameTime gt)
        {
            keyboard = Keyboard.GetState();
            Movement();
            Attacking(gt);
            previousKeyboard = keyboard;
        }

        /// <summary>
        /// Draws the player
        /// </summary>
        /// <param name="sb">The spritebatch</param>
        public void Draw(SpriteBatch sb)
        {
            DrawPlayerWalking(sb,SpriteEffects.None);
            DebugLib.DrawRectOutline(sb, destination, 3, myColor);
            DebugLib.DrawRectOutline(sb, attack, 3, myColor);
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
            if (yBottomCollision == CollisionTypes.None)
            {
                if (!yesFloor) { canJump = false; }
                if (currentYSpeed + gravity < maxYSpeed && !canJump)
                {
                    currentYSpeed += gravity;
                }
            }
            else //If else: 
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
            if (yTopCollision == CollisionTypes.Ceiling)
            {
                currentYSpeed = 4;
            }

            //If D & Not colliding with a right wall, move right
            if (keyboard.IsKeyDown(Keys.D))
            {
                playerState = PlayerState.FacingRight;
                destination.X += xSpeed; 
            }

            // If the previous key down was D, the charachter idles to the right
            if (previousKeyboard.IsKeyDown(Keys.D))
            {
                playerState = PlayerState.IdleRight;
            }

            // If the previous key down was A, the character idles left.
            if (previousKeyboard.IsKeyDown(Keys.A))
            {
                playerState = PlayerState.IdleLeft;
            }
            //If A & Not colliding with a left wall, move left
            if (keyboard.IsKeyDown(Keys.A))
            {
                playerState = PlayerState.FacingLeft;
                destination.X -= xSpeed; 
            }

            //If W & can jump, Jump         
            if (keyboard.IsKeyDown(Keys.W) && canJump)
            {
                if (keyboard.IsKeyDown(Keys.D))
                {
                    playerState = PlayerState.JumpLeft;
                }
                else if (keyboard.IsKeyDown(Keys.A))
                {
                    playerState = PlayerState.JumpLeft;
                }
                currentYSpeed = -24; 
                canJump = false; 
            }
            destination.Y += (int)currentYSpeed;

            if (keyboard.IsKeyDown(Keys.V) && previousKeyboard.IsKeyUp(Keys.V))
            {
                this.TakeDamage(1);
            }
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
                    if (xPush < 0)
                    {
                        xPush = overlap.Width;
                    }
                    else if (xPush > 0)
                    {
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

        /// <summary>
        /// Method that handles incoming "Death" logic.
        /// Ideally requires IDamageable objects to die
        /// if their health reaches below zero.
        /// </summary>
        public bool Die()
        {
            if (health >= 0)
            {
                playerColor = Color.Red;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Lowers health by the damage taken.
        /// </summary>
        /// <param name="damage">The amount of damage to be taken</param>
        public void TakeDamage(int damage)
        {
            Health -= damage;
            // If the object reaches zero health, it will die
            if (Health <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Spawns a rectangle infront of the player that deals damage
        /// </summary>
        public void Attacking(GameTime gt)
        {
            if ((keyboard.IsKeyDown(Keys.K) && previousKeyboard.IsKeyUp(Keys.K)))
            {
                timerCurrent = attackDuration;
                if ((playerState == PlayerState.IdleRight) || (playerState == PlayerState.FacingRight) || (playerState == PlayerState.JumpRight))
                {
                    attack = new Rectangle(destination.X + (destination.Width / 2), destination.Y, destination.Width, destination.Height);
                }
                else if ((playerState == PlayerState.IdleLeft) || (playerState == PlayerState.FacingLeft) || (playerState == PlayerState.JumpLeft))
                {
                    attack = new Rectangle(destination.X - (destination.Width / 2), destination.Y, destination.Width, destination.Height);
                }
            }
            timerCurrent -= gt.ElapsedGameTime.TotalSeconds;
            if (timerCurrent <= 0)
            {
                attack = new Rectangle(0, 0, 0, 0);
            }
        }

        //---ANIMATION---

        /// <summary>
        /// Draws animation
        /// </summary>
        /// <param name="flip">Should he be flipped horizontally or vertically?</param>
        private void DrawPlayerWalking(SpriteBatch sb, SpriteEffects flip)
        {
            // This version of draw can flip (mirror) the image horizontally or vertically,
            // depending on the method's SpriteEffects parameter.
            sb.Draw(
                playerSpriteSheet,                               // Whole sprite sheet
                position,                                       // Position of the sprite
                new Rectangle(                                  // Which portion of the sheet is drawn:
                    playerCurrentFrame * widthOfSingleSprite,   // - Left edge
                    0,                                          // - Top of sprite sheet
                    widthOfSingleSprite,                        // - Width 
                    playerTexture.Height),                      // - Height
                playerColor,                                    // Color
                0.0f,                                           // No rotation
                Vector2.Zero,                                   // Start origin at (0, 0) of sprite sheet 
                1.0f,                                           // Scale
                flip,                                           // Flip it horizontally or vertically?    
                0.0f);                                          // Layer depth
        }
    }
}
