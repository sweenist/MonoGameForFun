using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TestGame.Enums;
using TestGame.Extensions;
using TestGame.Services;

namespace TestGame.Maps
{
    public class MapManager : IMapManager
    {
        private IMap _currentMap;
        private IList<IMap> _adjacentMaps;
        private MapTransition _transition;
        private readonly ContentManager _content;
        private readonly GameComponentCollection _component;
        private readonly Game _game;

        public MapManager(Game game)
        {
            _game = game;
            _content = _game.Content;

            _component = _game.Components;

            CurrentMap = new Map(_game, new Point(3, 3));
            CurrentMap.ContentLoaded += OnMapContentLoaded;
        }

        public bool IsInTransition => !(_transition is null);

        private void OnMapContentLoaded(object sender, EventArgs e)
        {
            _adjacentMaps = new List<IMap>(GetAdjacentMaps());
        }

        private IEnumerable<IMap> GetAdjacentMaps()
        {
            foreach (var point in CurrentMap.GetOpenEdges())
                yield return new Map(_game, point, new Vector2(CurrentMap.Bounds.Width, CurrentMap.Bounds.Height) * (point - CurrentMap.MapIndex).ToVector2());
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
                    _component.Remove(_currentMap);
                    ServiceLocator.Instance.RemoveService<IMap>(_currentMap);
                }

                _component.Add(value);
                ServiceLocator.Instance.AddService<IMap>(value);

                _currentMap = value;
            }
        }

        public void Transition(Direction direction)
        {
            if (!(_transition is null))
                return;

            var transitiveMapIndex = DirectionVectors.GetPoint(direction) + CurrentMap.MapIndex;
            var transitiveMap = _adjacentMaps.Single(map => map.MapIndex.Equals(transitiveMapIndex));

            _transition = new MapTransition(CurrentMap, transitiveMap, direction, _game);
            _transition.Disposing += AssignMaps;
        }

        private void AssignMaps(object sender, MapTransitionEventArgs e)
        {
            _adjacentMaps.Clear();
            CurrentMap = e.NewMap;
            OnMapContentLoaded(sender, e);
        }
    }
}