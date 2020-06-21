using System;
using TiledSharp;

namespace TestGame.Maps
{
    public class Tile
    {
        public Tile(int id, TmxTilesetTile tileset)
        {
            Id = id;
            Name = tileset.Properties.ContainsKey("Name")
                ? tileset.Properties["Name"]
                : "()null)";
            IsCollideable = tileset.Properties.ContainsKey("Collision")
                ? Convert.ToBoolean(tileset.Properties["Collision"])
                : false;
        }

        public int Id { get; }
        public string Name{get;}
        public bool IsCollideable { get; }

        public override string ToString() => $"Id: {Id}; Name: {Name}; Collide?: {IsCollideable}" ;
    }
}