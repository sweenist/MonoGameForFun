using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TestGame.Enums;
using TestGame.Maps;

namespace TestGame
{
    public class MovementManager : GameComponent
    {
        const int MOVE_INCREMENT = 4;
        private Player _player;
        private Map _map;
        private GameWindow _window;
        private bool _printDebug;

        public MovementManager(Game game) : base(game)
        {
            _window = game.Window;
        }

        public void Add(Player player) => _player = player;
        public void Add(Map map) => _map = map;

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                UpdateMovement(Direction.East);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                UpdateMovement(Direction.West);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                UpdateMovement(Direction.North);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                UpdateMovement(Direction.South);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.M))
                _printDebug = true;
            if(_printDebug && Keyboard.GetState().IsKeyUp(Keys.M))
            {
                _printDebug = false;
                var tile = _map.GetTileAt(GetAdjacentTileInDirection(_player.CurrentDirection), true);

                Console.WriteLine($"Player Position: {_player.Destination}");
                Console.WriteLine($"\tTile Info: {tile}");
                Console.WriteLine($"\tMap Offset: {_map.Offset}");
                Console.WriteLine($"\tMap Bounds: {_map.Bounds}");
                Console.WriteLine();

            }
            base.Update(gameTime);
        }

        private void UpdateMovement(Direction direction)
        {
            if (_player.IsMoving || _map.IsScrolling)
                return;

            _player.CurrentDirection = direction;

            var targetTile = _map.GetTileAt(GetAdjacentTileInDirection(direction));
            var canMove = !targetTile.IsCollideable;
            if (canMove)
                SetupMovementBounds(direction);
        }

        private Rectangle GetAdjacentTileInDirection(Direction direction)
        {
            Rectangle targetRect = Rectangle.Empty;
            switch (direction)
            {
                case Direction.East:
                    targetRect = GetTargetTileSpace(deltaX: _player.Width);
                    break;
                case Direction.South:
                    targetRect = GetTargetTileSpace(deltaY: _player.Height);
                    break;
                case Direction.West:
                    targetRect = GetTargetTileSpace(deltaX: -(_player.Width));
                    break;
                case Direction.North:
                    targetRect = GetTargetTileSpace(deltaY: -(_player.Height));
                    break;
            }

            return targetRect;
        }

        private Rectangle GetTargetTileSpace(int deltaX = 0, int deltaY = 0)
        {
            return new Rectangle(_player.Destination.X + deltaX + (int)_map.Offset.X,
                                 _player.Destination.Y + deltaY + (int)_map.Offset.Y,
                                 _player.Width,
                                 _player.Height);
        }

        private void SetupMovementBounds(Direction direction)
        {
            Func<bool> playerMapMarginCheck = () => false;
            Func<bool> mapSpaceRemaining = () => false;
            Func<bool> screenEdgeCheck = () => false;
            Vector2 moveVector = Vector2.Zero;

            switch (direction)
            {
                case Direction.East:
                    playerMapMarginCheck = () => _player.Destination.X >= _window.ClientBounds.Width - _player.Width * 3;
                    mapSpaceRemaining = () => _map.Bounds.Width + _map.Offset.X > _window.ClientBounds.Width;
                    screenEdgeCheck = () => _player.Destination.X.Equals(_window.ClientBounds.Width - _player.Width);
                    moveVector = new Vector2(MOVE_INCREMENT, 0);
                    break;

                case Direction.South:
                    playerMapMarginCheck = () => _player.Destination.Y >= _window.ClientBounds.Height - _player.Height * 3;
                    mapSpaceRemaining = () => _map.Bounds.Height + _map.Offset.Y > _window.ClientBounds.Height;
                    screenEdgeCheck = () => _player.Destination.Y.Equals(_window.ClientBounds.Height - _player.Height);
                    moveVector = new Vector2(0, MOVE_INCREMENT);
                    break;

                case Direction.West:
                    playerMapMarginCheck = () => _player.Destination.X == _player.Width * 3;
                    mapSpaceRemaining = () => _map.Bounds.X < 0;
                    screenEdgeCheck = () => _player.Destination.X == 0;
                    moveVector = new Vector2(-MOVE_INCREMENT, 0);
                    break;

                case Direction.North:
                    playerMapMarginCheck = () => _player.Destination.Y == _player.Height * 3;
                    mapSpaceRemaining = () => _map.Bounds.Y < 0;
                    screenEdgeCheck = () => _player.Destination.Y == 0;
                    moveVector = new Vector2(0, -MOVE_INCREMENT);
                    break;
            }

            if (playerMapMarginCheck() && mapSpaceRemaining())
            {
                _map.MoveVector = -moveVector;
                _map.IsScrolling = true;
            }
            else if (!screenEdgeCheck())
            {
                _player.MoveVector = moveVector;
                _player.IsMoving = true;
            }
        }
    }
}