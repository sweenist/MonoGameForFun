using System;
using Microsoft.Xna.Framework;
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

        public void Add(Player player)
        {
            _player = player;
            _player.HandlePlayerMovement += OnPlayerMove;
        }


        public void Add(Map map)
        {
            _map = map;
        }

        public override void Update(GameTime gameTime)
        {

        }

        private void OnPlayerMove(object sender, MovementEventArgs e)
        {
            var player = (sender as Player);
            if (!(player is null))
            {
                var currentPlayerSpace = player.Destination;
                Rectangle targetRect = Rectangle.Empty;
                switch (e.Direction)
                {
                    case Direction.East:
                        targetRect = GetTargetTileSpace(deltaX: player.Width);
                        break;
                    case Direction.South:
                        targetRect = GetTargetTileSpace(deltaY: player.Height);
                        break;
                    case Direction.West:
                        targetRect = GetTargetTileSpace(deltaX: -player.Width);
                        break;
                    case Direction.North:
                        targetRect = GetTargetTileSpace(deltaY: -player.Height);
                        break;
                }
                var targetTile = _map.GetTileAt(targetRect);
                player.IsMoving = !targetTile.IsCollideable;

                Console.WriteLine($"{targetTile}");

                Rectangle GetTargetTileSpace(int deltaX = 0, int deltaY = 0)
                {
                    return new Rectangle(currentPlayerSpace.X + deltaX,
                                         currentPlayerSpace.Y + deltaY,
                                         player.Width,
                                         player.Height);
                }
            }
        }
    }
}