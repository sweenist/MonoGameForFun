using Microsoft.Xna.Framework;
using TestGame.Enums;
using static TestGame.Constants;

namespace TestGame
{
    public static class MovementChain
    {
        public static MovementToken Start(this MovementToken token, Direction direction)
        {
            token.Continue = !token.Player.IsMoving;
            token.Direction = direction;

            return token;
        }

        public static MovementToken CheckBorder(this MovementToken token)
        {
            if (!token.Continue)
                return token;

            if (token.Map.GetTileAt(token.Player.Destination).IsBorder)
            {
                token.Continue = false;

            }
            return token;
        }

        public static MovementToken CheckPlayerCollisions(this MovementToken token)
        {
            if (!token.Continue)
                return token;

            token.Player.CurrentDirection = token.Direction;
            Rectangle targetRect = Rectangle.Empty;

            switch (token.Direction)
            {
                case Direction.East:
                    targetRect = GetTargetTileSpace(deltaX: token.Player.Width);
                    token.Player.MoveVector = new Vector2(MoveIncrement, 0);
                    break;
                case Direction.South:
                    targetRect = GetTargetTileSpace(deltaY: token.Player.Height);
                    token.Player.MoveVector = new Vector2(0, MoveIncrement);
                    break;
                case Direction.West:
                    targetRect = GetTargetTileSpace(deltaX: -(token.Player.Width));
                    token.Player.MoveVector = new Vector2(-MoveIncrement, 0);
                    break;
                case Direction.North:
                    targetRect = GetTargetTileSpace(deltaY: -(token.Player.Height));
                    token.Player.MoveVector = new Vector2(0, -MoveIncrement);
                    break;
            }

            var targetTile = token.Map.GetTileAt(targetRect);
            token.Player.IsMoving = !targetTile.IsCollideable;

            return token;

            Rectangle GetTargetTileSpace(int deltaX = 0, int deltaY = 0)
            {
                return new Rectangle(token.Player.Destination.X + deltaX,
                                     token.Player.Destination.Y + deltaY,
                                     token.Player.Width,
                                     token.Player.Height);
            }
        }
    }
}