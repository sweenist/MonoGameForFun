using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace SweenGame.Maps
{
    public class TileBuilder
    {
        private readonly ContentManager _content;
        private TmxTileset _tileSet;
        private TmxLayer _tileLayer;

        public TileBuilder(ContentManager content, string tileFileName)
        {
            _content = content;

            LoadMap(tileFileName);
        }

        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public Point UpperTileBoundPoint {get; private set;}
        public Texture2D TileSetTexture { get; private set; }

        public IEnumerable<MapTile> BuildTileInformation()
        {
            var tileInfo = _tileSet.Tiles.ToDictionary(t => t.Key + 1, v => v.Value);
            var tileColumnCount = GetTileColumnCount();
            var margin = _tileSet.Margin;
            var spacing = _tileSet.Spacing;

            UpperTileBoundPoint = new Point(_tileLayer.Tiles.Max(t => t.X), _tileLayer.Tiles.Max(t => t.Y));

            foreach (var tile in _tileLayer.Tiles)
            {
                var sourceColumn = (tile.Gid - 1) % tileColumnCount;
                var sourceRow = (int)Math.Floor((decimal)(tile.Gid - 1) / tileColumnCount);
                var sourceRect = new Rectangle(TileWidth * sourceColumn + margin + sourceColumn * spacing,
                                               TileHeight * sourceRow + margin + sourceRow * spacing,
                                               TileWidth,
                                               TileHeight);

                var tilePosition = new Vector2(tile.X * TileWidth, tile.Y * TileHeight);

                yield return new MapTile(sourceRect, tilePosition, tileInfo[tile.Gid], IsBorder(tile));
            }
        }

        private void LoadMap(string tileFileName)
        {
            var map = new TmxMap($"SweenGame/Content/Maps/{tileFileName}");

            _tileLayer = map.TileLayers.First();
            _tileSet = map.Tilesets.First();

            TileSetTexture = _content.Load<Texture2D>($"Maps/{_tileSet.Name}");

            TileWidth = _tileSet.TileWidth;
            TileHeight = _tileSet.TileHeight;
        }

        private bool IsBorder(TmxLayerTile tile)
        {
            return tile.X.Equals(0) || tile.Y.Equals(0) || tile.X.Equals(UpperTileBoundPoint.X) || tile.Y.Equals(UpperTileBoundPoint.Y);
        }

        private int GetTileColumnCount()
        {
            var modifiedTextureWidth = TileSetTexture.Width - _tileSet.Margin;
            var modifiedTileWidth = TileWidth + _tileSet.Spacing;

            return modifiedTextureWidth / modifiedTileWidth;
        }
    }
}