using System.Collections.Generic;

namespace Tron.AdminClient.Models
{
    public class Match
    {
        public Match()
        {
        }

        public Map Map { get; set; }
        public Game Game { get; set; }
        public IList<PlayerState> PlayerStates { get; set; }
    }
}