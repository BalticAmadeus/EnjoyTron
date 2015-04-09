using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class WaitTurnInfo
    {
        public int PlayerIndex = -1;
        public int Turn;

        public bool TurnComplete;
        public bool GameFinished;
        public PlayerCondition FinishCondition;
        public string FinishComment;
    }
}
