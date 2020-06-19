using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TestGame.Maps;

namespace TestGame
{
    public class SweenGame : Game
    {
        public static int SCREEN_WIDTH = 19 * 48;
        public static int SCREEN_HEIGHT = 13 * 48;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private MovementManager _movementManager;
        private Player _player;
        private Song _themeMusic;
        private Map _map;

        public SweenGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            TargetElapsedTime = TimeSpan.FromMilliseconds(25);
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.ApplyChanges();
            Window.Title = "Sween's Awesome Game";

            spriteBatch = new SpriteBatch(GraphicsDevice);

            _movementManager = new MovementManager(this);
            Components.Add(_movementManager);

            _map = new Map(this, spriteBatch, Content);
            _movementManager.Add(_map);
            Components.Add(_map);

            _player = new Player(this, spriteBatch, Content);
            _movementManager.Add(_player);
            Components.Add(_player);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _themeMusic = Content.Load<Song>("Music/mega_adventure");

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Volume = 0.75f;
                MediaPlayer.Play(_themeMusic);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}
