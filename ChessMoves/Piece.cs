using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMoves
{
    public abstract class Piece
    {


        static Random rnd = new Random();
        public Position Position { get; set; }
        public bool IsWhite { get; set; }
        public abstract List<Move> GetValidMoves(Piece[,] board, Piece piece);
        public abstract void UpdateSupportingPieces(Piece[,] board, Piece piece, SupportingPiece[,] supportingPieces);
        public void AddMove(Piece[,] board, List<Move> moves, Position from, Position to, bool isKilling = false)
        {
            int randomValue = 0;

            if (!(this is King))
                randomValue = rnd.Next(0, 100);

                moves.Add(new Move { From = from, To = to, IsKilling = isKilling, PieceName = GetPieceName(this), Cost = isKilling ?100: 0 });
        }

        public string GetPieceName(Piece piece)
        {
            if (piece is Pawn)
            {
                if (piece.IsWhite)
                {
                    return "WP";
                }

                return "BP";
            }

            if (piece is Rook)
            {
                if (piece.IsWhite)
                {
                    return "WR";
                }

                return "BR";
            }

            if (piece is Knight)
            {
                if (piece.IsWhite)
                {
                    return "WN";
                }

                return "BN";
            }

            if (piece is Bishop)
            {
                if (piece.IsWhite)
                {
                    return "WB";
                }

                return "BB";
            }

            if (piece is Queen)
            {
                if (piece.IsWhite)
                {
                    return "WQ";
                }

                return "BQ";
            }

            if (piece is King)
            {
                if (piece.IsWhite)
                {
                    return "WK";
                }

                return "BK";
            }

            return string.Empty;
        }
        public int GetPieceCost(Piece piece)
        {
            if (piece is Pawn)
            {
                return 1;
            }

            if (piece is Rook)
            {
                return 5;
            }

            if (piece is Knight)
            {
                return 3;
            }

            if (piece is Bishop)
            {
                return 3;
            }

            if (piece is Queen)
            {
                return 9;
            }

            if (piece is King)
            {
                return 10000;
            }

            return 0;
        }
        public void AddPiece(SupportingPiece[,] supportingPieces, Piece piece, int row, int column)
        {
            if (supportingPieces[row, column] == null)
            {
                supportingPieces[row, column] = new SupportingPiece { Pieces = new List<Piece>() };
            }

            supportingPieces[row, column].Pieces.Add(piece);
        }

    }
}
