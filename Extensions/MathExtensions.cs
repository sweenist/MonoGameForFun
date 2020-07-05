using System;
using Microsoft.Xna.Framework;

namespace SweenGame.Extensions
{
    public static class MathExtensions
    {
        public static void ClampToNearest(this Vector2 vector, int nearest)
        {
            var xMin = (int)Math.Floor(vector.X / nearest) * nearest;
            var yMin = (int)Math.Floor(vector.Y / nearest) * nearest;

            var xMax = (int)Math.Ceiling(vector.X / nearest) * nearest;
            var yMax = (int)Math.Ceiling(vector.Y / nearest) * nearest;


            vector = new Vector2(Math.Clamp(vector.X, xMin, xMax), Math.Clamp(vector.Y, yMin, yMax));
        }

        public static Vector2 Invert(this Vector2 vector)
        {
            return vector * -1;
        }
    }
}