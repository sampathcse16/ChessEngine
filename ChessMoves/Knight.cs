using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMoves
{
    public class Knight:Piece
    {
        public override List<Move> GetValidMoves(Piece[,] board, Piece piece)
        {
            List<Move> moves = new List<Move>();
            int i = piece.Position.I;
            int j = piece.Position.J;

            if(i+2<=Constants.MaxIndex && j-1>=Constants.MinIndex && (board[i+2,j-1]==null || board[i + 2, j - 1].IsWhite!=piece.IsWhite))
               AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i + 2, J = j - 1 }, board[i + 2, j - 1] != null);

            if (i + 2 <= Constants.MaxIndex && j + 1 <= Constants.MaxIndex && (board[i + 2, j + 1] == null || board[i + 2, j + 1].IsWhite != piece.IsWhite))
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i + 2, J = j + 1 }, board[i + 2, j + 1] != null);

            if (i - 2 >= Constants.MinIndex && j + 1 <= Constants.MaxIndex && (board[i - 2, j + 1] == null || board[i - 2, j + 1].IsWhite != piece.IsWhite))
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i - 2, J = j + 1 }, board[i - 2, j + 1] != null);

            if (i - 2 >= Constants.MinIndex && j - 1 >= Constants.MinIndex && (board[i - 2, j - 1] == null || board[i - 2, j - 1].IsWhite != piece.IsWhite))
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i - 2, J = j - 1 }, board[i - 2, j - 1] != null);

            if (i - 1 >= Constants.MinIndex && j - 2 >= Constants.MinIndex && (board[i - 1, j - 2] == null || board[i - 1, j - 2].IsWhite != piece.IsWhite))
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i - 1, J = j - 2 }, board[i - 1, j - 2] != null);

            if (i + 1 <= Constants.MaxIndex && j - 2 >= Constants.MinIndex && (board[i + 1, j - 2] == null || board[i + 1, j - 2].IsWhite != piece.IsWhite))
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i + 1, J = j - 2 }, board[i + 1, j - 2] != null);

            if (i + 1 <= Constants.MaxIndex && j + 2 <= Constants.MaxIndex && (board[i + 1, j + 2] == null || board[i + 1, j + 2].IsWhite != piece.IsWhite))
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i + 1, J = j + 2 }, board[i + 1, j + 2] != null);

            if (i - 1 >= Constants.MinIndex && j + 2 <= Constants.MaxIndex && (board[i - 1, j + 2] == null || board[i - 1, j + 2].IsWhite != piece.IsWhite))
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i - 1, J = j + 2 }, board[i - 1, j + 2] != null);

            return moves;
        }

        public override void UpdateSupportingPieces(Piece[,] board, Piece piece, SupportingPiece[,] supportingPieces)
        {
            int i = piece.Position.I;
            int j = piece.Position.J;

            if (i + 2 <= Constants.MaxIndex && j - 1 >= Constants.MinIndex)
                AddPiece(supportingPieces, piece, i + 2, j - 1);

            if (i + 2 <= Constants.MaxIndex && j + 1 <= Constants.MaxIndex)
                AddPiece(supportingPieces, piece, i + 2, j + 1);

            if (i - 2 >= Constants.MinIndex && j + 1 <= Constants.MaxIndex)
                AddPiece(supportingPieces, piece, i - 2, j + 1);

            if (i - 2 >= Constants.MinIndex && j - 1 >= Constants.MinIndex)
                AddPiece(supportingPieces, piece, i - 2, j - 1);

            if (i - 1 >= Constants.MinIndex && j - 2 >= Constants.MinIndex)
                AddPiece(supportingPieces, piece, i - 1, j - 2);

            if (i + 1 <= Constants.MaxIndex && j - 2 >= Constants.MinIndex)
                AddPiece(supportingPieces, piece, i + 1, j - 2);

            if (i + 1 <= Constants.MaxIndex && j + 2 <= Constants.MaxIndex)
                AddPiece(supportingPieces, piece, i + 1, j + 2);

            if (i - 1 >= Constants.MinIndex && j + 2 <= Constants.MaxIndex)
                AddPiece(supportingPieces, piece, i - 1, j + 2);

        }
    }
}
