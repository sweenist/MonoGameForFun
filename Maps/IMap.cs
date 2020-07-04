using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TestGame.Enums;

namespace TestGame.Maps
{
    public interface IMap : IGameComponent
    {
        event EventHandler ContentLoaded;

        Point MapIndex { get; }
        Point MaxMapIndicies { get; }
        Point MaxMapTileLocation { get; }
        Rectangle Bounds { get; }

        MapTile GetTileAt(Rectangle target);
        void Adjust(Vector2 shift);
        IEnumerable<KeyValuePair<Direction, Point>> GetOpenEdges();
    }
}