using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace TestGame.Maps
{
    public class Map : DrawableGameComponent, INotifyPropertyChanged
    {
        const int SCROLL_AMOUNT = 4;
        const int SCROLL_INCREMENT = 4;
        private readonly SpriteBatch _spriteBatch;
        private readonly ContentManager _content;
        private TmxMap _map;
        private Texture2D _tileset;
        private Vector2 _offset = Vector2.Zero;

        private int _tileWidth;
        private int _tileHeight;
        private int _tileColumns;
        private int _tileRows;

        private int _margin;
        private int _spacing;
        private int _remainingScroll;
        private bool _isScrolling;

        private Vector2 ViewPortDimensions;

        public Map(Game game, SpriteBatch spriteBatch, ContentManager content) : base(game)
        {
            _spriteBatch = spriteBatch;
            _content = content;
            ViewPortDimensions = new Vector2(SweenGame.SCREEN_WIDTH, SweenGame.SCREEN_HEIGHT);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public List<Tile> TileInfo { get; set; }
        public List<MapTile> MapTiles { get; set; }
        public Vector2 Offset
        {
            get => _offset;
            set
            {
                if (_offset != value)
                {
                    _offset = value;
                    if(!_isScrolling)
                        RaisePropertyChange();
                }
            }
        }

        public Rectangle Bounds => new Rectangle(Offset.ToPoint(), new Point(Width, Height));

        public bool IsScrolling
        {
            get => _isScrolling;
            set
            {
                if (value)
                    _remainingScroll = _tileWidth;
                _isScrolling = value;
            }
        }
        public Vector2 MoveVector { get; set; } = Vector2.Zero;

        protected override void LoadContent()
        {
            _map = new TmxMap("Content/Maps/blah.tmx");

            Width = _map.Width * _map.TileWidth;
            Height = _map.Height * _map.TileHeight;

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

                return new MapTile(this, sourceRect, tilePosition, tileInfo[tile.Gid]);
            }).ToList();
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsScrolling)
                return;

            _remainingScroll -= SCROLL_INCREMENT;
            if (_remainingScroll.Equals(0))
                IsScrolling = false;
                
            Offset += MoveVector;
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            var debug = IsScrolling;
            foreach (var tile in MapTiles)
            {
                if (debug)
                {
                    Console.WriteLine($"Tile Location: {tile.Location}");
                    Console.WriteLine($"Map Offset: {Offset}");
                    Console.WriteLine($"Map Bounds: {Bounds}");
                    Console.WriteLine();
                    debug = false;
                }
                _spriteBatch.Draw(_tileset, tile.Location, tile.SourceRectangle, Color.White);
            }

            _spriteBatch.End();
        }

        public Tile GetTileAt(Rectangle target)
        {
            var tile = MapTiles.Single(tile => tile.DestinationRectangle.Intersects(target));
            Console.WriteLine(tile.Tile.ToString());
            return tile.Tile;
        }

        private void RaisePropertyChange([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}