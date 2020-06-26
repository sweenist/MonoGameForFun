using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Camera;
using TestGame.Enums;
using TestGame.Services;
using TiledSharp;

namespace TestGame.Maps
{
    public class Map : DrawableGameComponent, IMap
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly ContentManager _content;
        private readonly Point _mapIndex;
        private readonly string _tmxFilename;

        private TmxMap _map;
        private Texture2D _tileset;
        private ICamera2D _camera;

        private int _tileWidth;
        private int _tileHeight;
        private int _tileColumns;
        private int _tileRows;

        private int _margin;
        private int _spacing;

        public Map(Game game, Point mapIndex) : base(game)
        {
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
            _content = game.Content;
            _mapIndex = mapIndex;
            _tmxFilename = $"Overworld_{_mapIndex.X}_{_mapIndex.Y}.tmx";
        }

        public event EventHandler ContentLoaded;

        public List<MapTile> MapTiles { get; set; }

        public override void Initialize()
        {
            _camera = ServiceLocator.Instance.GetService<ICamera2D>();

            base.Initialize();
        }
        protected override void LoadContent()
        {
            _map = new TmxMap($"Content/Maps/{_tmxFilename}");

            var currentSet = _map.Tilesets.First();
            _margin = currentSet.Margin;
            _spacing = currentSet.Spacing;

            _tileset = _content.Load<Texture2D>($"Maps/{currentSet.Name}");

            _tileWidth = currentSet.TileWidth;
            _tileHeight = currentSet.TileHeight;

            _tileColumns = GetTileCountFromDimension(_margin, _spacing, _tileset.Width, _tileWidth);
            _tileRows = GetTileCountFromDimension(_margin, _spacing, _tileset.Height, _tileHeight);

            BuildTileInformation(currentSet, _map.TileLayers.First());
            ContentLoaded?.Invoke(this, new EventArgs());

            int GetTileCountFromDimension(int margin, int spacing, int textureDimension, int tileDimension)
            {
                var modifiedTextureDimension = textureDimension - (margin);
                var modifiedTileDimension = tileDimension + spacing;

                return (int)(modifiedTextureDimension / modifiedTileDimension);
            }
        }

        public override void Update(GameTime gameTime)
        {
            var firstTile = MapTiles.First();
            var lastTile = MapTiles.Last();
            var upperBound = new Point((int)lastTile.Location.X + lastTile.SourceRectangle.Width,
                                       (int)lastTile.Location.Y + lastTile.SourceRectangle.Height);

            _camera.ClampCamera(new Rectangle(firstTile.Location.ToPoint(), upperBound));
        }

        private void BuildTileInformation(TmxTileset tileset, TmxLayer layer)
        {
            var tileInfo = tileset.Tiles.ToDictionary(t => t.Key + 1, v => v.Value);

            MapTiles = layer.Tiles.Select(tile =>
            {
                var sourceColumn = (tile.Gid - 1) % _tileColumns;
                var sourceRow = (int)Math.Floor((decimal)(tile.Gid - 1) / _tileColumns);
                var sourceRect = new Rectangle((_tileWidth * sourceColumn) + _margin + (sourceColumn * _spacing),
                                               (_tileHeight * sourceRow) + _margin + (sourceRow * _spacing),
                                               _tileWidth,
                                               _tileHeight);

                var tilePosition = new Vector2(tile.X * _tileWidth, tile.Y * _tileHeight);

                return new MapTile(sourceRect, tilePosition, tileInfo[tile.Gid]);
            }).ToList();
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(transformMatrix: _camera.Transform);

            foreach (var tile in MapTiles)
            {
                _spriteBatch.Draw(_tileset, tile.Location, tile.SourceRectangle, Color.White);
            }

            _spriteBatch.End();
        }

        public MapTile GetTileAt(Rectangle target)
        {
            var tile = MapTiles.Single(tile => tile.DestinationRectangle.Intersects(target));
            return tile;
        }

        public IEnumerable<Point> GetOpenEdges()
        {
            var maxLocation = MapTiles.Last().DestinationRectangle.Location;

            var borderTiles = MapTiles.Where(tile =>
            {
                var rect = tile.DestinationRectangle;
                return !tile.IsCollideable
                    && (rect.X.Equals(0)
                    || rect.Y.Equals(0)
                    || rect.X.Equals(maxLocation.X)
                    || rect.Y.Equals(maxLocation.Y));
            })
            .ToList();

            if (borderTiles.Any(tile => tile.DestinationRectangle.X.Equals(0)))
                yield return new Point(_mapIndex.X - 1, _mapIndex.Y);
            if (borderTiles.Any(tile => tile.DestinationRectangle.Y.Equals(0)))
                yield return new Point(_mapIndex.X, _mapIndex.Y - 1);
            if (borderTiles.Any(tile => tile.DestinationRectangle.X.Equals(maxLocation.X)))
                yield return new Point(_mapIndex.X + 1, _mapIndex.Y);
            if (borderTiles.Any(tile => tile.DestinationRectangle.Y.Equals(maxLocation.Y)))
                yield return new Point(_mapIndex.X, _mapIndex.Y + 1);
        }
    }
}