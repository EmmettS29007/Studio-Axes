using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Project_AXES
{
    public class Enemy : ICollidable, IDamageable
    {
        private Texture2D enemySprite;

        // Field
        private int health;
        private Vector2 position;
        private Vector2 ogPosition;

        // Invincibility timers!
        private double enemyInvinTimer;
        private double enemyInvinDuration = 1;

        private bool moving;
        private float range;
        private float pushDistance = 0;
        private bool pushed;

        private bool dead;

        //Animation Fields
        private int enemyAnimation = 0; //type of cycle playing--1 is walk, 2 is die
        private int enemyFrame = 0; //frame in said cycle
        private int cycleFrameTotal = 10; //frame total in the cycle
        private double timeCounter = 0; //for frame switching
        private double secondsPerFrame = .1;
        private bool toFlip = false;

        // Colors
        private Color enemyColor;

        /// <summary>
        /// Returns the color of the enemy
        /// </summary>
        public Color EnemyColor 
        { 
            get { return enemyColor; } 
            set { enemyColor = value; } 
        }

        /// <summary>
        /// Returns the enemySprite
        /// </summary>
        public Texture2D getSprite
        {
            get { return enemySprite; }
        }

        /// <summary>
        /// Returns the enemy's invincibility timer 
        /// </summary>
        public double IFrames 
        { 
            get { return enemyInvinTimer; } 
            set { enemyInvinTimer = value; } 
        }

        /// <summary>
        /// Returns how long an enemy can stay invincibile for
        /// </summary>
        public double ImmunityTime
        {
            get { return enemyInvinDuration; }
        }

        /// <summary>
        /// Returns a rectangle using the Position Vector and the set size of enemies
        /// </summary>
        public Rectangle Position
        {
            get
            {
                return new Rectangle(
                (int)position.X,
                (int)position.Y,
                72,
                78);
            }
        }

        public float OGPosition
        {
            get { return ogPosition.X; }
            set { ogPosition.X = value; }
        }

        /// <summary>
        /// Returns range
        /// </summary>
        public float Range
        {
            get { return range; }
        }

        /// <summary>
        /// Allows for the current state of the enemy to change
        /// </summary>
        public bool Moving
        {
            get { return moving; }
            set { moving = value; }
        }

        /// <summary>
        /// Allows for the current state of the enemy to change
        /// </summary>
        public bool ToFlip
        {
            get { return toFlip; }
            set { toFlip = value; }
        }

        /// <summary>
        /// Gives get and set properties to the enemy's x coordinate
        /// </summary>
        public float X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        /// <summary>
        /// Gives get and set properties to the enemy's health
        /// </summary>
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        /// <summary>
        /// Allows for the enemy's current state (alive or dead) to be acquired
        /// </summary>
        public bool isDead
        {
            get { return dead; }
        }

        public Enemy(Texture2D enemySprite, int health, Vector2 position, float range)
        {
            this.enemySprite = enemySprite;
            this.health = health;
            this.position = position;
            this.ogPosition = position;
            this.range = range;
        }

        public void Attack(Player player)
        {
            //if the enemy isn't dead, deals damage
            if (!dead)
            {
                player.Health--;
            }
        }

        public void DetectCollision(ICollidable other) { }

        public void Push(int x, int y)
        {
            this.X += x;
            this.position.Y += y;
            ogPosition.X += x;
            ogPosition.Y += y;
        }


        public void TakeDamage()
        {
            if (health > 0)
            {
                health--;
            }

            else
            {
                Die();
            }
        }

        public void Die()
        {   
            //death animation
            enemyFrame = 0; //resets for full explosion
            enemyAnimation = 1;

            dead = true;
        }

        public void Reset()
        {
            dead = false;
            enemyAnimation = 0;
            position = ogPosition;
        }

        //*****--------------------ANIMATION-------------------****

        /// <summary>
        /// Draws animation
        /// </summary>
        /// <param name="flip">Should he be flipped horizontally or vertically?</param>
        public void DrawEnemy(SpriteBatch sb)
        {
            SpriteEffects flip;
            //checks if we need to flip the sprite
            if (toFlip) { flip = SpriteEffects.FlipHorizontally; }
            else { flip = SpriteEffects.None; }

            //draws the sprite
            sb.Draw(
                enemySprite,                                    // Whole sprite sheet
                new Vector2(Position.X, Position.Y),            // Position of the sprite
                new Rectangle(                                  // Which portion of the sheet is drawn:
                    enemyFrame * 64,                            // - Left edge
                    enemyAnimation * 64,        // - Top of sprite sheet
                    64,                                         // - Width 
                    64),                        // - Height
                enemyColor,                                    // Color
                0.0f,                                           // No rotation
                Vector2.Zero,                                   // Start origin at (0, 0) of sprite sheet 
                2.5f,                                           // Scale
                flip,                                           // Flip it horizontally or vertically?    
                0.0f);                                          // Layer depth
        }

        /// <summary>
        /// Updates frame 
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateEnemyFrame(GameTime gameTime)
        {
            //Code largely taken from mario pe

            // ElapsedGameTime is the duration of the last GAME frame
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            // Has enough time passed to flip to the next frame?
            if (timeCounter >= secondsPerFrame)
            {
                // Change which frame is active, ensuring the frame is reset back to the first 
                enemyFrame++;

                //sets up death
                if (isDead == true && enemyFrame >= cycleFrameTotal)
                {
                    enemyFrame = cycleFrameTotal; //freezes enemy in an invisible sprite
                }
                else if (enemyFrame >= cycleFrameTotal)
                {
                    enemyFrame = 0;
                }

                // Reset the time counter, keeping remaining elapsed time
                timeCounter -= secondsPerFrame;
            }
        }
    }
}
