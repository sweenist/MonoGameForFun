using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TestGame.Enums;
using TestGame.Maps;
using TestGame.Services;

namespace TestGame
{
    public partial class MovementManager : GameComponent
    {
        private MovementData _chainToken;

        public MovementManager(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            var player = ServiceLocator.Instance.GetService<IPlayer>();
            var map = ServiceLocator.Instance.GetService<IMap>();
            _chainToken = new MovementData(player, map);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _chainToken.Move(Direction.East);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _chainToken.Move(Direction.West);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _chainToken.Move(Direction.North);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                _chainToken.Move(Direction.South);
            }

            base.Update(gameTime);
        }
    }
}