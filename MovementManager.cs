using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TestGame.Enums;
using TestGame.Maps;
using TestGame.Services;

namespace TestGame
{
    public partial class MovementManager : GameComponent
    {
        private MovementToken _chainToken;

        public MovementManager(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            var player = ServiceLocator.Instance.GetService<IPlayer>();
            var map = ServiceLocator.Instance.GetService<IMap>();
            _chainToken = new MovementToken(player, map);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _chainToken.Start(Direction.East)
                           .CheckBorder()
                           .CheckPlayerCollisions();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _chainToken.Start(Direction.West)
                           .CheckBorder()
                           .CheckPlayerCollisions();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _chainToken.Start(Direction.North)
                           .CheckBorder()
                           .CheckPlayerCollisions();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                _chainToken.Start(Direction.South)
                           .CheckBorder()
                           .CheckPlayerCollisions();
            }

            base.Update(gameTime);
        }
    }
}