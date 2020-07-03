using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TiledSharp;

namespace TestGame.Maps
{
    public class MapTile
    {
        private readonly Dictionary<string, string> _tileProperties;

        public MapTile(Rectangle rectangle, Vector2 location, TmxTilesetTile tile, bool isBorder)
        {
            _tileProperties = tile.Properties;

            SourceRectangle = rectangle;
            Location = location;
            IsBorder = isBorder;
            DestinationRectangle = new Rectangle((int)Location.X, (int)Location.Y, SourceRectangle.Width, SourceRectangle.Height);
        }

        public Rectangle SourceRectangle { get; }
        public Rectangle DestinationRectangle { get; }

        public Vector2 Location { get; private set;}
        public bool IsBorder { get; }

        public bool IsCollideable => _tileProperties.ContainsKey("Collision")
                                     ? Convert.ToBoolean(_tileProperties["Collision"])
                                     : false;

        public void Adjust(Vector2 shift)
        {
            Location = new Vector2(Location.X + shift.X, Location.Y + shift.Y);
        }
    }
}