using System;
using Microsoft.Xna.Framework;
using SweenGame.Camera;
using SweenGame.Enums;
using SweenGame.Input;
using SweenGame.Maps;
using SweenGame.Services;

namespace SweenGame.Screens
{
    public class GameplayScreen : GameScreen
    {
        private MovementManager _movementManager;
        private readonly Game _game;
        private readonly GameComponentCollection _components;

        public GameplayScreen(Game game)
        {
            _game = game;
            _components = _game.Components;
        }

        public override void Initialize()
        {
            ServiceLocator.Instance.AddService<IMapManager>(typeof(MapManager), _game);

            // _movementManager = new MovementManager(_game);
            // _components.Add(_movementManager);

            var player = new Player(_game);
            ServiceLocator.Instance.AddService<IPlayer>(player);
            ServiceLocator.Instance.AddService<IFocusable>(player);
            _components.Add(player);

            ServiceLocator.Instance.AddService<ICamera2D>(typeof(Camera2D), _game);
            _components.Add(ServiceLocator.Instance.GetService<ICamera2D>());

            var input = ServiceLocator.Instance.GetService<IInputSystem>();
            ActionDelegate movementFunction = (d, _) => MovementData.Create().Move((Direction)d);
            input.SetAction("Move Up", movementFunction);
            input.SetAction("Move Down", movementFunction);
            input.SetAction("Move Left", movementFunction);
            input.SetAction("Move Right", movementFunction);
            input.SetAction("Debug", (_, __) => Console.WriteLine(player));
            input.SetAction("Stop", (_, __) => { });

            input.Enable();
        }

        public override void Draw(GameTime gameTime)
        {

        }
    }
}