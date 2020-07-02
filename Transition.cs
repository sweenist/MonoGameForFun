using System;
using Microsoft.Xna.Framework;

namespace TestGame
{
    public enum TransitionState
    {
        Off,
        Initialized,
        InTransit,
        Complete
    }

    public abstract class Transition : IUpdateable
    {
        protected bool _enabled;
        protected int _updateOrder;
        protected TransitionState _transitionState;
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
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
        public virtual bool Enabled
        {
            get => _enabled;
            protected set
            {
                if (_enabled.Equals(value))
                    return;

                _enabled = value;
                EnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public virtual int UpdateOrder
        {
            get => _updateOrder;
            protected set
            {
                if (_updateOrder.Equals(value))
                    return;

                _updateOrder = value;
                UpdateOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;
        }
    }
}