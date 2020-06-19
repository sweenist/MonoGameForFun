using Microsoft.Xna.Framework;

namespace TestGame.Maps
{
    public class MapTile
    {
        public MapTile(Rectangle rectangle, Vector2 location, Tile tile)
        {
            SourceRectangle = rectangle;
            Location = location;
            Tile = tile;
            DestinationRectangle = new Rectangle((int)Location.X, (int)Location.Y, SourceRectangle.Width, SourceRectangle.Height);
        }

        public Rectangle SourceRectangle { get; }
        public Rectangle DestinationRectangle { get; }
        public Vector2 Location { get; }
        public Tile Tile { get; }
    }
}