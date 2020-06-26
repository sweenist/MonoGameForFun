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
        private MapLocations _mapLocation;
        private IMap _currentMap;
        private IList<IMap> _adjacentMaps;
        private readonly ContentManager _content;
        private readonly GameComponentCollection _component;

        public MapManager(Game game)
        {
            _content = game.Content;
            _component = game.Components;
            _zone = 0;
            _mapLocation = MapLocations.Center;

            _component.ComponentAdded += OnComponentAdded;
            _component.ComponentRemoved += OnComponentRemoved;
            CurrentMap = new Map(game, $"Zone{_zone}_{GetMapLocationDescription(_mapLocation)}.tmx");
            _adjacentMaps = new List<IMap>(GetAdjacentMaps(game));
        }

        private object GetMapLocationDescription(MapLocations location)
        {
            return typeof(MapLocations).GetMember(location.ToString())
                .First(m => m.DeclaringType == typeof(MapLocations))
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .First().Description;
        }

        private IEnumerable<IMap> GetAdjacentMaps(Game game)
        {
            foreach(var field in typeof(MapLocations).GetMembers()
                                                     .Where(m => !(m.Name.Equals(_mapLocation.ToString()) || m.Name.Equals("value__"))
                                                         && m.MemberType == MemberTypes.Field))
            {
                yield return new Map(game, field.Name);
            }
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