using System;
using TiledSharp;

namespace TestGame.Maps
{
    public class Tile
    {
        public Tile(int id, TmxTilesetTile tileset)
        {
            Id = id;
            if(tileset.Properties.ContainsKey("Collision"))
            {
                Console.WriteLine($"Tile: Id:{Id}; AltId: {tileset.Id}; Collidable: {tileset.Properties["Collision"]}");
            }
            IsCollideable = tileset.Properties.ContainsKey("Collision")
                ? Convert.ToBoolean(tileset.Properties["Collision"])
                : false;
        }

        public int Id { get; }

        public bool IsCollideable { get; }

        public override string ToString() => $"Id: {Id}; Collide?: {IsCollideable}" ;
    }
}