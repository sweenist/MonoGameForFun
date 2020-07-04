using System;
using Microsoft.Xna.Framework;

namespace SweenGame
{
    public enum TransitionState
    {
        Off,
        Initialized,
        InTransit,
        Complete
    }

    public abstract class Transition : GameComponent
    {
        protected TransitionState _transitionState;

        protected Transition(Game game) : base(game)
        {
        }

        public event EventHandler<EventArgs> TransitionChanged;

        public virtual TimeSpan TransitionTime { get; protected set; }
        public virtual TransitionState State
        {
            get => _transitionState;
            set
            {
                if (_transitionState == value)
                    return;

                _transitionState = value;
                TransitionChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}