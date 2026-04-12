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

        private int health;
        private Vector2 position;
        private bool moving;
        private float range;
        private bool dead;

        public Texture2D getSprite
        {
            get {  return enemySprite; }
        }

        public Rectangle Position
        {
            get { return new Rectangle(
                (int)position.X,
                (int)position.Y,
                72,
                78); }
        }

        public float Range
        {
            get { return range; }
        }

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

        public void Push(int x, int y) { }

        public void TakeDamage(int damage)
        {
            health --;
        }

        public void Die()
        {
            dead = true;
        }
    }
}
