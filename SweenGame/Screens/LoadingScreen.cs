using System;
using Microsoft.Xna.Framework;
using SweenGame.Enums;
using SweenGame.Services;

namespace SweenGame.Screens
{
    public class LoadingScreen : GameScreen
    {
        private bool _isSlowLoading;
        private bool _otherScreensAreGone;

        private LoadingScreen()
        {
            _transitionOnTime = TimeSpan.FromMilliseconds(500);
        }

        event EventHandler<EventArgs> loadNextScreen;

        public bool IsSlowLoading
        {
            get => _isSlowLoading;
            set => _isSlowLoading = value;
        }
        public bool OtherScreensAreGone
        {
            get => _otherScreensAreGone;
            set => _otherScreensAreGone = value;
        }

        public static void Load(EventHandler<EventArgs> loadNextScreen, bool loadingIsSlow)
        {
            var screenManager = ServiceLocator.Instance.GetService<IScreenManager>();

            foreach (var screen in screenManager.GetScreens())
                screen.ExitScreen();

            var loadingScreen = new LoadingScreen();
            loadingScreen.loadNextScreen = loadNextScreen;
            loadingScreen.IsSlowLoading = loadingIsSlow;

            screenManager.AddScreen(loadingScreen);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool isCoveredByOtherScreen)
        {
            if (_otherScreensAreGone)
            {
                _screenManager.RemoveScreen(this);
                loadNextScreen(this, EventArgs.Empty);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (ScreenState == ScreenState.Active
            && _screenManager.GetScreens().Length.Equals(1))
            {
                _otherScreensAreGone = true;
            }
            if (_isSlowLoading)
            {
                // do something with a loading message here...
            }

        }
    }
}