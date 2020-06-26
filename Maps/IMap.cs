using Microsoft.Xna.Framework;

namespace TestGame.Maps
{
    public interface IMap : IGameComponent
    {
        MapTile GetTileAt(Rectangle target);
    }
}