using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SweenGame.Enums;
using SweenGame.Extensions;
using SweenGame.Maps;
using SweenGame.Services;

namespace SweenGame.Camera
{
    public class Camera2D : GameComponent, ICamera2D
    {
        private Vector2 _position;
        private float _viewportHeight;
        private float _viewportWidth;
        private int _debugCount;

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

            if (ServiceLocator.Instance.GetService<IMapManager>().IsInTransition)
            {

                var direction = ServiceLocator.Instance.GetService<IPlayer>().CurrentDirection;
                var m = ServiceLocator.Instance.GetService<IMap>(Direction.South.ToString());
                Console.WriteLine($"Camera: {_position} Focus: {FocalPoint.Position} min: {m.Bounds.Height - _viewportHeight}; map {m.Bounds}");

                if (FocalPoint.Position.Y < ScreenCenter.Y && (FocalPoint.Position.Y <= m.Bounds.Height - _viewportHeight + 12)) //mapbounds - viewport
                {
                    if (FocalPoint.Position.Y == 0)
                    {
                        _position.Y -= 24;
                    }
                    _position.Y = Math.Max(_position.Y - 12, ScreenCenter.Y);
                    _debugCount = 8;
                }
            }
            else
            {
                if (_debugCount > 0)
                {
                    var m = ServiceLocator.Instance.GetService<IMap>(Constants.Current);
                    Console.WriteLine($"-Camera: {_position} Focus: {FocalPoint.Position} min: {m.Bounds.Height - _viewportHeight}; map {m.Bounds}");
                    _debugCount--;
                }
                _position.X += (int)((FocalPoint.Position.X - Position.X) * MoveSpeed * delta);
                _position.Y += (int)((FocalPoint.Position.Y - Position.Y) * MoveSpeed * delta);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                Console.WriteLine($"Class: {GetType().Name}");
                Console.WriteLine($"\tCamera Origin: {Origin}");
                Console.WriteLine($"\tCamera Position: {Position}");
                Console.WriteLine();
            }

            base.Update(gameTime);
        }

        public void ClampCamera(Rectangle bounds)
        {
            if (ServiceLocator.Instance.GetService<IMapManager>().IsInTransition)
                return;
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
    }
}