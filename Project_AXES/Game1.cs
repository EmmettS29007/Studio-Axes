//using AXES_Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project_Axes;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Project_AXES
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState kbState;

        //screen specs
        private int screenWidth;
        private int screenHeight;

        //player
        private Texture2D playerTexture;
        private Player player;

        //Tile Manager 
        Texture2D tileSet;
        TileManager myTileManager;

        //Dialogue Manager
        private DialogueManager dialogueManager;
        private SpriteFont arial12;
        private Texture2D textBox;
        private bool test;

        //Enemy and Entity Manager
        private Texture2D enemySprite;
        private Enemy enemy;
        private EntityManager entityManager;
        private List<Enemy> enemies;

        //HUD
        private HUD hud;
        private Texture2D heart;

        // Temporary task string for hud
        private string task;

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
            task = "Try out the game! (WIP)";
            enemies = new List<Enemy>();

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

            //Dialogue Box
            arial12 = Content.Load<SpriteFont>("arial-12");
            textBox = Content.Load<Texture2D>("purple_txt_box");
            dialogueManager = new DialogueManager(arial12,
                textBox,
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight);

            //HUD Initialization
            heart = Content.Load<Texture2D>("heart");
            hud = new HUD(heart, task, arial12, player);

            //Enemy and EntityManager Setup
            enemySprite = Content.Load<Texture2D>("tempEnemySprite");

            enemy = new Enemy(enemySprite, 3, new Vector2(200, 300), 76);
            enemies.Add(enemy);

            entityManager = new EntityManager(enemies);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Keyboard State
            KeyboardState kbPrevState = kbState;
            kbState = Keyboard.GetState();

            //player
            player.Update();
            player.PreCollision();

            //tile manager
            myTileManager.CollisionCheck(player);

            //dialogue
            if (kbPrevState.IsKeyDown(Keys.P) && kbState.IsKeyUp(Keys.P))
            {
                test = !test;
                if (test)
                {
                    dialogueManager.FileName = "Content/example_dialogue.txt";
                }
            }
            dialogueManager.Update(gameTime, test);
            entityManager.Update(gameTime, player);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            myTileManager.DisplayTiles();
            player.Draw(_spriteBatch);
            dialogueManager.Draw(_spriteBatch);
            hud.Draw(_spriteBatch, screenHeight);
            entityManager.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
