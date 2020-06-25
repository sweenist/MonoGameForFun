using System.ComponentModel;

namespace TestGame.Enums
{
    public enum MapLocations
    {
        [Description("C")] Center,
        [Description("N")] North,
        [Description("NE")] NE,
        [Description("E")] East,
        [Description("SE")] SE,
        [Description("S")] South,
        [Description("SW")] SW,
        [Description("W")] West,
        [Description("NW")] NW,
    }
}