using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SweenGame.Enums;

namespace SweenGame.Input
{
    public class InputSystem : GameComponent, IInputSystem
    {
        private KeyboardState _currentKeyboardState;
        private KeyboardState _lastKeyboardState;
        private MouseState _currentMouseState;
        private MouseState _lastMouseState;
        private List<MappedAction> _mappedActions;

        private Func<string, Func<MappedAction, bool>> _namePredicateBuilder = x => y => y.Name.Equals(x);

        public InputSystem(Game game) : base(game)
        {
            _mappedActions = new List<MappedAction>();
        }
        public event ActionDelegate Move;
        public event ActionDelegate MoveUp;
        public event ActionDelegate MoveDown;
        public event ActionDelegate MoveLeft;
        public event ActionDelegate MoveRight;
        public event ActionDelegate Stop;

        public List<MappedAction> MappedActions
        {
            get => _mappedActions;
            set => _mappedActions = value;
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void SetAction(string name, ActionDelegate function)
        {
            if (name.Equals(nameof(Stop)))
            {
                Stop += function;
            }

            var predicate = _namePredicateBuilder(name);
            _mappedActions.FirstOrDefault(predicate).Function = function;
        }

        public void AddAction(string name, Control control, ActionType actionType, ActionDelegate function)
        {
            _mappedActions.Add(new MappedAction(name, control, actionType, function));
        }

        public void SetActionControl(string name, Control control)
        {
            var predicate = _namePredicateBuilder(name);
            _mappedActions.FirstOrDefault(predicate).Control = control;
        }

        public override void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();

            var direction = -1;
            var moving = false;

            if (Enabled)
            {
                foreach (var action in _mappedActions)
                {
                    switch (action.Control)
                    {
                        case Control.Up:
                            action.Function(Direction.North, gameTime);
                            moving = true;
                            break;
                        case Control.Down:
                            action.Function(Direction.South, gameTime);
                            moving = true;
                            break;
                        case Control.Left:
                            action.Function(Direction.West, gameTime);
                            moving = true;
                            break;
                        case Control.Right:
                            action.Function(Direction.East, gameTime);
                            moving = true;
                            break;
                    }

                    if(!moving)
                        Stop(null, gameTime);
                }
            }

            _lastKeyboardState = _currentKeyboardState;
            _lastMouseState = _currentMouseState;
        }
    }
}