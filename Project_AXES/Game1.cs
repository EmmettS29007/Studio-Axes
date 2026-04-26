//using AXES_Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Project_Axes;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization.Formatters;
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
        private bool debug;
        private bool hasntReset;

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
        private SpriteFont arial16;
        private Texture2D textBox;
        private bool test;

        //Enemy and Entity Manager
        private Texture2D enemySprite;
        private Enemy enemy;
        private EntityManager entityManager;
        private List<Enemy> enemies;
        private Texture2D milkSprite;

        //NPC 
        private Texture2D npcSpritesheet;
        private NPC npc;

        //HUD
        private HUD hud;
        private Texture2D heart;
        private SpriteFont arial24;

        // Temporary task string for hud
        private string[] task;

        // Menu and Win / Loss screens
        private Menu menu;
        private Texture2D titleScreen;
        private Texture2D controlsGuide;
        private Texture2D gameOver;
        private Texture2D winScreen;

        //...milk
        private Enemy milk;

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
            string task1 = "Talk to thy maiden!";
            string task2 = "Get thy milk!";
            task[0] = task1;
            task[1] = task2;

            enemies = new List<Enemy>();
            gameState = GameState.Menu;
            debug = false;
            hasntReset = true;

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

            //Player Setup
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
            arial16 = Content.Load<SpriteFont>("arial-12");
            textBox = Content.Load<Texture2D>("HUD Text Box");
            dialogueManager = new DialogueManager(arial16,
                textBox,
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight);

            //HUD Initialization
            heart = Content.Load<Texture2D>("heart");
            arial24 = Content.Load<SpriteFont>("arial-24");
            hud = new HUD(heart, task, arial24, player);


            //NPC Setup
            npcSpritesheet = Content.Load<Texture2D>("npc_idle");
            npc = new NPC(npcSpritesheet, new Rectangle(900, 750,250,1000));

            //...milk
            milkSprite = Content.Load<Texture2D>("milk");
            milk = new Enemy(milkSprite, 1, new Vector2(8085, 335), 0);

            //Enemy and EntityManager Setup
            enemySprite = Content.Load<Texture2D>("KABLOOEY");

            enemy = new Enemy(enemySprite, 3, new Vector2(3000, 500), 76);
            enemies.Add(enemy);

            entityManager = new EntityManager(enemies, milkSprite, milk, player, npc);

            //Camera
            camera = new(player, enemies, entityManager.Milk, npc, myTileManager.TileList, screenWidth, screenHeight);

            // Menu & Win / Loss
            titleScreen = Content.Load<Texture2D>("titleScreen");
            controlsGuide = Content.Load<Texture2D>("controls_guide");
            menu = new Menu(arial24, textBox, titleScreen, controlsGuide, screenWidth, screenHeight);
            gameOver = Content.Load<Texture2D>("game_over_screen");
            winScreen = Content.Load<Texture2D>("win_screen");
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
                    //Resets game state in menu
                    player.Reset();
                    //camera ia made new so it doesn't
                    //spawn in where the last one was
                    camera = new(player, enemies, entityManager.Milk, 
                        npc, myTileManager.TileList, screenWidth, screenHeight);
                    //reset is run to re-reposition everything
                    camera.Reset();
                    gameState = menu.Update();
                    if(gameState == GameState.Game)
                    {
                        hasntReset = true;
                    }
                    break;

                case GameState.Game:
                    if (hasntReset)
                    {
                        entityManager = new EntityManager(enemies, milkSprite, milk, player, npc);
                        foreach(Enemy enemy in enemies)
                        {
                            enemy.Reset();
                            
                        }
                        hud.Reset();
                        player.Health = 3;
                        hasntReset = false;
                    }
                    // Updates the player and checks collision
                    player.Update(gameTime);
                    player.PreCollision();

                    // Checks collision for each player
                    myTileManager.CollisionCheck(player);

                    dialogueManager.Update(gameTime, test);
                    camera.Update();
                    entityManager.Update(gameTime);

                    //NPC stuff
                    // Checks if player is in interact range w/ npc
                    if (npc.Interact(player))
                    {
                        if (kbPrevState.IsKeyDown(Keys.E) && kbState.IsKeyUp(Keys.E))
                        {
                            test = true;
                            if (test)
                            {
                                dialogueManager.FileName = "Content/npc_dialogue.txt";
                            }
                            hud.Update();
                        }
                    }

                    //Win state debug
                    if (kbPrevState.IsKeyDown(Keys.X) && kbState.IsKeyUp(Keys.X))
                    {
                        entityManager.Win = true;
                    }

                    if (kbPrevState.IsKeyDown(Keys.Tab) && kbState.IsKeyUp(Keys.Tab))
                    {
                        debug = !debug;
                    }
                    // Dialogue Debugging
                    if (debug && kbPrevState.IsKeyDown(Keys.P) && kbState.IsKeyUp(Keys.P))
                    {
                        test = true;
                        if (test)
                        {
                            dialogueManager.FileName = "Content/example_dialogue.txt";
                        }
                    }

                    // If the player health is zero...
                    if (player.Health <= 0)
                    {
                        // Enter GameOver state
                        player.Health = player.Max;
                        gameState = GameState.GameOver;
                    }
                    // If the milk is collected...
                    if (entityManager.Win)
                    {
                        // Enter Win state
                        gameState = GameState.Win;
                    }
                    break;

                case GameState.GameOver:
                    if (kbPrevState.IsKeyDown(Keys.Enter) && kbState.IsKeyUp(Keys.Enter))
                    {
                        gameState = GameState.Menu;
                    }
                    break;
                case GameState.Win:
                    if (kbPrevState.IsKeyDown(Keys.Enter) && kbState.IsKeyUp(Keys.Enter))
                    {
                        gameState = GameState.Menu;
                    }
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
                    menu.Draw(_spriteBatch);
                    break;

                case GameState.Game:
                    //this is layered for view!! do not change layering!
                    _spriteBatch.Draw(myBackground, Vector2.Zero, Color.White); //bg
                    myTileManager.DisplayTiles(); //tiles
                    player.Draw(_spriteBatch, debug); //player
                    hud.Draw(_spriteBatch, screenHeight);
                    entityManager.Draw(_spriteBatch);
                    dialogueManager.Draw(_spriteBatch);

                    if (debug == true)
                    {
                        DrawDebug(_spriteBatch);
                    }

                    break;

                case GameState.GameOver:
                    // Draws the loss screen
                    _spriteBatch.Draw(gameOver,
                        new Rectangle(0, 0, screenWidth, screenHeight),
                        Color.White);
                    break;
                case GameState.Win:
                    // Draws the win screen
                    _spriteBatch.Draw(winScreen,
                        new Rectangle(0, 0, screenWidth, screenHeight),
                        Color.White);
                    break;
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawDebug (SpriteBatch sb)
        {
            sb.DrawString
                (arial24,
                "Press P to see test dialogue",
                new Vector2
                    (screenWidth/2 - arial16.MeasureString("Press P to see test dialogue").X,
                    screenHeight - 70),
                Color.Pink);

            sb.DrawString
                (arial24,
                $"Player Coords - X: {player.Position.X}, Y: {player.Position.Y}",
                new Vector2
                    (20,
                    screenHeight - 130),
                Color.Pink);

            sb.DrawString
                (arial24,
                "Press TAB to exit debug mode",
                new Vector2
                    (20,
                    screenHeight - 160),
                Color.Pink);

            sb.DrawString
                (arial24,
                $"Enemies Left {enemies.Count}",
                new Vector2
                    (20,
                    screenHeight - 190),
                Color.Pink);
        }
    }
}
