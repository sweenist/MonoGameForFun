using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SweenGame.Enums;

namespace SweenGame.Screens
{
    public class ScreenManager : DrawableGameComponent, IScreenManager
    {
        ICollection<GameScreen> _gameScreens = new List<GameScreen>();
        List<GameScreen> _screensToUpdate = new List<GameScreen>();

        bool _traceEnabled;

        public ScreenManager(Game game, GraphicsDeviceManager graphicsDeviceManager) : base(game)
        {

        }

        public bool TraceEnabled
        {
            get => _traceEnabled;
            set => _traceEnabled = value;
        }

        protected override void LoadContent()
        {
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
            var screennames = _gameScreens.Select(s => s.GetType().Name).ToList();
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
        }

        public void RemoveScreen(GameScreen screen)
        {
            _gameScreens.Remove(screen);
            _screensToUpdate.Remove(screen);
        }

        public GameScreen[] GetScreens()
        {
            return _gameScreens.ToArray();
        }

        public Game GetGame() => base.Game;
    }
}