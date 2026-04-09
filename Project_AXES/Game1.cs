//using AXES_Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Project_AXES
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //screen specs
        private int screenWidth;
        private int screenHeight;

        //player
        private Texture2D playerTexture;
        private Player player;

        //Tile Manager 
        Texture2D tileSet;
        TileManager myTileManager;

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


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Player Textures
            playerTexture = Content.Load<Texture2D>("_Run");
            player = new Player(playerTexture, 3, new Vector2(0, 0));

            //Tile Mapping
            tileSet = Content.Load<Texture2D>("1_Industrial_Tileset_1B");
            myTileManager = new TileManager(tileSet, _spriteBatch);
            myTileManager.AssignTiles("../../../Content/textureMappingData.txt");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update();
            player.PreCollision();

            myTileManager.CollisionCheck(player);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            myTileManager.DisplayTiles();
            player.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
