using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SweenGame.Enums;
using SweenGame.Extensions;
using SweenGame.Services;
using static SweenGame.Extensions.Constants;

namespace SweenGame.Maps
{
    public class MapManager : IMapManager
    {
        private IMap _currentMap;
        private MapTransition _transition;
        private readonly GameComponentCollection _components;
        private readonly Game _game;

        public MapManager(Game game, GameComponentCollection components)
        {
            _game = game;
            _components = components;

            CurrentMap = new Map(_game, new Point(3, 3), MapType.Overworld);
        }

        public bool IsInTransition => !(_transition is null);

        private void AddAdjacentMaps()
        {
            foreach (var edge in CurrentMap.GetOpenEdges())
            {
                var direction = edge.Key;
                var point = edge.Value;

                var offsetVector = new Vector2(CurrentMap.Bounds.Width, CurrentMap.Bounds.Height) * DirectionVectors.GetVector(direction);

                var map = new Map(_game, point, MapType.Overworld, offsetVector);

                ServiceLocator.Instance.TryRemoveService<IMap>(direction.ToString());
                ServiceLocator.Instance.AddService<IMap, Map>(map, edge.Key.ToString());
            }
        }

        public IMap CurrentMap
        {
            get => _currentMap;
            set
            {
                if (value == _currentMap)
                    return;

                if (!(_currentMap is null))
                {
                    _components.Remove(_currentMap);
                    ServiceLocator.Instance.RemoveService<IMap>(Current);
                }

                _components.Add(value);
                ServiceLocator.Instance.AddService<IMap, Map>(instance: value as Map, name: Current);

                _currentMap = value;
                AddAdjacentMaps();
            }
        }

        public void SlideTransition(Direction direction)
        {
            if (!(_transition is null))
                return;

            var transitiveMap = ServiceLocator.Instance.GetService<IMap>(direction.ToString());

            _transition = new MapTransition(CurrentMap, transitiveMap, direction, _game, _components);
            _transition.Disposing += AssignMaps;
        }

        public void AreaTransition()
        {
            //Todo: may be better served with screen management
        }

        private void AssignMaps(object sender, MapTransitionEventArgs e)
        {
            _transition.Disposing -= AssignMaps;
            _transition = null;

            CurrentMap = e.NewMap;
            AddAdjacentMaps();
        }
    }
}