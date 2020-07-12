using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SweenGame.Screens
{
    public class MessageBoxScreen : GameScreen
    {
        private readonly string _usageText = "Press Enter or Space for Ok"
                                             + $"{Environment.NewLine}Esc Cancels";
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
            
        }
    }
}