using System;
using Microsoft.Xna.Framework;

namespace SweenGame.Extensions
{
    public static class MathExtensions
    {
        public static void ClampToNearest(this ref Vector2 vector, int nearest)
        {
            var xMin = (int)Math.Floor(vector.X / nearest) * nearest;
            var yMin = (int)Math.Floor(vector.Y / nearest) * nearest;

            var xMax = (int)Math.Ceiling(vector.X / nearest) * nearest;
            var yMax = (int)Math.Ceiling(vector.Y / nearest) * nearest;

            var x = (vector.X - xMin) > nearest / 2
            ? xMax : xMin;

            var y = (vector.Y - yMin) > nearest / 2
            ? yMax : yMin;

            vector = new Vector2(x, y);
        }

        public static Vector2 Invert(this Vector2 vector)
        {
            return vector * -1;
        }
    }
}