using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SweenGame.Enums;
using SweenGame.Camera;
using SweenGame.Extensions;
using SweenGame.Services;
using System;

namespace SweenGame.Maps
{
    public class Map : DrawableGameComponent, IMap
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly ContentManager _content;
        private readonly MapType _mapType;
        private Texture2D _tileSource;
        private ICamera2D _camera;
        private Vector2 _tileOffsetVector = Vector2.Zero;
        private Point _upperMapBoundIndex;

        private int _tileWidth;
        private int _tileHeight;

        public Map(Game game, Point mapIndex, MapType mapType, Vector2 tileOffset) : this(game, mapIndex, mapType)
        {
            _tileOffsetVector = tileOffset;
        }

        public Map(Game game, Point mapIndex, MapType mapType) : base(game)
        {
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
            _content = game.Content;
            MapIndex = mapIndex;
            _mapType = mapType;
        }

        public Point MapIndex { get; }
        public Point MaxMapTileLocation => _upperMapBoundIndex * new Point(_tileWidth, _tileHeight);
        public Rectangle Bounds => new Rectangle((int)_tileOffsetVector.X, (int)_tileOffsetVector.Y, MaxMapTileLocation.X + _tileWidth, MaxMapTileLocation.Y + _tileHeight);

        public List<MapTile> MapTiles { get; set; }

        private string MapFileName => $"{_mapType}_{MapIndex.X}_{MapIndex.Y}.tmx";
        private ICamera2D Camera => _camera ?? ( _camera = ServiceLocator.Instance.GetService<ICamera2D>());

        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            using (var tileBuilder = new TileBuilder(_content, MapFileName))
            {
                MapTiles = tileBuilder.BuildTileInformation().ToList();
                _tileWidth = tileBuilder.TileWidth;
                _tileHeight = tileBuilder.TileHeight;
                _tileSource = tileBuilder.TileSetTexture;
                _upperMapBoundIndex = tileBuilder.UpperTileBoundPoint;
            }
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var firstTile = MapTiles.First();
            var lastTile = MapTiles.Last();
            var upperBound = new Point((int)lastTile.Location.X + lastTile.SourceRectangle.Width,
                                       (int)lastTile.Location.Y + lastTile.SourceRectangle.Height);

            Camera.ClampCamera(new Rectangle(firstTile.Location.ToPoint(), upperBound));
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(transformMatrix: Camera.Transform);

            foreach (var tile in MapTiles)
            {
                var modifiedLocation = tile.Location + Bounds.Location.ToVector2();
                if (Camera.IsInView(modifiedLocation, tile.SourceRectangle))
                    _spriteBatch.Draw(_tileSource, modifiedLocation, tile.SourceRectangle, Color.White);
            }

            _spriteBatch.End();
        }

        public MapTile GetTileAt(Rectangle target)
        {
            var tile = MapTiles.Single(tile => tile.DestinationRectangle.Intersects(target));
            return tile;
        }

        public IEnumerable<KeyValuePair<Direction, Point>> GetOpenEdges()
        {
            var borderTiles = MapTiles.Where(tile => !tile.IsCollideable && tile.IsBorder).ToList();

            if (borderTiles.Any(tile => tile.DestinationRectangle.X.Equals(0)))
                yield return new KeyValuePair<Direction, Point>(Direction.West, MapIndex + DirectionVectors.WestPoint);
            if (borderTiles.Any(tile => tile.DestinationRectangle.Y.Equals(0)))
                yield return new KeyValuePair<Direction, Point>(Direction.North, MapIndex + DirectionVectors.NorthPoint);
            if (borderTiles.Any(tile => tile.DestinationRectangle.X.Equals(MaxMapTileLocation.X)))
                yield return new KeyValuePair<Direction, Point>(Direction.East, MapIndex + DirectionVectors.EastPoint);
            if (borderTiles.Any(tile => tile.DestinationRectangle.Y.Equals(MaxMapTileLocation.Y)))
                yield return new KeyValuePair<Direction, Point>(Direction.South, MapIndex + DirectionVectors.SouthPoint);
        }

        public bool Transition(Vector2 unitShift)
        {
            var shift = unitShift * new Vector2(_tileWidth / 3, _tileHeight / 4);
            _tileOffsetVector -= shift.Invert();

            return _tileOffsetVector == Vector2.Zero;
        }

        public override string ToString()
        {
            return "Map:\n"
            + $"\tBounds: {Bounds}\n"
            + $"\tOffset Vector: {_tileOffsetVector}\n";
        }
    }
}