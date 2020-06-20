using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TestGame.Enums;
using TestGame.Maps;

namespace TestGame
{
    public class MovementManager : GameComponent
    {
        private Player _player;
        private Map _map;

        public MovementManager(Game game) : base(game)
        {
        }

        public void Add(Player player) => _player = player;
        public void Add(Map map) => _map = map;

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                CheckPlayerCollisions(Direction.East);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                CheckPlayerCollisions(Direction.West);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                CheckPlayerCollisions(Direction.North);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                CheckPlayerCollisions(Direction.South);
            }

            base.Update(gameTime);
        }

        private void CheckPlayerCollisions(Direction direction)
        {
            if (_player.IsMoving)
                return;

            _player.CurrentDirection = direction;
            Rectangle targetRect = Rectangle.Empty;

            switch (direction)
            {
                case Direction.East:
                    targetRect = GetTargetTileSpace(deltaX: _player.Width);
                    _player.MoveVector = new Vector2(1, 0);
                    break;
                case Direction.South:
                    targetRect = GetTargetTileSpace(deltaY: _player.Height);
                    _player.MoveVector = new Vector2(0, 1);
                    break;
                case Direction.West:
                    targetRect = GetTargetTileSpace(deltaX: -(_player.Width));
                    _player.MoveVector = new Vector2(-1, 0);
                    break;
                case Direction.North:
                    targetRect = GetTargetTileSpace(deltaY: -(_player.Height));
                    _player.MoveVector = new Vector2(0, -1);
                    break;
            }

            var targetTile = _map.GetTileAt(targetRect);
            _player.IsMoving = !targetTile.IsCollideable;

            Rectangle GetTargetTileSpace(int deltaX = 0, int deltaY = 0)
            {
                return new Rectangle(_player.Destination.X + deltaX,
                                     _player.Destination.Y + deltaY,
                                     _player.Width,
                                     _player.Height);
            }
        }
    }
}