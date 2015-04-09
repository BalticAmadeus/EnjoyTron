using System;
using System.Collections.Generic;

namespace Tron.DebugClient.Infrastructure
{
    public class MapService : IMapService
    {
        public event CellChangedEventHandler CellChanged;

        public void UpdateCell(int x, int y, string state, Player player)
        {
            Map[y] = Map[y].Substring(0, x) + Convert.ToChar(state) + Map[y].Substring(x + 1);
            Players[player.Index] = player;

            var handler = CellChanged;
            if (handler != null)
                CellChanged(this, new CellChangedEvent { State = state, X = x, Y = y });
        }

        public event MapChangedEventHandler MapChanged;

        public void UpdateMap(string[] map, List<Player> players)
        {
            Map = map;
            Players = players;

            var handler = MapChanged;
            if (handler != null)
                MapChanged(this, new MapChangedEventArgs {Map = map});
        }

        public string[] Map { get; private set; }
        public IList<Player> Players { get; private set; } 
    }
}
