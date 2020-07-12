using Microsoft.Xna.Framework;

namespace SweenGame.Extensions
{
    public static class Constants
    {
        public static int MoveIncrement = 4;
        public static int AnimationDelay = 5;
        public static int TotalStepsPerTile = 48;
        public static int ScreenWidth = 912;
        public static int ScreenHeight = 624;

        public static string Current = "current";
        public static string CancelMessage = "Are you sure yo want to quit this game?";
        public static string LoadingMessage = "Loading...";
        public static string CancelActionPrompt = "Press <ESC> to exit";

        public static Vector2 ScreenDimensions = new Vector2(19, 13);
    }
}