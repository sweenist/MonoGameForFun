using System;
using Microsoft.Xna.Framework;
using SweenGame.Entities;
using SweenGame.Enums;
using SweenGame.Extensions;
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
        private readonly IPlayer _player;
        private readonly GameComponentCollection _components;
        private Vector2 _adjustmentVector;

        public MapTransition(IMap current, IMap other, Direction direction, Game game, GameComponentCollection components) : base(game)
        {
            _direction = direction;
            _current = current;
            _other = other;

            _components = components;
            _components.Add(_other);
            _components.Add(this);

            _player = ServiceLocator.Instance.GetService<IPlayer>();

            _adjustmentVector = DirectionVectors.GetVector(_direction).Invert();
            State = TransitionState.Initialized;
        }

        public event EventHandler<MapTransitionEventArgs> Disposing;

        public override void Update(GameTime gameTime)
        {
            if (State == TransitionState.Initialized)
            {
                State = TransitionState.InTransit;
            }
            if (State == TransitionState.InTransit)
            {
                _player.Transition(_other.MaxMapTileLocation);
                _current.Transition(_adjustmentVector);

                if (_other.Transition(_adjustmentVector))
                {
                    State = TransitionState.Complete;
                }
            }
            else if (State == TransitionState.Complete)
            {
                Dispose();
            }
        }

        protected override void Dispose(bool disposing)
        {
            _components.Remove(_other);
            Disposing?.Invoke(this, new MapTransitionEventArgs(_current, _other));

            _components.Remove(this);

            base.Dispose(disposing);
        }
    }
}