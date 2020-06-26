using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TestGame.Enums;
using TestGame.Services;

namespace TestGame.Maps
{
    public class MapManager : IMapManager
    {
        private int _zone;
        private IMap _currentMap;
        private IList<IMap> _adjacentMaps;
        private readonly ContentManager _content;
        private readonly GameComponentCollection _component;

        public MapManager(Game game)
        {
            _content = game.Content;
            _component = game.Components;
            _zone = 0;

            _component.ComponentAdded += OnComponentAdded;
            _component.ComponentRemoved += OnComponentRemoved;
            CurrentMap = new Map(game, new Point(3,3));
            _adjacentMaps = new List<IMap>(GetAdjacentMaps(game));
        }

        private IEnumerable<IMap> GetAdjacentMaps(Game game)
        {
            yield return null;
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