using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SweenGame.Enums;
using SweenGame.Input;
using SweenGame.Screens;
using SweenGame.Services;
using static SweenGame.Extensions.Constants;

namespace SweenGame
{
    public class SweenGame : Game
    {
        private GraphicsDeviceManager _graphicsManager;
        private Song _themeMusic;
        private ScreenManager _screenManager;

        public SweenGame()
        {
            _graphicsManager = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            TargetElapsedTime = TimeSpan.FromMilliseconds(25);
            //GetDisplayModes();
        }

        private static void GetDisplayModes()
        {
            foreach (var mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                Console.WriteLine($"Aspect: {mode.AspectRatio}\n\tSize: {mode.Width}x{mode.Height} Safe Area:{mode.TitleSafeArea}");
            }
        }

        protected override void Initialize()
        {
            _graphicsManager.PreferredBackBufferWidth = ScreenWidth;
            _graphicsManager.PreferredBackBufferHeight = ScreenHeight;
            _graphicsManager.ApplyChanges();

            Window.Title = "Sween's Awesome Game";

            _screenManager = new ScreenManager(this, _graphicsManager);
            ServiceLocator.Instance.AddService<IScreenManager>(_screenManager);
            _screenManager.Initialize();
            _screenManager.AddScreen(new GameplayScreen(this));
            Components.Add(_screenManager);

            var input = new InputSystem(this);
            input.Enabled = true;
            input.AddAction("MoveLeft", Control.Left, ActionType.MoveLeft, null);
            input.AddAction("MoveUp", Control.Up, ActionType.MoveUp, null);
            input.AddAction("MoveRight", Control.Right, ActionType.MoveRight, null);
            input.AddAction("MoveDown", Control.Down, ActionType.MoveDown, null);
            ServiceLocator.Instance.AddService<IInputSystem>(input);
            Components.Add(input);

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
