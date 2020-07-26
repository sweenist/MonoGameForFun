using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private readonly IEntityManager _entityManager;
        private ISoundManager _sounds;

        public GameplayScreen(Game game)
        {
            _game = game;
            _entityManager = ServiceLocator.Instance.AddService<IEntityManager, EntityManager>(nameof(GameplayScreen), _game);
        }

        public override void Initialize()
        {
            _entityManager.Initialize();

            SetupInputManager();
        }

        private void SetupInputManager()
        {
            ActionDelegate movementFunction = (d, _) => MovementData.Create().Move((Direction)d);
            ActionDelegate debugAction = (should, __) => ServiceLocator.Instance.PrintDebug = (bool)should;

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

        public override void UnloadContent()
        {
            _sounds.StopSong(true);
            Console.WriteLine("Unloading");
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

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool isCoveredByOtherScreen)
        {
            if (!otherScreenHasFocus && !isCoveredByOtherScreen)
                _entityManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _screenManager.ClearBuffer();
            _screenManager.SpriteBatch.Begin();
            _entityManager.Draw(gameTime);
            _screenManager.SpriteBatch.End();
        }

        public override void ExitScreen()
        {
            (_entityManager as IDisposable).Dispose();
        }
    }
}