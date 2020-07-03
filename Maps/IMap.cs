using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

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
        IEnumerable<Point> GetOpenEdges();
        void Adjust(Vector2 shift);
    }
}