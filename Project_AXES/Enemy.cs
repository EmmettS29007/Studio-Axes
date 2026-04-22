using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace Project_AXES
{
    public class Enemy : ICollidable, IDamageable
    {
        // Field

        // Main enemy data
        private Texture2D enemySprite;
        private int health;
        private Vector2 position;

        // Variables for moving and drawing
        private bool moving;
        private float range;
        private bool dead;


        /// <summary>
        /// Returns the enemySprite
        /// </summary>
        public Texture2D getSprite
        {
            get {  return enemySprite; }
        }

        /// <summary>
        /// Returns a rectangle using the Position Vector and the set size of enemies
        /// </summary>
        public Rectangle Position
        {
            get { return new Rectangle(
                (int)position.X,
                (int)position.Y,
                72,
                78); }
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
        /// Allows for the X coordinate of the enemy to be accessed and changed
        /// </summary>
        public float X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        /// <summary>
        /// Allows for the enemy's health to be accessed and changed
        /// </summary>
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        
        /// <summary>
        /// Allows for the enemy's current state (alive or dead) to be accessed
        /// </summary>
        public bool isDead
        {
            get { return dead; }
        }

        /// <summary>
        /// Constructor for enemy
        /// </summary>
        /// <param name="enemySprite"> The sprite for the enemy </param>
        /// <param name="health"> The enemy's health </param>
        /// <param name="position"> The position of the enemy </param>
        /// <param name="range"> The range in which the enenmy will be walking </param>
        public Enemy(Texture2D enemySprite, int health, Vector2 position, float range)
        {
            this.enemySprite = enemySprite;
            this.health = health;
            this.position = position;
            this.range = range;
        }

        /// <summary>
        /// Attacks the player, causing the player to take damage, and die once their health hits 0
        /// </summary>
        /// <param name="player"></param>
        public void Attack(Player player)
        {
            if (player.Health > 0)
            {
                player.Health--;
            }

            else
            {
                player.Die();
            }
        }

        /// <summary>
        /// Allows for collision detection between 2 ICollidable classes
        /// </summary>
        /// <param name="other"> The other class to use to detect collision </param>
        public void DetectCollision(ICollidable other) { }


        /// <summary>
        /// Pushes the enemy a certain amount
        /// </summary>
        /// <param name="x"> The value to add to the enemy's x coordinate </param>
        /// <param name="y"> The value to add to the enemy's y coordinate </param>
        public void Push(int x, int y)
        {
            this.position.X += x;
            this.position.Y += y;
        }

        /// <summary>
        /// Makes the enemy take damage, when health hits 0, the enemy dies
        /// </summary>
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

        /// <summary>
        /// Once the enemy dies, change its state to dead.
        /// </summary>
        public void Die()
        {
            dead = true;
        }
    }
}
