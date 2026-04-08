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

namespace Enemy_EntityManager_HUD
{
    public class Enemy
    {
        private Texture2D enemySprite;

        // Idk the verdict on portrait
        // Texture2D portrait;

        private int health;
        private Vector2 position;
        private bool moving;
        private float range;

        public Texture2D getSprite
        {
            get {  return enemySprite; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
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

        public Enemy(Texture2D enemySprite, int health, Vector2 position, float range)
        {
            this.enemySprite = enemySprite;
            this.health = health;
            this.position = position;
            this.range = range;
        }



    }
}
