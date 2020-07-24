using System;
using Microsoft.Xna.Framework;
using SweenGame.Camera;
using SweenGame.Entities;
using SweenGame.Enums;
using SweenGame.Input;
using SweenGame.Maps;
using SweenGame.Services;
using SweenGame.Sounds;

namespace SweenGame.Screens
{
    public class GameplayScreen : GameScreen
    {
        private readonly Game _game;
        private readonly GameComponentCollection _components;
        private ISoundManager _sounds;

        public GameplayScreen(Game game)
        {
            _game = game;
            _components = _game.Components;
        }

        public override void Initialize()
        {
            var player = new Player(_game);
            ServiceLocator.Instance.AddService<IPlayer>(player);
            ServiceLocator.Instance.AddService<IFocusable>(player);
            ServiceLocator.Instance.AddService<ICamera2D>(typeof(Camera2D), _game);
            ServiceLocator.Instance.AddService<IMapManager>(typeof(MapManager), _game);

            _components.Add(player);
            _components.Add(ServiceLocator.Instance.GetService<ICamera2D>());

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