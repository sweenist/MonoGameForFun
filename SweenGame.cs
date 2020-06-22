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
        private GraphicsDeviceManager _graphicsManager;
        private SpriteBatch _spriteBatch;
        private MovementManager _movementManager;
        private Player _player;
        private Song _themeMusic;
        private Map _map;
        private Camera2D _camera;

        public SweenGame()
        {
            _graphicsManager = new GraphicsDeviceManager(this);
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

            _movementManager = new MovementManager(this);
            Components.Add(_movementManager);

            _map = new Map(this, _spriteBatch, Content);
            _movementManager.Add(_map);
            Components.Add(_map);

            _player = new Player(this, _spriteBatch, Content);
            _movementManager.Add(_player);
            Components.Add(_player);

            _camera = new Camera2D(this,_player);
            _movementManager.Add(_camera);
            _player.Add(_camera);
            _map.Add(_camera);
            Components.Add(_camera);


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
