using Microsoft.Xna.Framework;
using SweenGame.Entities;
using SweenGame.Services;

namespace SweenGame.Camera
{
    public class Camera2D : GameComponent, ICamera2D
    {
        private Vector2 _position;
        private float _viewportHeight;
        private float _viewportWidth;

        public Camera2D(Game game) : base(game)
        {
        }

        ///<summary>The tracking position of the camera. Follows a focal point</summary>
        public Vector2 Position
        {
            get => _position;
            set { _position = value; }
        }

        public Vector2 Origin { get; set; }
        public Vector2 ScreenCenter { get; private set; }
        public Matrix Transform { get; private set; }
        public IFocusable FocalPoint { get; set; }

        public float MoveSpeed { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }

        public override void Initialize()
        {
            FocalPoint = ServiceLocator.Instance.GetService<IFocusable>();

            _viewportWidth = Game.GraphicsDevice.Viewport.Width;
            _viewportHeight = Game.GraphicsDevice.Viewport.Height;

            ScreenCenter = new Vector2(_viewportWidth / 2, _viewportHeight / 2);
            Scale = 1;
            MoveSpeed = 3;
            Origin = ScreenCenter / Scale;
            Position = FocalPoint.Position;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            Transform = Matrix.Identity
                        * Matrix.CreateTranslation(-Position.X, -Position.Y, 0)
                        * Matrix.CreateRotationZ(Rotation)
                        * Matrix.CreateTranslation(Origin.X, Origin.Y, 0)
                        * Matrix.CreateScale(Scale);

            Origin = ScreenCenter / Scale;

            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _position.X += (int)((FocalPoint.Position.X - Position.X) * MoveSpeed * delta);
            _position.Y += (int)((FocalPoint.Position.Y - Position.Y) * MoveSpeed * delta);

            base.Update(gameTime);
        }

        public void ClampCamera(Rectangle bounds)
        {
            var maximumX = bounds.Width - ScreenCenter.X;
            var maximumY = bounds.Height - ScreenCenter.Y;
            var maxPosition = new Vector2(maximumX, maximumY);

            Position = Vector2.Clamp(Position, ScreenCenter, maxPosition);
        }

        public bool IsInView(Vector2 position, Rectangle tileRectangle)
        {
            if (position.X + tileRectangle.Width < Position.X - Origin.X || position.X > Position.X + Origin.X)
                return false;
            if (position.Y + tileRectangle.Height < Position.Y - Origin.Y || position.Y > Position.Y + Origin.Y)
                return false;
            return true;
        }

        public override string ToString()
        {
            return $"Camera2D:\n"
                   + $"\tOrigin: {Origin}\n"
                   + $"\tPosition: {Position}\n";
        }
    }
}