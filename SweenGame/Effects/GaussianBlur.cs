using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SweenGame.Effects
{
    public class GaussianBlur : IDisposable
    {
        private readonly Game _game;
        private Effect effect;

        public GaussianBlur(Game game)
        {
            _game = game;
            effect = game.Content.Load<Effect>("Shaders/GaussianBlur");
        }

        public int Radius { get; set; }

        public float Amount { get; set; }
        public float Sigma { get; set; }
        public float[] Kernel { get; set; }
        public Vector2[] TextureOffsetsX { get; set; }
        public Vector2[] TextureOffsetsY { get; set; }

        public void ComputerKernel(int blurRadius, float blurAmount)
        {
            Radius = blurRadius;
            Amount = blurAmount;

            Kernel = new float[Radius * 2 + 1];
            Sigma = Radius / Amount;

            var twoSigmaSquare = 2.0f * Sigma * Sigma;
            var sigmaRoot = (float)Math.Sqrt(twoSigmaSquare * Math.PI);
            var total = 0f;
            var distance = 0f;
            var index = 0;

            for (var i = -Radius; i < Radius; ++i)
            {
                distance = i * i;
                index = i + Radius;
                Kernel[index] = (float)Math.Exp(-distance / twoSigmaSquare) / sigmaRoot;

                total += Kernel[index];
            }

            for (var i = 0; i < Kernel.Length; ++i)
            {
                Kernel[i] /= total;
            }
        }

        public void ComputeOffsets(float textureWidth, float textureHeight)
        {
            TextureOffsetsX = new Vector2[Radius * 2 + 1];
            TextureOffsetsY = new Vector2[Radius * 2 + 1];

            var index = 0;
            var xOffset = 1.0f / textureWidth;
            var yOffset = 1.0f / textureHeight;

            for (var i = -Radius; i < Radius; ++i)
            {
                index = i + Radius;
                TextureOffsetsX[index] = new Vector2(i * xOffset, 0.0f);
                TextureOffsetsY[index] = new Vector2(0.0f, i * yOffset);
            }
        }

        public Texture2D PerformGaussianBlur(Texture2D source,
                                             RenderTarget2D renderTarget1,
                                             RenderTarget2D renderTarget2,
                                             SpriteBatch spriteBatch)
        {
            var sourceRect = new Rectangle(0, 0, source.Width, source.Height);
            var targetRect1 = new Rectangle(0, 0, renderTarget1.Width, renderTarget1.Height);
            var targetRect2 = new Rectangle(0, 0, renderTarget2.Width, renderTarget2.Height);

            _game.GraphicsDevice.SetRenderTarget(renderTarget1);

            effect.CurrentTechnique = effect.Techniques["GaussianBlur"];
            effect.Parameters["weights"].SetValue(Kernel);
            effect.Parameters["colorMapTexture"].SetValue(source);
            effect.Parameters["offsets"].SetValue(TextureOffsetsX);

            spriteBatch.Begin(blendState: BlendState.Opaque, effect: effect);
            spriteBatch.Draw(source, targetRect1, Color.White);
            spriteBatch.End();

            _game.GraphicsDevice.SetRenderTarget(renderTarget2);
            var outputTexture = (Texture2D)renderTarget1;

            effect.CurrentTechnique = effect.Techniques["GaussianBlur"];
            effect.Parameters["colorMapTexture"].SetValue(source);
            effect.Parameters["offsets"].SetValue(TextureOffsetsX);

            spriteBatch.Begin(blendState: BlendState.Opaque, effect: effect);
            spriteBatch.Draw(outputTexture, targetRect2, Color.White);
            spriteBatch.End();

            _game.GraphicsDevice.SetRenderTarget(null);
            return (Texture2D)renderTarget2;

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}