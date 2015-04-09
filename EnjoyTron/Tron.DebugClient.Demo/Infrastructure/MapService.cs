using System;

namespace Tron.DebugClient.Demo.Infrastructure
{
    public class MapService : IMapService
    {
        public event CellChangedEventHandler CellChanged;

        public void UpdateCell(int x, int y, string state)
        {
            Map[y] = Map[y].Replace(Map[y][x], Convert.ToChar(state));

            var handler = CellChanged;
            if (handler != null)
                CellChanged(this, new CellChangedEvent { State = state, X = x, Y = y });
        }

        public event MapChangedEventHandler MapChanged;

        public void UpdateMap(string[] map)
        {
            Map = map;

            var handler = MapChanged;
            if (handler != null)
                MapChanged(this, new MapChangedEventArgs {Map = map});
        }

        public string[] Map { get; private set; }
    }
}
