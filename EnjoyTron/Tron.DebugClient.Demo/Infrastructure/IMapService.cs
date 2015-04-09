using System;

namespace Tron.DebugClient.Demo.Infrastructure
{
    public interface IMapService
    {
        event CellChangedEventHandler CellChanged;
        event MapChangedEventHandler MapChanged;

        void UpdateCell(int x, int y, string state);
        void UpdateMap(string[] map);
        string[] Map { get; }
    }

    public delegate void MapChangedEventHandler(object sender, MapChangedEventArgs args);

    public class MapChangedEventArgs
    {
        public string[] Map { get; set; }
    }

    public class CellChangedEvent : EventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string State { get; set; }
    }

    public delegate void CellChangedEventHandler(object sender, CellChangedEvent args);
}