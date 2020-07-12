using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SweenGame.Extensions;

namespace SweenGame.Screens
{
    public class CreditsScreen : GameScreen
    {
        private const string _creditsTitle = "C R E D I T S";

        private SpriteFont _titleFont;
        private IDictionary<string, string[]> CreditRoles = new Dictionary<string, string[]>{
            {"Everything", new[]{"Sween"}},
            {"With help from", new[]{"DaKota Reid", "Monogame Succinctly", "The Internet"}}
        };

        public CreditsScreen()
        {
            _transitionOnTime = TimeSpan.FromMilliseconds(1500);
            _transitionOffTime = TimeSpan.FromMilliseconds(500);
        }

        public override void LoadContent()
        {
            _titleFont = _screenManager.Content.Load<SpriteFont>("Fonts/Arial28");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 textPosition;
            Vector2 textSize;
            _screenManager.GetGraphicsDevice().Clear(ClearOptions.Target,
                                                     Color.Black,
                                                     depth: 0,
                                                     stencil: 0);
            var color = Color.Black;
            color.A = TransitionAlpha;

            _screenManager.SpriteBatch.Begin();

            #region Draw Title

            var titleSize = _titleFont.MeasureString(_creditsTitle);
            var viewport = _screenManager.GetGraphicsDevice().Viewport;

            textPosition = new Vector2((viewport.Width / 2) - (titleSize.X / 2), 5);
            _screenManager.SpriteBatch.DrawString(_titleFont, _creditsTitle, textPosition, color);

            #endregion

            #region Draw Credits

            color = Color.White;
            color.A = TransitionAlpha;

            int count = 0;

            foreach (var kvp in CreditRoles)
            {
                textPosition = new Vector2(10, 150 + 50 * count);

                _screenManager.SpriteBatch.DrawString(_screenManager.SpriteFont, kvp.Key, textPosition, color);

                foreach (var creditor in kvp.Value)
                {
                    textSize = _screenManager.SpriteFont.MeasureString(creditor);
                    textPosition = new Vector2(viewport.Width - textSize.X - 10, 150 + 50 * count);

                    _screenManager.SpriteBatch.DrawString(_screenManager.SpriteFont, creditor, textPosition, color);
                    count++;
                }
            }

            #endregion

            #region Exit Prompt

            textSize = _screenManager.SpriteFont.MeasureString(Constants.CancelActionPrompt);
            textPosition = new Vector2(viewport.Width - textSize.X - 10, viewport.Height - textSize.Y - 10);

            _screenManager.SpriteBatch.DrawString(_screenManager.SpriteFont, Constants.CancelActionPrompt, textPosition, color);

            #endregion

            _screenManager.SpriteBatch.End();
        }
    }
}