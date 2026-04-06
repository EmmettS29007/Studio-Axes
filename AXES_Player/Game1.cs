using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace AXES_Player
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private int screenWidth;
        private int screenHeight;
        private SpriteBatch _spriteBatch;
        private Texture2D playerTexture;
        private Player player;
        private List<Tile> tempTileManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screenHeight = GraphicsDevice.DisplayMode.Height;
            screenWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            tempTileManager = new List<Tile>();
            tempTileManager.Add(new Tile(new Rectangle(0, screenHeight / 3 * 2, screenWidth, 256)));
            tempTileManager.Add(new Tile(new Rectangle(0, screenHeight / 2, 256, screenHeight)));
            tempTileManager.Add(new Tile(new Rectangle(0, screenHeight / 3, screenWidth / 3, 256)));
            tempTileManager.Add(new Tile(new Rectangle(screenWidth - 256, screenHeight / 2, 256, screenHeight)));
            tempTileManager.Add(new Tile(new Rectangle(screenWidth / 3 * 2, screenWidth / 3, 256, 256)));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            playerTexture = Content.Load<Texture2D>("_Run");
            player = new Player(playerTexture, 100, new Vector2(screenWidth / 2, -600));//screenHeight / 2));


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            player.PreCollision();
            foreach(Tile tile in tempTileManager)
            {
                player.DetectCollision(tile);
            }
            player.Update();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            player.Draw(_spriteBatch);
            foreach (Tile tile in tempTileManager)
            {
                tile.Draw(_spriteBatch);
            }
            

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
