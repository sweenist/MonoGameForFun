using System.Linq;
using Microsoft.Xna.Framework;

namespace SweenGame.Extensions
{
    public static class GameExtensions
    {
        public static void Update(this GameComponentCollection self, GameTime gameTime)
        {
            foreach(var comp in self)
            {
                if(comp is IUpdateable updateable)
                    updateable.Update(gameTime);
            }
        }

        public static void Draw(this GameComponentCollection self, GameTime gameTime)
        {
            foreach(var comp in self)
            {
                if(comp is IDrawable drawable)
                    drawable.Draw(gameTime);
            }
        }
    }
}