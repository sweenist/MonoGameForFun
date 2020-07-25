using System;
using Microsoft.Xna.Framework;
using SweenGame.Entities;
using SweenGame.Enums;
using SweenGame.Input;
using SweenGame.Services;
using SweenGame.Sounds;

namespace SweenGame.Screens
{
    public class GameplayScreen : GameScreen
    {
        private readonly Game _game;
        private ISoundManager _sounds;

        public GameplayScreen(Game game)
        {
            _game = game;
        }

        public override void Initialize()
        {
            ServiceLocator.Instance.AddService<IEntityManager, EntityManager>(nameof(GameplayScreen), _game);
            SetupInputManager((should, __) => ServiceLocator.Instance.PrintDebug = (bool)should);
        }

        private void SetupInputManager(ActionDelegate debugAction)
        {
            ActionDelegate movementFunction = (d, _) => MovementData.Create().Move((Direction)d);

            var input = ServiceLocator.Instance.GetService<IInputSystem>();
            input.SetAction(ActionType.MoveUp, movementFunction);
            input.SetAction(ActionType.MoveDown, movementFunction);
            input.SetAction(ActionType.MoveLeft, movementFunction);
            input.SetAction(ActionType.MoveRight, movementFunction);
            input.SetAction(ActionType.Debug, debugAction);

            input.Enable();
        }

        public override void LoadContent()
        {
            _sounds = ServiceLocator.Instance.GetService<ISoundManager>();
            _sounds.LoadSong(SongNames.Overworld);
        }

        public override void HandleInput(InputState state, GameTime gameTime)
        {
            if (state is null)
                throw new ArgumentNullException($"Input state is null in {GetType().Name}");

            if (state.PauseGame)
            {
                _sounds.Volume = 0.65f;
                _screenManager.AddScreen(new PauseMenuScreen());
            }
        }

        public override void Draw(GameTime gameTime)
        {
        }
    }
}