using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace TestGame
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
        private bool _debugDraw;

        public Map(Game game, SpriteBatch spriteBatch, ContentManager content) : base(game)
        {
            _spriteBatch = spriteBatch;
            _content = content;
            ViewPortDimensions = new Vector2(SweenGame.SCREEN_WIDTH, SweenGame.SCREEN_HEIGHT);
            _debugDraw = true;
        }

        protected override void LoadContent()
        {
            _map = new TmxMap("Content/Maps/blah.tmx");
            Console.WriteLine($"There are {_map.Tilesets.Count} tilesets");

            var currentSet = _map.Tilesets.First();
            _margin = currentSet.Margin;
            _spacing = currentSet.Spacing;

            _tileset = _content.Load<Texture2D>($"Maps/{currentSet.Name}");

            _tileWidth = currentSet.TileWidth;
            _tileHeight = currentSet.TileHeight;

            // TODO: Maybe extract the row and column count from Layer[0]
            _tileColumns = GetTileCountFromDimension(_margin, _spacing, _tileset.Width, _tileWidth);
            _tileRows = GetTileCountFromDimension(_margin, _spacing, _tileset.Height, _tileHeight);

            Console.WriteLine($"There are {_tileColumns} columns and {_tileRows} rows in this map.");

            var layer = _map.TileLayers.First();

            int GetTileCountFromDimension(int margin, int spacing, int textureDimension, int tileDimension)
            {
                var modifiedTextureDimension = textureDimension - (margin);
                var modifiedTileDimension = tileDimension + spacing;

                return (int)(modifiedTextureDimension / modifiedTileDimension);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            var layer = _map.TileLayers.First();
            foreach (var tile in layer.Tiles)
            {
                var sourceColumn = (tile.Gid - 1) % _tileColumns;
                var sourceRow = (int)Math.Floor((decimal)(tile.Gid - 1) / _tileColumns);
                //TODO load tiles in loadContent
                var sourceRect = new Rectangle((_tileWidth * sourceColumn) + _margin + (sourceColumn * _spacing),
                                               (_tileHeight * sourceRow) + _margin + (sourceRow * _spacing),
                                               _tileWidth,
                                               _tileHeight);

                var tilePosition = new Vector2(tile.X * _tileWidth, tile.Y * _tileHeight);

                if (_debugDraw)
                {
                    Console.WriteLine("Tile Properties");
                    Console.WriteLine($"\tTile Id {tile.Gid}");

                    Console.WriteLine($"\tTile Column?: {sourceColumn}\tTile Row?: {sourceRow}");
                    Console.WriteLine($"\tTile rectangle?: {sourceRect}");
                }
                _spriteBatch.Draw(_tileset, tilePosition, sourceRect, Color.White);
            }
            _debugDraw = false;

            _spriteBatch.End();
        }
    }
}