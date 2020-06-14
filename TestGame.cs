using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame
{
    public class TestGame : Game
    {
        const int SCREEN_WIDTH = 19 * 48;
        const int SCREEN_HEIGHT = 13 * 48;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D _playerAtlas;
        private Player _player;

        public TestGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.ApplyChanges();
            Window.Title = "Sween's Awesome Game";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _playerAtlas = Content.Load<Texture2D>("Sprites/dw_green_hero");
            _player = new Player(_playerAtlas, 4, 8);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _player.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _player.Draw(spriteBatch, new Vector2(256, 256));

            base.Draw(gameTime);
        }
    }
}
