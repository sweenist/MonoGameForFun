using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TestGame.Camera;
using TestGame.Maps;
using TestGame.Services;

namespace TestGame
{
    public class SweenGame : Game
    {
        public static int SCREEN_WIDTH = 912; //19 * 48;
        public static int SCREEN_HEIGHT = 624; //13 * 48;
        private readonly IServiceLocator _serviceLocator;
        private GraphicsDeviceManager _graphicsManager;
        private SpriteBatch _spriteBatch;
        private MovementManager _movementManager;
        private Song _themeMusic;

        public SweenGame()
        {
            _graphicsManager = new GraphicsDeviceManager(this);
            _serviceLocator = new ServiceLocator();

            Content.RootDirectory = "Content";
            TargetElapsedTime = TimeSpan.FromMilliseconds(25);
        }

        protected override void Initialize()
        {
            _graphicsManager.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphicsManager.PreferredBackBufferHeight = SCREEN_HEIGHT;
            _graphicsManager.ApplyChanges();

            Window.Title = "Sween's Awesome Game";

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _movementManager = new MovementManager(this, _serviceLocator);
            Components.Add(_movementManager);

            _serviceLocator.AddService<IMap>(typeof(Map), this, _serviceLocator);
            Components.Add(_serviceLocator.GetService<IMap>());

            var player = new Player(this, _spriteBatch, Content, _serviceLocator);
            _serviceLocator.AddService<IPlayer>(player);
            _serviceLocator.AddService<IFocusable>(player);
            Components.Add(player);

            _serviceLocator.AddService<ICamera2D>(typeof(Camera2D), this, _serviceLocator);
            Components.Add(_serviceLocator.GetService<ICamera2D>());

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
