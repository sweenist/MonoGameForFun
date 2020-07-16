using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SweenGame.Screens
{
    public class MessageBoxScreen : GameScreen
    {
        private readonly string _usageText = "Press <ENTER> or Space for Ok"
                                             + $"{Environment.NewLine}<ESC> Cancels";
        private readonly string _message;
        private Texture2D _gradient;

        public MessageBoxScreen(string message)
        {
            _message = message + Environment.NewLine + _usageText;
            _isPopup = true;
            _transitionOnTime = TimeSpan.FromMilliseconds(200);
            _transitionOffTime = TimeSpan.FromMilliseconds(200);
        }

        public event EventHandler<EventArgs> Accepted;
        public event EventHandler<EventArgs> Cancelled;


        public override void LoadContent()
        {
            _gradient = _screenManager.GetGame().Content.Load<Texture2D>("MessageGradient");
        }

        public override void Draw(GameTime gameTime)
        {
            _screenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            var viewport = _screenManager.GetGraphicsDevice().Viewport;
            var viewportSize = new Vector2(viewport.Width, viewport.Height);
            var textSize = _screenManager.SpriteFont.MeasureString(_message);
            var textPosition = (viewportSize - textSize) / 2;

            var hPad = 32;
            var vPad = 16;

            var backgroundRect = new Rectangle((int)textPosition.X - hPad,
                                               (int)textPosition.Y - vPad,
                                               (int)textSize.X + hPad * 2,
                                               (int)textSize.Y + vPad * 2);
            var scale = GetScale(_gradient.Bounds.Size, backgroundRect.Size);

            var color = Color.White;
            color.A = TransitionAlpha;

            _screenManager.SpriteBatch.Begin();
            _screenManager.SpriteBatch.Draw(_gradient,
                                            backgroundRect.Location.ToVector2(),
                                            sourceRectangle: null,
                                            color,
                                            rotation: 0,
                                            origin: Vector2.Zero,
                                            scale,
                                            SpriteEffects.None,
                                            layerDepth: 0);

            _screenManager.SpriteBatch.DrawString(_screenManager.SpriteFont, _message, textPosition, color);
            _screenManager.SpriteBatch.End();
        }

        private Vector2 GetScale(Point source, Point target)
        {
            var horizontalScale = target.ToVector2().X / source.X;
            var verticalScale = target.ToVector2().Y / source.Y;

            return new Vector2(horizontalScale, verticalScale);
        }

        public override void HandleInput(Input.InputState state, GameTime gameTime)
        {
            if (state.MenuSelect)
            {
                Accepted?.Invoke(this, EventArgs.Empty);
                ExitScreen();
            }
            else if (state.MenuCancel)
            {
                Cancelled?.Invoke(this, EventArgs.Empty);
                ExitScreen();
            }
        }
    }
}