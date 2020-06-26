using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TestGame.Services;

namespace TestGame.Maps
{
    public class MapManager : IMapManager
    {
        private IMap _currentMap;
        private IList<IMap> _adjacentMaps;
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

        private void OnMapContentLoaded(object sender, EventArgs e)
        {
            _adjacentMaps = new List<IMap>(GetAdjacentMaps());
        }

        private IEnumerable<IMap> GetAdjacentMaps()
        {
            foreach (var point in CurrentMap.GetOpenEdges())
                yield return new Map(_game, point);
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

        private void OnComponentAdded(object sender, GameComponentCollectionEventArgs e)
        {
            if (e.GameComponent.GetType().Equals(typeof(Map)))
            {
                ServiceLocator.Instance.AddService<IMap>(e.GameComponent);
            }
        }

        private void OnComponentRemoved(object sender, GameComponentCollectionEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}