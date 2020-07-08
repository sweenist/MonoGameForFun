using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TiledSharp;

namespace SweenGame.Maps
{
    public class MapTile
    {
        private readonly Dictionary<string, string> _tileProperties;

        public MapTile(Rectangle rectangle, Vector2 location, TmxTilesetTile tile, bool isBorder)
        {
            _tileProperties = tile.Properties;

            Type = tile.Type;
            SourceRectangle = rectangle;
            Location = location;
            IsBorder = isBorder;
        }

        public Rectangle SourceRectangle { get; }
        public Rectangle DestinationRectangle => new Rectangle((int)Location.X, (int)Location.Y, SourceRectangle.Width, SourceRectangle.Height);

        public Vector2 Location { get; private set; }

        public string Type { get; }
        public bool IsTrigger => (Type?.Equals("Trigger")).GetValueOrDefault(false);
        public bool IsBorder { get; }

        public bool IsCollideable => _tileProperties.ContainsKey("Collision")
                                     ? Convert.ToBoolean(_tileProperties["Collision"])
                                     : false;

        public void Adjust(Vector2 shift)
        {
            Location = new Vector2(Location.X + shift.X, Location.Y + shift.Y);
        }

        public override string ToString()
        {
            return "Tile Info:\n"
                   + $"\tTile Location: {Location}\n"
                   + $"\tTile Collide? {IsCollideable}\n"
                   + $"\tTile Destination: {DestinationRectangle}\n";
        }
    }
}