using SweenGame.Entities;
using SweenGame.Enums;
using SweenGame.Maps;
using SweenGame.Services;
using static SweenGame.Extensions.Constants;

namespace SweenGame
{
    public class MovementData
    {
        public MovementData(IPlayer player, IMap map)
        {
            Player = player;
            Map = map;
            Direction = Player.CurrentDirection;
            Continue = true;
        }

        public static MovementData Create()
        {
            var player = ServiceLocator.Instance.GetService<IPlayer>();
            var map = ServiceLocator.Instance.GetService<IMap>(Current);

            return new MovementData(player, map);
        }

        public bool Continue { get; set; }
        public IPlayer Player { get; }
        public IMap Map { get; }
        public Direction Direction { get; set; }
    }
}