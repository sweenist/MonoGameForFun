using System;
using Microsoft.Xna.Framework;
using SweenGame.Camera;
using SweenGame.Extensions;
using SweenGame.Maps;
using SweenGame.Services;

namespace SweenGame.Entities
{
    public class EntityManager : DrawableGameComponent, IEntityManager
    {
        private readonly Game _game;
        private readonly GameComponentCollection _components;
        private IPlayer _player;
        private IMapManager _mapManager;

        public EntityManager(Game game) : base(game)
        {
            _game = game;
            _components = new GameComponentCollection();

            _components.ComponentAdded += OnComponentAdded;
        }

        public override void Initialize()
        {
            Console.WriteLine("Initializing EntityManager");
            _player = new Player(_game);
            ServiceLocator.Instance.AddService<IPlayer, Player>(_player as Player);
            ServiceLocator.Instance.AddService<IFocusable, Player>(_player as Player);
            var camera = ServiceLocator.Instance.AddService<ICamera2D, Camera2D>(arguments: _game);

            _mapManager = ServiceLocator.Instance.AddService<IMapManager, MapManager>(arguments: new object[] { _game, _components });

            _components.Add(_player);
            _components.Add(camera);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _components.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _components.Draw(gameTime);
            base.Draw(gameTime);
        }

        private void OnComponentAdded(object sender, GameComponentCollectionEventArgs e)
        {
            if (e.GameComponent is IGameComponent component)
                component.Initialize();
        }

        private void OnComponentRemoved(object sender, GameComponentCollectionEventArgs e)
        {
            Console.WriteLine($"Removing {e.GameComponent.ToString()}");
            (e.GameComponent as GameComponent).Dispose();
        }
    }
}