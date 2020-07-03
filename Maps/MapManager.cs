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
            _component.ComponentAdded += OnComponentAdded;
            _component.ComponentRemoved += OnComponentRemoved;

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
                if (value != _currentMap)
                {
                    _component.Remove(_currentMap);
                    _component.Add(value);
                    _currentMap = value;
                }
            }
        }

        public void Transition(Direction direction)
        {
            if (!(_transition is null))
                return;

            var transitiveMapIndex = DirectionVectors.GetPoint(direction) + CurrentMap.MapIndex;
            var transitiveMap = _adjacentMaps.Single(map => map.MapIndex.Equals(transitiveMapIndex));

            _component.ComponentAdded -= OnComponentAdded;
            _transition = new MapTransition(CurrentMap, transitiveMap, direction, _game);
            _transition.Disposing += AssignMaps;
            _component.ComponentAdded += OnComponentAdded;
        }

        private void AssignMaps(object sender, MapTransitionEventArgs e)
        {
            _adjacentMaps.Clear();
            CurrentMap = e.NewMap;
            OnMapContentLoaded(sender, e);
        }

        private void OnComponentAdded(object sender, GameComponentCollectionEventArgs e)
        {
            if (e.GameComponent.GetType().Equals(typeof(Map)))
            {
                ServiceLocator.Instance.AddService<IMap>(e.GameComponent);
            }
        }

        private void OnComponentRemoved(object sender, GameComponentCollectionEventArgs e)
        {
            if (e.GameComponent.GetType().Equals(typeof(Map)))
            {
                ServiceLocator.Instance.RemoveService<IMap>(e.GameComponent);
            }
        }
    }
}