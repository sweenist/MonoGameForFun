using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TestGame.Enums;
using TestGame.Maps;
using TestGame.Services;

namespace TestGame
{
    public partial class MovementManager : GameComponent
    {
        public MovementManager(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                MovementData.Create().Move(Direction.East);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                MovementData.Create().Move(Direction.West);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                MovementData.Create().Move(Direction.North);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                MovementData.Create().Move(Direction.South);
            }

            base.Update(gameTime);
        }
    }
}