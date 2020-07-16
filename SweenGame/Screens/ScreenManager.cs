using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SweenGame.Enums;
using SweenGame.Extensions;
using SweenGame.Input;

namespace SweenGame.Screens
{
    public class ScreenManager : DrawableGameComponent, IScreenManager
    {
        private IGraphicsDeviceService _graphicsDeviceService;
        private readonly ContentManager _contentManager;
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private InputState _input;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private Texture2D _blankTexture;

        private ICollection<GameScreen> _gameScreens = new List<GameScreen>();
        private List<GameScreen> _screensToUpdate = new List<GameScreen>();

        bool _traceEnabled;

        public ScreenManager(Game game, GraphicsDeviceManager graphicsDeviceManager) : base(game)
        {
            _contentManager = game.Content;
            _graphicsDeviceManager = graphicsDeviceManager;
            _graphicsDeviceService = game.Services.GetService<IGraphicsDeviceService>();

            if (_graphicsDeviceService is null)
                throw new InvalidOperationException("No graphics device service was found");

            _input = new InputState();
            //_traceEnabled = true;
        }

        public SpriteBatch SpriteBatch => _spriteBatch;
        public SpriteFont SpriteFont => _spriteFont;
        public GraphicsDeviceManager GraphicsManager => _graphicsDeviceManager;
        public ContentManager Content => _contentManager;

        public bool TraceEnabled
        {
            get => _traceEnabled;
            set => _traceEnabled = value;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteFont = _contentManager.Load<SpriteFont>("Fonts/Arial20");
            _blankTexture = new Texture2D(GraphicsDevice, Constants.ScreenWidth, Constants.ScreenHeight);
            var textureData = Enumerable.Repeat(Color.Black, Constants.ScreenWidth * Constants.ScreenHeight).ToArray();

            _blankTexture.SetData(textureData);

            foreach (var screen in _gameScreens)
            {
                screen.LoadContent();
            }
        }

        protected override void UnloadContent()
        {
            foreach (var screen in _gameScreens)
            {
                screen.UnloadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            _input.Update();
            _screensToUpdate.Clear();
            foreach (var screen in _gameScreens)
                _screensToUpdate.Add(screen);
            var otherScreenHasFocus = !base.Game.IsActive;
            var coveredByOtherScreen = false;

            while (_screensToUpdate.Count > 0)
            {
                var screenIndex = _screensToUpdate.Count - 1;
                var screen = _screensToUpdate[screenIndex];
                _screensToUpdate.RemoveAt(screenIndex);
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
                {
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(_input, gameTime);
                        otherScreenHasFocus = true;
                    }
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }
            if (_traceEnabled)
                TraceScreens();
        }

        private void TraceScreens()
        {
            Console.WriteLine(string.Join(',', _gameScreens.Select(s => s.GetType().Name).ToList()));
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var screen in _gameScreens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }

        public void AddScreen(GameScreen screen)
        {
            screen.Initialize();
            _gameScreens.Add(screen);
            Console.WriteLine(string.Join(',', _gameScreens.Select(s => s.GetType().Name)));

            if (_graphicsDeviceService != null && _graphicsDeviceService.GraphicsDevice != null)
                screen.LoadContent();
        }

        public void RemoveScreen(GameScreen screen)
        {
            _gameScreens.Remove(screen);
            _screensToUpdate.Remove(screen);

            if (_graphicsDeviceService != null && _graphicsDeviceService.GraphicsDevice != null)
                screen.UnloadContent();
        }

        public void FadeBackBufferToBlack(int alpha)
        {
            var viewport = GraphicsDevice.Viewport;
            _spriteBatch.Begin();
            _spriteBatch.Draw(_blankTexture,
            new Rectangle(0, 0, viewport.Width, viewport.Height),
            new Color(0, 0, 0, alpha));

            _spriteBatch.End();
        }

        public GameScreen[] GetScreens()
        {
            return _gameScreens.ToArray();
        }

        public Game GetGame() => base.Game;
        public GraphicsDevice GetGraphicsDevice() => base.GraphicsDevice;
    }
}