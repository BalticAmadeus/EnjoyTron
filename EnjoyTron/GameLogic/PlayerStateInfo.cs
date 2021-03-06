﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class PlayerStateInfo
    {
        public PlayerCondition Condition;
        public Point Position;
        public string Comment;

        public PlayerStateInfo(PlayerState ps)
        {
            Condition = ps.Condition;
            Position = ps.Head;
            Comment = ps.Comment;
        }
    }
}
