using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMoves
{
    public class Move
    {
        public Position From { get; set; }
        public Position To{ get; set; }
        public string PieceName { get; set; }
        public bool IsKilling { get; set; }
        public int Cost { get; set; }
        public int KillingMovesFromTargetPosition { get; set; }
        public int Value { get; set; }
    }
}
