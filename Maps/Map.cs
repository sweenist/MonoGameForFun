using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace TestGame.Maps
{
    public class Map : DrawableGameComponent
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly ContentManager _content;
        private TmxMap _map;
        private Texture2D _tileset;

        private int _tileWidth;
        private int _tileHeight;
        private int _tileColumns;
        private int _tileRows;

        private int _margin;
        private int _spacing;

        private Vector2 ViewPortDimensions;
        private ICamera2D _camera;

        public Map(Game game, SpriteBatch spriteBatch, ContentManager content) : base(game)
        {
            _spriteBatch = spriteBatch;
            _content = content;
            ViewPortDimensions = new Vector2(SweenGame.SCREEN_WIDTH, SweenGame.SCREEN_HEIGHT);
        }

        public List<Tile> TileInfo { get; set; }
        public List<MapTile> MapTiles { get; set; }
        protected override void LoadContent()
        {
            _map = new TmxMap("Content/Maps/blah.tmx");

            var currentSet = _map.Tilesets.First();
            _margin = currentSet.Margin;
            _spacing = currentSet.Spacing;

            _tileset = _content.Load<Texture2D>($"Maps/{currentSet.Name}");

            _tileWidth = currentSet.TileWidth;
            _tileHeight = currentSet.TileHeight;

            _tileColumns = GetTileCountFromDimension(_margin, _spacing, _tileset.Width, _tileWidth);
            _tileRows = GetTileCountFromDimension(_margin, _spacing, _tileset.Height, _tileHeight);

            BuildTileInformation(currentSet, _map.TileLayers.First());

            int GetTileCountFromDimension(int margin, int spacing, int textureDimension, int tileDimension)
            {
                var modifiedTextureDimension = textureDimension - (margin);
                var modifiedTileDimension = tileDimension + spacing;

                return (int)(modifiedTextureDimension / modifiedTileDimension);
            }
        }

        private void BuildTileInformation(TmxTileset tileset, TmxLayer layer)
        {
            var tileInfo = tileset.Tiles.ToDictionary(t => t.Key + 1, v => new Tile(v.Key, v.Value));

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


        public Tile GetTileAt(Rectangle target)
        {
            Console.WriteLine($"Incoming Rect: {target}");
            var tile = MapTiles.Single(tile => tile.DestinationRectangle.Intersects(target));
            Console.WriteLine($"\t{tile.DestinationRectangle}");

            return tile.Tile;
        }

        public void Add(ICamera2D camera) => _camera = camera;
    }
}