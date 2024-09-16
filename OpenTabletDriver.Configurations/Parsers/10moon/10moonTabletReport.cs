using System;
using System.Numerics;
using OpenTabletDriver.Tablet;

namespace OpenTabletDriver.Configurations.Parsers.TenMoon
{
    public struct TenMoonTabletReport : ITabletReport, IAuxReport
    {
        public TenMoonTabletReport(byte[] report)
        {
            Raw = report;

            Position = new Vector2
            {
                X = report[1] << 8 | report[2],
                Y = Math.Max((short)(report[3] << 8 | report[4]), (short)0)
            };

            var buttonPressed = (report[9] & 6) != 0;
            var prePressure = report[5] << 8 | report[6];
            Pressure = (uint)(1800 - (prePressure - (buttonPressed ? 50 : 0)));

            PenButtons = new bool[]
            {
                (report[9] & 6) == 4,
                (report[9] & 6) == 6
            };

            AuxButtons = new bool[]
            {
                !report[12].IsBitSet(0),
                !report[12].IsBitSet(5),
                !report[12].IsBitSet(1),
                !report[12].IsBitSet(4),
                !report[11].IsBitSet(6),
                !report[11].IsBitSet(7),
                !report[11].IsBitSet(5),
                !report[11].IsBitSet(4),
            };
        }

        public byte[] Raw { set; get; }
        public Vector2 Position { set; get; }
        public uint Pressure { set; get; }
        public bool[] PenButtons { set; get; }
        public bool[] AuxButtons { set; get; }
    }
}
