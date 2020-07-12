using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SweenGame.Screens
{
    public class CreditsScreen : GameScreen
    {
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
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 textPosition;
            int count = 0;

            foreach(var kvp in CreditRoles){
                textPosition = new Vector2(50, 150 + 50 * count);
                count++;
            }
        }
    }
}