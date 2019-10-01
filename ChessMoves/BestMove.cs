using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMoves
{
    public class BestMove
    {
        public Move Move { get; set; }
        public int Value { get; set; }
        public List<Move> AllMoves { get; set; }
        public int MovesCount { get; set; }
    }
}
