using Microsoft.Xna.Framework;
using TestGame.Enums;
using static TestGame.Constants;

namespace TestGame
{
    public static class MovementChain
    {
        public static MovementData Move(this MovementData token, Direction direction)
        {
            token.Continue = !token.Player.IsMoving;
            token.Direction = direction;

            return token.TurnPlayer()
                        .CheckBorder()
                        .CheckPlayerCollisions();
        }

        private static MovementData TurnPlayer(this MovementData data)
        {
            if (!data.Continue)
                return data;

            data.Player.CurrentDirection = data.Direction;
            return data;
        }

        private static MovementData CheckBorder(this MovementData token)
        {
            if (!token.Continue)
                return token;

            var currentTile = token.Map.GetTileAt(token.Player.Destination);
            if (currentTile.IsBorder)
            {
                switch (token.Direction)
                {
                    case Direction.East:
                        if (token.Player.Destination.X.Equals(token.Map.MaxMapTileLocation.X))
                            token.Continue = false;
                        return token;

                    case Direction.West:
                        if (token.Player.Destination.X.Equals(0))
                            token.Continue = false;
                        return token;

                    case Direction.South:
                        if (token.Player.Destination.Y.Equals(token.Map.MaxMapTileLocation.Y))
                            token.Continue = false;
                        return token;

                    case Direction.North:
                        if (token.Player.Destination.Y.Equals(0))
                            token.Continue = false;
                        return token;
                }

            }
            return token;
        }

        private static MovementData CheckPlayerCollisions(this MovementData token)
        {
            if (!token.Continue)
                return token;

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