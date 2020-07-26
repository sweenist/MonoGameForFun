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
            _screenManager.GetGraphicsDevice().Clear(ClearOptions.Target, Color.Black, 0, 0);

            var position = new Vector2(100, 150);
            var transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            if (ScreenState == ScreenState.TransitionOn)
                position.X -= transitionOffset * 256;
            else
                position.X += transitionOffset * 512;

            _screenManager.SpriteBatch.Begin();

            Color color;
            float scale;

            for (int i = 0; i < _menuEntries.Count; i++)
            {
                if (IsActive && i.Equals(_selectedIndex))
                {
                    var time = gameTime.TotalGameTime.TotalSeconds;
                    var pulse = (float)Math.Sin(time * 6) + 1;

                    color = Color.YellowGreen;
                    scale = 1 + pulse * 0.05f;
                }
                else
                {
                    color = Color.White;
                    scale = 1f;
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

        public override void HandleInput(Input.InputState state, GameTime gameTime)
        {
            if (state.MenuUp)
            {
                _selectedIndex--;
                if (_selectedIndex < 0)
                    _selectedIndex = _menuEntries.Count - 1;
            }
            if (state.MenuDown)
            {
                _selectedIndex++;
                if (_selectedIndex.Equals(_menuEntries.Count))
                    _selectedIndex = 0;
            }
            if (state.MenuSelect)
            {
                OnSelectEntry(_selectedIndex);
            }
            if (state.MenuCancel)
                OnCancel();

            if (state.IsNewKeyPress(Keys.Left))
                OnNewArrowDown(Keys.Left, _selectedIndex);
            else if (state.IsKeyDown(Keys.Left))
                OnArrowDown(Keys.Left, _selectedIndex, gameTime);
            if (state.IsNewKeyUp(Keys.Left))
                OnArrowUp(Keys.Left, _selectedIndex);

            if (state.IsNewKeyPress(Keys.Right))
                OnNewArrowDown(Keys.Right, _selectedIndex);
            else if (state.IsKeyDown(Keys.Right))
                OnArrowDown(Keys.Right, _selectedIndex, gameTime);
            if (state.IsNewKeyUp(Keys.Right))
                OnArrowUp(Keys.Right, _selectedIndex);
        }
    }
}