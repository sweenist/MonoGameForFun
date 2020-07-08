using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SweenGame.Maps;
using SweenGame.Camera;
using SweenGame.Extensions;
using SweenGame.Services;
using static SweenGame.Extensions.Constants;

namespace SweenGame
{
    public class SweenGame : Game
    {
        private GraphicsDeviceManager _graphicsManager;
        private MovementManager _movementManager;
        private Song _themeMusic;

        public SweenGame()
        {
            _graphicsManager = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            TargetElapsedTime = TimeSpan.FromMilliseconds(50);
        }

        protected override void Initialize()
        {
            _graphicsManager.PreferredBackBufferWidth = ScreenWidth;
            _graphicsManager.PreferredBackBufferHeight = ScreenHeight;
            _graphicsManager.ApplyChanges();

            Window.Title = "Sween's Awesome Game";

            ServiceLocator.Instance.AddService<IMapManager>(typeof(MapManager), this);

            _movementManager = new MovementManager(this);
            Components.Add(_movementManager);

            var player = new Player(this);
            ServiceLocator.Instance.AddService<IPlayer>(player);
            ServiceLocator.Instance.AddService<IFocusable>(player);
            Components.Add(player);

            ServiceLocator.Instance.AddService<ICamera2D>(typeof(Camera2D), this);
            Components.Add(ServiceLocator.Instance.GetService<ICamera2D>());

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

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                var player = ServiceLocator.Instance.GetService<IPlayer>();
                var map = ServiceLocator.Instance.GetService<IMap>(Constants.Current);
                var tile = map.GetTileAt(player.Destination);
                var d = map as DrawableGameComponent;

                Console.WriteLine($"Tile Info:\n\tTile Location: {tile.Location}\n\tTile Collide? {tile.IsCollideable}\n\tTile Draw Index: {d?.DrawOrder}");
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
