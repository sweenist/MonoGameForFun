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
        Point MaxMapIndicies { get; }
        Point MaxMapTileLocation { get; }
        Rectangle Bounds { get; }
        List<MapTile> MapTiles { get; set; }

        MapTile GetTileAt(Rectangle target);
        bool Transition(Vector2 unitShift);
        IEnumerable<KeyValuePair<Direction, Point>> GetOpenEdges();
        void Reset();
    }
}