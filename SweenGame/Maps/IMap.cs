using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SweenGame.Enums;
using SweenGame.Maps;

namespace SweenGame.Maps
{
    public interface IMap : IGameComponent
    {
        event EventHandler ContentLoaded;

        Point MapIndex { get; }
        Point MaxMapTileLocation { get; }
        Rectangle Bounds { get; }

        MapTile GetTileAt(Rectangle target);
        bool Transition(Vector2 unitShift);
        IEnumerable<KeyValuePair<Direction, Point>> GetOpenEdges();
    }
}