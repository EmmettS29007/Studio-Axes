//using AXES_Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Project_Axes;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;


namespace Project_AXES
{

    enum GameState
    {
        Menu,
        Game,
        GameOver,
        Win
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState kbState;

        // Game States
        private GameState gameState;

        //screen specs
        private int screenWidth;
        private int screenHeight;

        //player
        private Texture2D playerTexture;
        private Texture2D playerSpriteSheet;
        private Player player;

        //Tile Manager 
        private Texture2D tileSet;
        private TileManager myTileManager;
        private Texture2D myBackground;

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
        private string[] task;

        // Menu
        private Menu menu;

        //Camera
        private Camera camera;

        //Audio
        private Song backgroundMusic;
        private SoundEffect hit;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screenHeight = 1080;
            screenWidth = 1920;
            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            task = new string[2];
            string task1 = "Try out the game! (WIP)";
            task[0] = task1;

            enemies = new List<Enemy>();
            gameState = GameState.Menu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Audio
            backgroundMusic = Content.Load<Song>
                ("backgroundmusicforvideos-game-gaming-background-music-385611");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;

            hit = Content.Load<SoundEffect>("attack");

            //Player Textures
            playerTexture = Content.Load<Texture2D>("_Run");
            playerSpriteSheet = Content.Load<Texture2D>("knightSpriteSheet");
            player = new Player(playerSpriteSheet, playerTexture, 3, new Vector2(0, 0), hit);

            //Tile Mapping
            tileSet = Content.Load<Texture2D>("1_Industrial_Tileset_1B");
            myTileManager = new TileManager(tileSet, _spriteBatch);
            myTileManager.AssignTiles("../../../Content/textureMappingData.txt");

            //BG Texture
            myBackground = Content.Load<Texture2D>("10");

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

            enemy = new Enemy(enemySprite, 3, new Vector2(900, 700), 76);
            enemies.Add(enemy);

            entityManager = new EntityManager(enemies, enemySprite, player);

            //Camera
            camera = new(player, enemies, entityManager.Milk, myTileManager.TileList, screenWidth, screenHeight);

            // Menu
            menu = new Menu(arial12, textBox, textBox, textBox, screenWidth, screenHeight);



        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Keyboard State
            KeyboardState kbPrevState = kbState;
            kbState = Keyboard.GetState();

            // The game's FSM
            switch (gameState)
            {
                case GameState.Menu:
                    if (kbPrevState.IsKeyDown(Keys.P) && kbState.IsKeyUp(Keys.P))
                    {
                        gameState = GameState.Game;
                    }
                    // menu.Update(gameState);
                    break;

                case GameState.Game:
                    // Updates the player and checks collision
                    player.Update(gameTime);
                    player.PreCollision();

                    // Checks collision for each player
                    myTileManager.CollisionCheck(player);

                    // Dialogue Debugging
                    if (kbPrevState.IsKeyDown(Keys.P) && kbState.IsKeyUp(Keys.P))
                    {
                        test = true;
                        if (test)
                        {
                            dialogueManager.FileName = "Content/example_dialogue.txt";
                        }
                    }
                    dialogueManager.Update(gameTime, test);
                    entityManager.Update(gameTime);
                    camera.Update();
                    break;

                case GameState.GameOver:
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            switch (gameState)
            {
                case GameState.Menu:
                    _spriteBatch.DrawString(arial12, "Press P to play",
                        new Vector2(screenWidth / 2, screenHeight / 2),
                        Color.White);
                    menu.Draw(_spriteBatch);
                    break;

                case GameState.Game:
                    //this is layered for view!! do not change layering!
                    _spriteBatch.Draw(myBackground, Vector2.Zero, Color.White); //bg
                    myTileManager.DisplayTiles(); //tiles
                    player.Draw(_spriteBatch); //player
                    dialogueManager.Draw(_spriteBatch);
                    hud.Draw(_spriteBatch, screenHeight);
                    entityManager.Draw(_spriteBatch);
                    break;

                case GameState.GameOver:
                    break;
            }



            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
