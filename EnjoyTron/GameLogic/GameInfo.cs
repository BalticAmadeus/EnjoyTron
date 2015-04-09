using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class GameInfo
    {
        public int GameId;
        public string Label;
        public GameState State;

        public GameInfo(Game game)
        {
            GameId = game.GameId;
            Label = game.Label;
            State = game.State;
        }
    }
}
