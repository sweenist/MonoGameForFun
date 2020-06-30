using TestGame.Enums;
using TestGame.Maps;

namespace TestGame
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

        public bool Continue { get; set; }
        public IPlayer Player { get; }
        public IMap Map { get; }
        public Direction Direction { get; set; }
    }
}