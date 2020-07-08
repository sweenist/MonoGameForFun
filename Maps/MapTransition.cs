using System;
using Microsoft.Xna.Framework;
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

        public MapTransition(IMap current, IMap other, Direction direction, Game game) : base(game)
        {
            _direction = direction;
            _current = current;
            _other = other;

            _components = game.Components;
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
                Console.WriteLine($"Seams: Current -> L:{_current.Bounds.Left}; B:{_current.Bounds.Bottom}; R:{_current.Bounds.Right}");
                Console.WriteLine($"\t: Other -> L:{_other.Bounds.Left}; T:{_other.Bounds.Top}; R:{_other.Bounds.Right}");
                State = TransitionState.InTransit;
            }
            if (State == TransitionState.InTransit)
            {
                var playerTransitioning = _player.Transition(_other.MaxMapTileLocation);
                var adjustment = playerTransitioning ? _adjustmentVector : _adjustmentVector * 2;
                Console.WriteLine($"Trans: Player: {playerTransitioning}; Adj:{_adjustmentVector}/ {adjustment}");
                
                _current.Transition(adjustment);

                if (_other.Transition(adjustment))
                {
                    // catch player finished. Set map to 0 offset
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