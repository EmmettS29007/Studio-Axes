using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemy_EntityManager_HUD
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D squirtle;
        private Texture2D heart;
        private SpriteFont arial32;

        private Enemy evilSquirtle;

        private EntityManager entityManager;
        private HUD hud;
        private List<Enemy> enemies;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            enemies = new List<Enemy>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            squirtle = Content.Load<Texture2D>("squirtle!");
            heart = Content.Load<Texture2D>("heart");
            arial32 = Content.Load<SpriteFont>("arial-32");

            evilSquirtle = new Enemy(squirtle, 3, new Vector2(50, 200));
            enemies.Add(evilSquirtle);
            entityManager = new EntityManager(enemies);
            hud = new HUD(heart, "nothing.", arial32);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            entityManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            entityManager.Draw(_spriteBatch);
            hud.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
