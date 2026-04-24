using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project_Axes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_AXES
{
    public class NPC : ICollidable
    {
        //**---FIELDS---**
        private Texture2D npcSprite;
        private Rectangle position;
        private Color tint = Color.White;
        private int x = 900;
        private int y = 750;

        //Animation Fields
        private int npcAnimation = 0; //type of cycle playing--1 is walk, 2 is die
        private int npcFrame = 0; //frame in said cycle
        private int cycleFrameTotal = 4; //frame total in the cycle
        private double timeCounter = 0; //for frame switching
        private double secondsPerFrame = .1;

        //**---PROPERTIES---**
        /// <summary>
        /// Returns a rectangle using the Position Vector and the set size of npc
        /// </summary>
        public Rectangle Position
        {
            get
            {
                return new Rectangle(
                x, y,
                250, 1000);
                //big length and width for a big interact box
            }
        }

        //x and y properties so it can be moved in accordance with camera

        /// <summary>
        /// returns and changes x
        /// upper left corner of bounding box
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// returns and changes x
        /// upper left corner of bounding box
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// returns sprite
        /// </summary>
        public Texture2D getSprite
        {
            get { return npcSprite; } 
        }

        /// <summary>
        /// returns and changes color
        /// </summary>
        public Color Tint
        {
            get { return tint; }
            set { tint = value; }
        }

        //**---CTOR---***
        public NPC(Texture2D sprite, Rectangle position)
        {
            this.position = position;
            this.npcSprite = sprite;
        }

        //**---METHODS---**
        public bool Interact(Player player)
        {
            if (Position.Intersects(player.Position))
            {
                tint = Color.Orange;
                return true;
            }
            else
            {
                tint = Color.White;
                return false;
            }
        }

        //--COLLISION--
        public void DetectCollision(ICollidable other) { }
        public void Push(int x, int y){ }


        //*****--------------------ANIMATION-------------------****

        /// <summary>
        /// Draws animation
        /// </summary>
        /// <param name="flip">Should he be flipped horizontally or vertically?</param>
        public void DrawNPC(SpriteBatch sb)
        { 
            //draws the sprite
            sb.Draw(
                npcSprite,                                    // Whole sprite sheet
                new Vector2(Position.X, Position.Y),            // Position of the sprite
                new Rectangle(                                  // Which portion of the sheet is drawn:
                    npcFrame * 71,                            // - Left edge
                    npcAnimation * npcSprite.Height,        // - Top of sprite sheet
                    71,                                         // - Width 
                    npcSprite.Height),                          // - Height
                tint,                                           // Color
                0.0f,                                           // No rotation
                Vector2.Zero,                                   // Start origin at (0, 0) of sprite sheet 
                4f,                                             // Scale
                SpriteEffects.None,                             // Flip it horizontally or vertically?    
                0.0f);                                          // Layer depth
        }

        /// <summary>
        /// Updates frame 
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateNPCFrame(GameTime gameTime)
        {
            //Code largely taken from mario pe

            // ElapsedGameTime is the duration of the last GAME frame
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            // Has enough time passed to flip to the next frame?
            if (timeCounter >= secondsPerFrame)
            {
                // Change which frame is active, ensuring the frame is reset back to the first 
                npcFrame++;

                if (npcFrame >= cycleFrameTotal)
                {
                    npcFrame = 0;
                }


                // Reset the time counter, keeping remaining elapsed time
                timeCounter -= secondsPerFrame;
            }
        }
    }
}
