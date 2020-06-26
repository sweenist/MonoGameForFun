using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TiledSharp;

namespace TestGame.Maps
{
    public class MapTile
    {
        private readonly Dictionary<string, string> _tileProperties;
        public MapTile(Rectangle rectangle, Vector2 location, TmxTilesetTile tile)
        {
            _tileProperties = tile.Properties;

            SourceRectangle = rectangle;
            Location = location;
            DestinationRectangle = new Rectangle((int)Location.X, (int)Location.Y, SourceRectangle.Width, SourceRectangle.Height);
        }

        public Rectangle SourceRectangle { get; }
        public Rectangle DestinationRectangle { get; }
        public Vector2 Location { get; }
        public bool IsCollideable => _tileProperties.ContainsKey("Collision")
                                     ? Convert.ToBoolean(_tileProperties["Collision"])
                                     : false;
    }
}