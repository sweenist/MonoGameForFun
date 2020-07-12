using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

            _screenManager.SpriteBatch.Begin();

            var color = Color.White;
            var scale = 1f;
            for (int i = 0; i < _menuEntries.Count; i++)
            {
                if (IsActive && i.Equals(_selectedIndex))
                {
                    var time = gameTime.TotalGameTime.TotalSeconds;
                    var pulse = (float)Math.Sin(time * 6) + 1;

                    color = Color.YellowGreen;
                    scale = 1 + pulse * 0.05f;
                }

                color.A = TransitionAlpha;
                var origin = new Vector2(0, _screenManager.SpriteFont.LineSpacing / 2);
                _screenManager.SpriteBatch.DrawString(_screenManager.SpriteFont,
                                                      _menuEntries[i],
                                                      position,
                                                      color,
                                                      rotation: 0,
                                                      origin,
                                                      scale,
                                                      SpriteEffects.None,
                                                      layerDepth: 0);

                position.Y += _screenManager.SpriteFont.LineSpacing;
            }

            _screenManager.SpriteBatch.End();
        }
    }
}