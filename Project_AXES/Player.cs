//using DamageableInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
    enum PlayerStateMovement
    {
        IdleLeft,
        IdleRight,
        FacingLeft,
        FacingRight,
        JumpLeft,
        JumpRight,
    }

    /// <summary>
    /// Enumeration for states other than movement
    /// Used so the two player states can exist in conjunction
    /// </summary>
    enum PlayerStateEffects
    {
        None,
        TakeDamage,
        Attack,
        Dead,
    }
    public class Player : ICollidable, IDamageable
    {
        // Players maximum health
        const int MaxHealth = 3;
        //Sprites/Textures
        private Texture2D playerSpriteSheet;
        private int widthOfSingleSprite = 128;
        private int heightOfSingleSprite = 60;

        //Position Data/Movement
        private Rectangle destination;
        private float currentYSpeed;
        private CollisionTypes yTopCollision;
        private CollisionTypes yBottomCollision;
        private bool canJump;
        private bool yesFloor;
        private int xSpeed;
        private PlayerStateMovement playerStateMovement;
        private PlayerStateEffects playerStateEffects;
        private bool sideCollision;
        private Rectangle playerMidline;

        // Attacking data
        private Rectangle attack;
        private double attackDuration;
        private SoundEffect hit;

        // Time used for Attacking
        private double timerCurrent;

        //Player Input
        private KeyboardState keyboard;
        private KeyboardState previousKeyboard;
        private float gravity;
        private float maxYSpeed;

        //God Mode/Debug
        private bool godMode;
        private int prevHealth;

        //Basic Data
        private Color myColor;
        private int health;
        private Color playerColor;

        //Animation Fields
        private int playerAnimation = 0; //type of cycle playing--i.e run, jump
        private int playerFrame = 0; //frame in said cycle
        private int cycleFrameTotal = 11; //frame total in the cycle
        private double timeCounter = 0; //for frame switching
        private double otherTimeCounter = 0; //for checking animations end properly
        private double secondsPerFrame = .1;
        private bool toFlip = false;
        PlayerStateMovement prevPlayerStateMovement;
        PlayerStateEffects prevPlayerStateEffects;

        /// <summary>
        /// The Constructor for the player
        /// </summary>
        /// <param name="playerTexture">The texture of the player</param>
        /// <param name="health">The player's health</param>
        /// <param name="position">The location of the player</param>
        public Player(Texture2D playerSpriteSheet, Texture2D playerTexture, int health, Vector2 position, SoundEffect hit)
        {
            this.playerSpriteSheet = playerSpriteSheet;
            this.health = health;
            destination = new Rectangle   //collision box
                ((int)position.X,       //x
                (int)position.Y - 400,  //y
                heightOfSingleSprite,   //height
                widthOfSingleSprite);   //width 
            playerMidline = new Rectangle //A line to allow for better collision with walls/floors
                (destination.X + 8,
                destination.Y,
                destination.Width - 16,
                destination.Height);
            gravity = .75f;
            currentYSpeed = 0;
            maxYSpeed = 24;
            myColor = Color.Red;
            canJump = false;
            yesFloor = false;
            xSpeed = 8;
            playerColor = Color.White;
            attackDuration = 0.25;
            this.hit = hit;
            sideCollision = false;
            godMode = false;
        }

        //Properties

        public int Health { get { return health; } set { health = value; } }

        public int Max { get { return MaxHealth; } }

        public Color PlayerColor { get { return playerColor; } set { playerColor = value; } }

        public Rectangle Attack { get { return attack; } }


        //****----------UPDATE/DRAW---------****

        /// <summary>
        /// Updates the player
        /// </summary>
        public void Update(GameTime gt)
        {
            keyboard = Keyboard.GetState();
            Movement();
            Attacking(gt);
            UpdateAnimation(gt);
            UpdateAnimationFrame(gt);
            previousKeyboard = keyboard;
        }

        /// <summary>
        /// Draws the player
        /// </summary>
        /// <param name="sb">The spritebatch</param>
        public void Draw(SpriteBatch sb, bool debug)
        {
            DrawPlayer(sb, SpriteEffects.None);
            if (debug)
            {
                DebugLib.DrawRectOutline(sb, destination, 3, myColor);
                DebugLib.DrawRectOutline(sb, attack, 3, myColor);
                DebugLib.DrawRectOutline(sb, playerMidline, 3, myColor);
            }
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
            //**NOTE-- movement has been intentionally layered for the smoothest animation appearance
            //if you change the order, please ensure that the animations are alright

            //If collides with the top: stop and move the other way
            if (yTopCollision == CollisionTypes.Ceiling)
            {
                currentYSpeed = 4;
            }

            //---IDLE---

            // If the previous key down was D, the charachter idles to the right
            if (previousKeyboard.IsKeyDown(Keys.D))
            {
                playerStateMovement = PlayerStateMovement.IdleRight;
            }
            // If the previous key down was A, the character idles left.
            else if (previousKeyboard.IsKeyDown(Keys.A))
            {
                playerStateMovement = PlayerStateMovement.IdleLeft;
            }

            //---LEFT AND RIGHT RUN---

            //If D & Not colliding with a right wall, move right
            if (keyboard.IsKeyDown(Keys.D))
            {
                playerStateMovement = PlayerStateMovement.FacingRight;
                destination.X += xSpeed;
            }
            //If A & Not colliding with a left wall, move left
            else if (keyboard.IsKeyDown(Keys.A))
            {
                playerStateMovement = PlayerStateMovement.FacingLeft;
                destination.X -= xSpeed;
            }

            //---JUMP---

            //If W & can jump, Jump         
            if (keyboard.IsKeyDown(Keys.W) && canJump)
            {
                if (keyboard.IsKeyDown(Keys.D))
                {
                    playerStateMovement = PlayerStateMovement.JumpRight;
                }
                else if (keyboard.IsKeyDown(Keys.A))
                {
                    playerStateMovement = PlayerStateMovement.JumpLeft;
                }
                currentYSpeed = -24;
                canJump = false;
            }
            destination.Y += (int)currentYSpeed;

            //---FALLING---

            //If BottomCollision is Nothing, Gravity
            if (yBottomCollision == CollisionTypes.None)
            {
                if (!yesFloor) { canJump = false; }
                if (currentYSpeed + gravity < maxYSpeed && !canJump)
                {
                    currentYSpeed += gravity;

                    //--animation--
                    // If the previous key down was D, the charachter jumps to the right
                    if (previousKeyboard.IsKeyDown(Keys.D))
                    {
                        playerStateMovement = PlayerStateMovement.JumpRight;
                    }
                    // If the previous key down was A, the character jumps to the left.
                    else if (previousKeyboard.IsKeyDown(Keys.A))
                    {
                        playerStateMovement = PlayerStateMovement.JumpLeft;
                    }

                }
            }
            else //If else: 
            {
                //Checks if the player is moving down
                if (currentYSpeed >= 0)
                {
                    //if it is: landing on a floor
                    currentYSpeed = 0;

                    //--animation--
                    // If charatcer was jumping right, the charachter idles to the right
                    if (playerStateMovement == PlayerStateMovement.JumpRight)
                    {
                        playerStateMovement = PlayerStateMovement.IdleRight;
                    }
                    // If the previous key down was A, the character idles left.
                    else if (playerStateMovement == PlayerStateMovement.JumpLeft)
                    {
                        playerStateMovement = PlayerStateMovement.IdleLeft;
                    }
                }
                else
                {
                    //if not: it's moving up and/or jumping so it should not be able to double jump
                    canJump = false;
                }

            }

            //---DAMAGE DEBUG---

            if (keyboard.IsKeyDown(Keys.V) && previousKeyboard.IsKeyUp(Keys.V))
            {
                this.TakeDamage(1);
            }

            //---GODMODE---

            if (keyboard.IsKeyDown(Keys.G) && previousKeyboard.IsKeyUp(Keys.G))
            {
                godMode = !godMode;
                if (godMode)
                {
                    prevHealth = health; //preserves original health value
                    health = 1000;
                    
                }
                else if (!godMode)
                {
                    health = prevHealth;
                }
            }

            playerMidline.X = destination.X + 8;
            playerMidline.Y = destination.Y;
        }

        //******----------------COLLISIONS--------------------******

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
                if (overlap.Height >= overlap.Width) //Width overlap
                {
                    sideCollision = true;
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
                else //Height overlap
                {
                    if (playerMidline.Intersects(otherObject.Position))
                    //Check if it's truly intersecting with the floor/ceiling
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
            yesFloor = false;
            xSpeed = 8;
        }


        //*****------------DAMAGE AND WORLD INTERACTION------------------******

        /// <summary>
        /// Method that handles incoming "Death" logic.
        /// Ideally requires IDamageable objects to die
        /// if their health reaches below zero.
        /// </summary>
        public bool Die()
        {
            if (health <= 0)
            {
                playerColor = Color.Red;
                playerStateEffects = PlayerStateEffects.Dead;
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
            playerStateEffects = PlayerStateEffects.TakeDamage;
            playerFrame = 0; //resets frame so plays trhough full damage animation
            //resetting frame also resets it in case of death, which is nice

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
                hit.Play();
                timerCurrent = attackDuration;
                // If the player is facing right, attack to the right
                if ((playerStateMovement == PlayerStateMovement.IdleRight) ||
                    (playerStateMovement == PlayerStateMovement.FacingRight) ||
                    (playerStateMovement == PlayerStateMovement.JumpRight))
                {
                    attack = new Rectangle(destination.X + (destination.Width), destination.Y, destination.Width * 2, destination.Height);
                }
                // Else if the player is facing left, attack to the left
                else if ((playerStateMovement == PlayerStateMovement.IdleLeft) ||
                    (playerStateMovement == PlayerStateMovement.FacingLeft) || (
                    playerStateMovement == PlayerStateMovement.JumpLeft))
                {
                    attack = new Rectangle(destination.X - (destination.Width * 2), destination.Y, destination.Width * 2, destination.Height);
                }
                playerStateEffects = PlayerStateEffects.Attack;
                otherTimeCounter = 0; //resets time counter 

            }
            timerCurrent -= gt.ElapsedGameTime.TotalSeconds;
            if (timerCurrent <= 0)
            {
                attack = new Rectangle(0, 0, 0, 0);
            }
        }

        //*****--------------------ANIMATION-------------------****

        /// <summary>
        /// Draws animation
        /// </summary>
        /// <param name="flip">Should he be flipped horizontally or vertically?</param>
        private void DrawPlayer(SpriteBatch sb, SpriteEffects flip)
        {
            //checks if we need to flip the sprite
            if (toFlip) { flip = SpriteEffects.FlipHorizontally; }
            else { flip = SpriteEffects.None; }

            //draws the sprite
            sb.Draw(
                playerSpriteSheet,                              // Whole sprite sheet
                new Vector2(Position.X - 125, Position.Y - 20),      // Position of the sprite
                new Rectangle(                                  // Which portion of the sheet is drawn:
                    playerFrame * widthOfSingleSprite,          // - Left edge
                    playerAnimation * heightOfSingleSprite,     // - Top of sprite sheet
                    widthOfSingleSprite,                        // - Width 
                    heightOfSingleSprite),                      // - Height
                playerColor,                                    // Color
                0.0f,                                           // No rotation
                Vector2.Zero,                                   // Start origin at (0, 0) of sprite sheet 
                2.5f,                                           // Scale
                flip,                                           // Flip it horizontally or vertically?    
                0.0f);                                          // Layer depth
        }

        /// <summary>
        /// Updates animation based on player states
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateAnimation(GameTime gameTime)
        {
            //This is intentionally layered so that the effects display will always
            //take priority over movement display--but both can exit simultaneously 

            otherTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            //cycles through movement animations
            switch (playerStateMovement)
            {
                case PlayerStateMovement.IdleLeft:
                    toFlip = true;
                    cycleFrameTotal = 5;
                    playerAnimation = 4;
                    break;
                case PlayerStateMovement.IdleRight:
                    toFlip = false;
                    cycleFrameTotal = 5;
                    playerAnimation = 4;
                    break;
                case PlayerStateMovement.FacingLeft:
                    toFlip = true;
                    cycleFrameTotal = 8;
                    playerAnimation = 6;
                    break;
                case PlayerStateMovement.FacingRight:
                    toFlip = false;
                    cycleFrameTotal = 8;
                    playerAnimation = 6;
                    break;
                case PlayerStateMovement.JumpLeft:
                    toFlip = true;
                    cycleFrameTotal = 4;
                    playerAnimation = 2;
                    break;
                case PlayerStateMovement.JumpRight:
                    toFlip = false;
                    cycleFrameTotal = 4;
                    playerAnimation = 2;
                    break;
                default:
                    break;
            }

            //runs if the player has an effect
            //if no effect, plays the default movmement animations
            switch (playerStateEffects)
            {
                case PlayerStateEffects.TakeDamage:
                    cycleFrameTotal = 2;
                    playerAnimation = 3;
                    if (playerFrame == 1) //after animation plays, returns to none
                    {
                        playerStateEffects = PlayerStateEffects.None;
                    }
                    break;
                case PlayerStateEffects.Attack:
                    cycleFrameTotal = 10;
                    playerAnimation = 0;
                    if (otherTimeCounter>1) //after animation plays, returns to none
                    {
                        playerStateEffects = PlayerStateEffects.None;
                    }
                    break;
                case PlayerStateEffects.Dead:
                    cycleFrameTotal = 6;
                    playerAnimation = 1;
                    playerStateEffects = PlayerStateEffects.Dead; // can't stop bein dead
                    break;
                case PlayerStateEffects.None:
                    break;
                default:
                    break;
            }

            //checks if the state has changed. if it has,
            //resets frame so that the full animation will play
            //and so that it wont accidentally play a blank frame
            if ((playerStateMovement != prevPlayerStateMovement) || (playerStateEffects != prevPlayerStateEffects))
            {
                playerFrame = 0;
            }

            //establishes player state to check it after it changes next cycle
            prevPlayerStateMovement = playerStateMovement;
            prevPlayerStateEffects = playerStateEffects;
        }

        /// <summary>
        /// Updates frame 
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateAnimationFrame(GameTime gameTime)
        {
            //Code largely taken from mario pe

            // ElapsedGameTime is the duration of the last GAME frame
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            // Has enough time passed to flip to the next frame?
            if (timeCounter >= secondsPerFrame)
            {
                // Change which frame is active, ensuring the frame is reset back to the first 
                playerFrame++;

                if (playerStateEffects == PlayerStateEffects.Dead && playerFrame == 6)
                {
                    playerFrame--;
                }

                if (playerFrame >= cycleFrameTotal)
                {
                    playerFrame = 0;
                }


                // Reset the time counter, keeping remaining elapsed time
                timeCounter -= secondsPerFrame;
            }
        }

        /// <summary>
        /// This method simply resets the player back to it's original starting position
        /// </summary>
        public void Reset()
        {
            destination.Y = -100;
            destination.X = 0;
        }
    }
}
