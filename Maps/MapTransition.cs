using System;
using Microsoft.Xna.Framework;

namespace TestGame.Maps
{
    public class MapTransition : Transition, IDisposable
    {
        private readonly IMap _current;
        private readonly IMap _other;

        public MapTransition(IMap current, IMap other)
        {
            _current = current;
            _other = other;
            TransitionTime = TimeSpan.FromSeconds(1);

            TransitionChanged += OnTransitionChanged;
        }

        public override void Update(GameTime gameTime)
        {
            if (_transitionState == TransitionState.Off)
                return;
            else if (_transitionState == TransitionState.Initialized)
            {
                State = TransitionState.InTransit;
            }
            else if (_transitionState == TransitionState.InTransit)
            {
                var timeDelta = (float)(gameTime.TotalGameTime.Milliseconds / TransitionTime.Milliseconds);
                // Transition position logic here 0..1 or 0..ScreenDimension||MapDimension
            }

        }

        private void OnTransitionChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            TransitionChanged -= OnTransitionChanged;
        }
    }
}