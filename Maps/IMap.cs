using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TestGame.Maps
{
    public interface IMap : IGameComponent
    {
        event EventHandler ContentLoaded;
        MapTile GetTileAt(Rectangle target);
        IEnumerable<Point> GetOpenEdges();
    }
}