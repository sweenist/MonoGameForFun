﻿using System;
using Microsoft.Xna.Framework;
using SweenGame.Enums;
using SweenGame.Maps;
using SweenGame.Services;
using static SweenGame.Extensions.Constants;

namespace SweenGame
{
    public static class MovementFunctoid
    {
        public static MovementData Move(this MovementData data, Direction direction)
        {
            var mapManager = ServiceLocator.Instance.GetService<IMapManager>();

            if (mapManager.IsInTransition)
                return data;

            if (!data.Player.IsMoving)
            {
                data.Continue = true;
                data.Direction = direction;

                return data.TurnPlayer()
                           .CheckBorder()
                           .CheckPlayerCollisions();
            }

            return data;
        }

        private static MovementData TurnPlayer(this MovementData data)
        {
            if (!data.Continue)
                return data;

            data.Player.CurrentDirection = data.Direction;
            return data;
        }

        private static MovementData CheckBorder(this MovementData data)
        {
            if (!data.Continue)
                return data;

            var currentTile = data.Map.GetTileAt(data.Player.Destination);
            if (currentTile.IsBorder)
            {
                switch (data.Direction)
                {
                    case Direction.East:
                        if (data.Player.Destination.X.Equals(data.Map.MaxMapTileLocation.X))
                            Transition();
                        return data;

                    case Direction.West:
                        if (data.Player.Destination.X.Equals(0))
                            Transition();
                        return data;

                    case Direction.South:
                        if (data.Player.Destination.Y.Equals(data.Map.MaxMapTileLocation.Y))
                            Transition();
                        return data;

                    case Direction.North:
                        if (data.Player.Destination.Y.Equals(0))
                            Transition();
                        return data;
                }
            }
            return data;

            void Transition()
            {
                var mapManager = ServiceLocator.Instance.GetService<IMapManager>();
                mapManager.SlideTransition(data.Direction);
                data.Continue = false;
            }
        }

        private static MovementData CheckPlayerCollisions(this MovementData data)
        {
            if (!data.Continue)
                return data;

            Rectangle targetRect = Rectangle.Empty;

            switch (data.Direction)
            {
                case Direction.East:
                    targetRect = GetTargetTileSpace(deltaX: data.Player.Width);
                    data.Player.MoveVector = new Vector2(MoveIncrement, 0);
                    break;
                case Direction.South:
                    targetRect = GetTargetTileSpace(deltaY: data.Player.Height);
                    data.Player.MoveVector = new Vector2(0, MoveIncrement);
                    break;
                case Direction.West:
                    targetRect = GetTargetTileSpace(deltaX: -(data.Player.Width));
                    data.Player.MoveVector = new Vector2(-MoveIncrement, 0);
                    break;
                case Direction.North:
                    targetRect = GetTargetTileSpace(deltaY: -(data.Player.Height));
                    data.Player.MoveVector = new Vector2(0, -MoveIncrement);
                    break;
            }

            var targetTile = data.Map.GetTileAt(targetRect);
            data.Player.IsMoving = !targetTile.IsCollideable;
            if(targetTile.IsTrigger)
                Console.WriteLine("Trigger!"); // Action Manager?

            return data;

            Rectangle GetTargetTileSpace(int deltaX = 0, int deltaY = 0)
            {
                return new Rectangle(data.Player.Destination.X + deltaX,
                                     data.Player.Destination.Y + deltaY,
                                     data.Player.Width,
                                     data.Player.Height);
            }
        }
    }
}