using Microsoft.Xna.Framework;

namespace TestGame.Maps
{
    public interface IMap : IGameComponent
    {
        Tile GetTileAt(Rectangle target);
    }
}