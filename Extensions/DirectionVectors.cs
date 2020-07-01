using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TestGame.Enums;

namespace TestGame.Extensions
{
    public static class DirectionVectors
    {
        private static IDictionary<Direction, Vector2> _vectors = new Dictionary<Direction, Vector2> {
            {Direction.East, new Vector2(1,0)},
            {Direction.South, new Vector2(0,1)},
            {Direction.West, new Vector2(-1,0)},
            {Direction.North, new Vector2(0,-1)}
        };

        public static Vector2 East => _vectors[Direction.East];
        public static Point EastPoint => _vectors[Direction.East].ToPoint();


        public static Vector2 South => _vectors[Direction.South];
        public static Point SouthPoint => _vectors[Direction.South].ToPoint();


        public static Vector2 West => _vectors[Direction.West];
        public static Point WestPoint => _vectors[Direction.West].ToPoint();


        public static Vector2 North => _vectors[Direction.North];
        public static Point NorthPoint => _vectors[Direction.North].ToPoint();

        public static Vector2 GetVector(Direction direction) => _vectors[direction];
        public static Point GetPoint(Direction direction) => _vectors[direction].ToPoint();
    }
}