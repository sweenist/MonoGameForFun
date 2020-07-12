using System;
using Microsoft.Xna.Framework;
using SweenGame.Extensions;

namespace SweenGame.Screens
{
    public class PauseMenuScreen : MenuScreen
    {

        public PauseMenuScreen()
        {
            _menuEntries.Add("Resume Game");
            _menuEntries.Add("Quit Game");

            _isPopup = true;
        }

        public override void Draw(GameTime gameTime)
        {
            _screenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            base.Draw(gameTime);
        }

        protected override void OnCancel()
        {
            ExitScreen();
        }

        protected override void OnSelectEntry(int entryIndex)
        {
            switch (entryIndex)
            {
                case 0:
                    ExitScreen();
                    break;

                case 1:
                    BuildCancelMessageBox();
                    break;
            }
        }

        private void BuildCancelMessageBox()
        {
            var messageBox = new MessageBoxScreen(Constants.CancelMessage);
            messageBox.Accepted += QuitMessageBoxAccepted;
            _screenManager.AddScreen(messageBox);
        }

        private void QuitMessageBoxAccepted(object sender, EventArgs e)
        {
            LoadingScreen.Load(LoadMainMenuScreen, false);
        }

        private void LoadMainMenuScreen(object sender, EventArgs e)
        {
            _screenManager.AddScreen(new MainMenuScreen());
        }
    }
}