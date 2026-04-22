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
        private Texture2D enemySprite;

        // Idk the verdict on portrait
        // Texture2D portrait;


        // Field
        private int health;
        private Vector2 position;
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

        public float X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        
        public bool isDead
        {
            get { return dead; }
        }

        public Enemy(Texture2D enemySprite, int health, Vector2 position, float range)
        {
            this.enemySprite = enemySprite;
            this.health = health;
            this.position = position;
            this.range = range;
        }

        public void Attack(Player player)
        {
            player.Health--;
        }

        public void DetectCollision(ICollidable other) { }

        public void Push(int x, int y)
        {
            this.position.X += x;
            this.position.Y += y;
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
            dead = true;
        }
    }
}
