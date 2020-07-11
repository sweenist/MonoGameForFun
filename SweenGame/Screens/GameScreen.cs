using System;
using Microsoft.Xna.Framework;
using SweenGame.Enums;
using SweenGame.Services;

namespace SweenGame.Screens
{
    public abstract class GameScreen
    {
        protected bool _isPopup;
        protected bool _isExiting;
        protected bool _otherScreenHasFocus;
        protected float _transitionPosition = -1.0f;
        protected TimeSpan _transitionOnTime;
        protected TimeSpan _transitionOffTime;
        protected IScreenManager _screenManager;

        protected ScreenState _screenState = ScreenState.TransitionOn;

        public GameScreen()
        {
            _screenManager = ServiceLocator.Instance.GetService<IScreenManager>();
        }
        public bool IsPopup
        {
            get => _isPopup;
            protected set => _isPopup = value;
        }

        public TimeSpan TransistionOnTime
        {
            get => _transitionOnTime;
            protected set => _transitionOnTime = value;
        }

        public TimeSpan TransistionOffTime
        {
            get => _transitionOffTime;
            protected set => _transitionOffTime = value;
        }

        public float TransitionPosition
        {
            get => _transitionPosition;
            protected set => _transitionPosition = value;
        }

        public byte TransitionAlpha => (byte)(255 - TransitionPosition * 255);

        public ScreenState ScreenState
        {
            get => _screenState;
            protected set => _screenState = value;
        }

        public bool IsExiting
        {
            get => IsExiting;
            set => IsExiting = value;
        }

        public bool IsActive => !_otherScreenHasFocus
                                && (_screenState == ScreenState.TransitionOn || _screenState == ScreenState.Active);

        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool isCoveredByOtherScreen)
        {
            _otherScreenHasFocus = otherScreenHasFocus;
            if (_isExiting)
            {
                _screenState = ScreenState.TransitionOff;
                if (!UpdateTransition(gameTime, _transitionOffTime, 1))
                    _screenState = ScreenState.TransitionOff;
                else
                    _screenState = ScreenState.Hidden;
            }
            else
            {
                if (UpdateTransition(gameTime, _transitionOnTime, -1))
                    _screenState = ScreenState.TransitionOn;
                else
                    _screenState = ScreenState.Active;
            }
        }

        private bool UpdateTransition(GameTime gameTime, TimeSpan transitionTime, int direction)
        {
            var transitionDelta = transitionTime.Equals(TimeSpan.Zero)
            ? 1.0f
            : (float)(gameTime.ElapsedGameTime.TotalMilliseconds / transitionTime.TotalMilliseconds);

            _transitionPosition += transitionDelta * direction;
            _transitionPosition = MathHelper.Clamp(_transitionPosition, 0, 1);

            return _transitionPosition > 0 && _transitionPosition < 1;
        }

        public abstract void Draw(GameTime gameTime);

        public virtual void ExitScreen()
        {
            if (_transitionOffTime <= TimeSpan.Zero)
                _screenManager.RemoveScreen(this);
            else
                _isExiting = true;
        }
    }
}