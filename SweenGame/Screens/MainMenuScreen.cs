using System;
using Microsoft.Xna.Framework;
using SweenGame.Extensions;

namespace SweenGame.Screens
{
    public class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
        {
            _menuEntries.Add("Continue");
            _menuEntries.Add("Load");
            _menuEntries.Add("New Game");
            _menuEntries.Add("Credits");
            _menuEntries.Add("Exit");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            _screenManager.LoadSong(MusicResources.Title);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool isCoveredByOtherScreen)
        {

            base.Update(gameTime, otherScreenHasFocus, isCoveredByOtherScreen);
        }

        protected override void OnSelectEntry(int entryIndex)
        {
            switch (entryIndex)
            {
                case 0:
                case 1:
                case 2:
                    _screenManager.StopSong();
                    LoadingScreen.Load((_, __) => _screenManager.AddScreen(new GameplayScreen(_screenManager.GetGame())), true);
                    break;
                case 3:
                    _screenManager.AddScreen(new CreditsScreen());
                    break;
                case 4:
                    OnCancel();
                    break;
            }
        }

        protected override void OnCancel()
        {
            var messageBox = new MessageBoxScreen(Constants.CancelMessage);
            messageBox.Accepted += ExitMessageBoxAccepted;
            _screenManager.AddScreen(messageBox);
        }

        private void ExitMessageBoxAccepted(object sender, EventArgs e)
        {
            _screenManager.GetGame().Exit();
        }
    }
}