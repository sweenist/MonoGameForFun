using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SweenGame.Enums;

namespace SweenGame.Screens
{
    public abstract class MenuScreen : GameScreen
    {
        protected IList<string> _menuEntries = new List<string>();
        private int _selectedIndex = 0;

        public MenuScreen()
        {
            TransistionOnTime = TimeSpan.FromMilliseconds(500);
            TransistionOffTime = TimeSpan.FromMilliseconds(500);
        }

        protected IList<string> MenuEntries => _menuEntries;

        public override void Initialize()
        {
            base.Initialize();
        }

        protected abstract void OnSelectEntry(int entryIndex);
        protected virtual void OnNewArrowDown(Keys arrow, int entryIndex) { }
        protected virtual void OnArrowDown(Keys arrow, int entryIndex, GameTime gametime) { }
        protected virtual void OnArrowUp(Keys arrow, int entryIndex) { }

        protected abstract void OnCancel();

        public override void Draw(GameTime gameTime)
        {
            var position = new Vector2(100, 150);
            var transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            if (ScreenState == ScreenState.TransitionOn)
                position.X -= transitionOffset * 256;
            else
                position.X += transitionOffset * 512;
        }
    }
}