using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace DialogueManager
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont arial12;
        private Texture2D textBox;
        private KeyboardState kbState;
        private DialogueManager dialogueManager;
        private bool test;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 800;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 800;   // set this value to the desired height of your window
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            test = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            arial12 = Content.Load<SpriteFont>("arial-12");
            textBox = Content.Load<Texture2D>("purple_txt_box");
            dialogueManager = new DialogueManager(arial12,
                textBox,
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState kbPrevState = kbState;
            kbState = Keyboard.GetState();


            if (kbPrevState.IsKeyDown(Keys.P) && kbState.IsKeyUp(Keys.P))
            {
                test = !test;
                if (test)
                {
                    dialogueManager.FileName = "Content/example_dialogue.txt";
                }
            }
            dialogueManager.Update(gameTime, test);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            dialogueManager.Draw(_spriteBatch);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
