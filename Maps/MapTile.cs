using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace TestGame.Maps
{
    public class MapTile : IDisposable
    {
        private readonly Map _map;

        public MapTile(Map map, Rectangle rectangle, Vector2 location, Tile tile)
        {
            SourceRectangle = rectangle;
            Location = location;
            Tile = tile;
            
            _map = map;
            _map.PropertyChanged += OnMapPropertyChanged;
        }

        public Rectangle SourceRectangle { get; }
        public Vector2 Location { get; private set;}
        public Tile Tile { get; }
        public Rectangle DestinationRectangle => new Rectangle((int)Location.X,
                                                                   (int)Location.Y,
                                                                   SourceRectangle.Width,
                                                                   SourceRectangle.Height);

        public void OnMapPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals(nameof(Map.Offset)))
                Location += _map.Offset;
        }

        public void Dispose()
        {
            _map.PropertyChanged -= OnMapPropertyChanged;
        }
    }
}