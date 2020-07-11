using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using static SweenGame.Extensions.Constants;

namespace SweenGame.Helpers
{
    public static class GraphicsHelpers
    {
        public static void TakeScreenShot(GraphicsDevice graphicsDevice, string filename)
        {
            var w = ScreenWidth;
            var h = ScreenHeight;
            var colors = new Color[w * h];
            graphicsDevice.GetBackBufferData<Color>(colors);

            using (var texture = new Texture2D(graphicsDevice, w, 48))
            {
                texture.SetData<Color>(colors.Take(w * 48).ToArray());
                using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    texture.SaveAsJpeg(fs, w, 48);
                }
            }
        }

        public static Texture2D GetViewportMap(GraphicsDevice graphics)
        {
            var colorData = new Color[ScreenWidth * ScreenHeight];
            graphics.GetBackBufferData<Color>(colorData);

            var texture = new Texture2D(graphics, ScreenWidth, ScreenHeight);
            texture.SetData<Color>(colorData);
            return texture;
        }
    }
}