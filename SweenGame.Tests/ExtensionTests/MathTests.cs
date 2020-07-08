using Xunit;
using Microsoft.Xna.Framework;
using SweenGame.Extensions;
using System.Collections.Generic;

namespace SweenGame.Tests
{
    public class MathTests
    {
        [Fact]
        public void ShouldInvertVector2()
        {
            var vector = new Vector2(3, -5);
            var result = vector.Invert();

            Assert.Equal(new Vector2(-3, 5), result);
        }

        [Theory]
        [MemberData(nameof(GetClampVectors))]
        public void ClampVector(Vector2 vector, int clampValue, Vector2 expected)
        {
            vector.ClampToNearest(clampValue);

            Assert.Equal(expected, vector);
        }

        public static IEnumerable<object[]> GetClampVectors()
        {
            yield return new object[] { new Vector2(26, 47), 12, new Vector2(24, 48) };
            yield return new object[] { new Vector2(-5, 2), 12, new Vector2(0, 0) };
            yield return new object[] { new Vector2(6, 7), 12, new Vector2(0, 12) };
            yield return new object[] { new Vector2(96, 47), 48, new Vector2(96, 48) };
            yield return new object[] { new Vector2(37, 38), 25, new Vector2(25, 50) };
        }
    }
}
