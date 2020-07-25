using Microsoft.Xna.Framework;
using SweenGame.Camera;
using SweenGame.Maps;
using SweenGame.Services;

namespace SweenGame.Entities
{
    public class EntityManager : DrawableGameComponent, IEntityManager
    {
        private readonly Game _game;

        public EntityManager(Game game) : base(game)
        {
            _game = game;
        }
        public override void Initialize()
        {
            var player = new Player(_game);
            ServiceLocator.Instance.AddService<IPlayer, Player>(player);
            ServiceLocator.Instance.AddService<IFocusable, Player>(player);
            ServiceLocator.Instance.AddService<ICamera2D, Camera2D>(arguments: _game);
            ServiceLocator.Instance.AddService<IMapManager, MapManager>(arguments: _game);
        }

        protected override void LoadContent()
        {

        }

        public override void Update(GameTime gameTime){}

        public override void Draw(GameTime gameTime)
        {
            
        }
    }
}