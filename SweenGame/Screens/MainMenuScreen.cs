using System;
using Microsoft.Xna.Framework;
using SweenGame.Extensions;
using SweenGame.Input;
using SweenGame.Services;
using SweenGame.Sounds;

namespace SweenGame.Screens
{
    public class MainMenuScreen : MenuScreen
    {
        private ISoundManager _sounds;

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
            _sounds = ServiceLocator.Instance.GetService<ISoundManager>();
            _sounds.LoadSong(SongNames.Title);
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
                    _sounds.StopSong();
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

        public override void HandleInput(InputState state, GameTime gameTime)
        {
            if (state.MenuUp || state.MenuDown)
                _sounds.Play(SoundEffectNames.SelectEffect);
            else if (state.MenuSelect)
                _sounds.Play(SoundEffectNames.SelectEffect, -0.33f);

            base.HandleInput(state, gameTime);
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