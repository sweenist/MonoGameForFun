using System;
using Microsoft.Xna.Framework;
using SweenGame;
using SweenGame.Enums;
using SweenGame.Extensions;
using SweenGame.Maps;
using SweenGame.Services;

namespace SweenGame.Maps
{
    public class MapTransitionEventArgs : EventArgs
    {
        public MapTransitionEventArgs(IMap oldMap, IMap newMap)
        {
            OldMap = oldMap;
            NewMap = newMap;
        }

        public IMap OldMap { get; set; }
        public IMap NewMap { get; set; }
    }

    public class MapTransition : Transition, IDisposable
    {
        private readonly IMap _current;
        private readonly IMap _other;
        private readonly Direction _direction;
        private readonly GameComponentCollection _components;
        private TimeSpan _elapsedTime;

        public MapTransition(IMap current, IMap other, Direction direction, Game game) : base(game)
        {
            _direction = direction;
            _current = current;
            _other = other;

            _components = game.Components;
            _components.Add(this);
            _components.Add(_other);

            _elapsedTime = TimeSpan.Zero;
            TransitionTime = TimeSpan.FromSeconds(3);
            State = TransitionState.Initialized;
            TransitionChanged += OnTransitionChanged;
        }

        public event EventHandler<MapTransitionEventArgs> Disposing;

        public override void Update(GameTime gameTime)
        {
            if (State == TransitionState.Initialized)
            {
                State = TransitionState.InTransit;
            }
            else if (State == TransitionState.InTransit)
            {
                _elapsedTime += gameTime.ElapsedGameTime;
                var timeDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / TransitionTime.TotalMilliseconds);
                var shiftDelta = timeDelta * _current.Bounds.Height;

                var adjustmentVector = DirectionVectors.GetVector(_direction) * -(int)shiftDelta;
                _current.Adjust(adjustmentVector);
                _other.Adjust(adjustmentVector);

                if (_elapsedTime > TransitionTime)
                {
                    State = TransitionState.Complete;
                }
            }
            else if (State == TransitionState.Complete)
            {
                Dispose();
            }
        }

        private void OnTransitionChanged(object sender, EventArgs e)
        {

        }

        protected override void Dispose(bool disposing)
        {
            _components.Remove(_other);
            Disposing?.Invoke(this, new MapTransitionEventArgs(_current, _other));

            TransitionChanged -= OnTransitionChanged;
            _components.Remove(this);

            base.Dispose(disposing);
        }
    }
}