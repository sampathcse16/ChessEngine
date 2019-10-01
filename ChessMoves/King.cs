using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMoves
{
    public class King:Piece
    {
        public override List<Move> GetValidMoves(Piece[,] board, Piece piece)
        {
            List<Move> moves = new List<Move>();
            int i = piece.Position.I;
            int j = piece.Position.J;

            if (i + 1 <= Constants.MaxIndex && (board[i + 1, j] == null || board[i + 1, j].IsWhite != piece.IsWhite))
            {
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i + 1, J = j  }, board[i + 1, j] != null);
            }

            if (i - 1 >= Constants.MinIndex && (board[i - 1, j] == null || board[i - 1, j].IsWhite != piece.IsWhite))
            {
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i - 1, J = j }, board[i - 1, j] != null);
            }

            if (j + 1 <= Constants.MaxIndex && (board[i , j+1] == null || board[i, j + 1].IsWhite != piece.IsWhite))
            {
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i , J = j + 1 }, board[i, j + 1] != null);
            }

            if (j - 1 >= Constants.MinIndex && (board[i, j-1] == null || board[i, j - 1].IsWhite != piece.IsWhite))
            {
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i, J = j -1}, board[i, j - 1] != null);
            }

            if (i - 1 >= Constants.MinIndex && j + 1 <= Constants.MaxIndex && (board[i-1, j + 1] == null || board[i - 1, j + 1].IsWhite != piece.IsWhite))
            {
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i - 1, J = j + 1 }, board[i - 1, j + 1] != null);
            }

            if (i + 1 <= Constants.MaxIndex && j - 1 >= Constants.MinIndex && (board[i+1, j - 1] == null || board[i + 1, j - 1].IsWhite != piece.IsWhite))
            {
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i + 1, J = j - 1 }, board[i + 1, j - 1] != null);
            }

            if (i + 1 <= Constants.MaxIndex && j + 1 <= Constants.MaxIndex && (board[i + 1, j + 1] == null || board[i + 1, j + 1].IsWhite != piece.IsWhite))
            {
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i + 1, J = j + 1 }, board[i + 1, j + 1] != null);
            }

            if (i - 1 >= Constants.MinIndex && j - 1 >= Constants.MinIndex && (board[i - 1, j - 1] == null || board[i - 1, j - 1].IsWhite != piece.IsWhite))
            {
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i - 1, J = j - 1 }, board[i - 1, j - 1] != null);
            }

            if (piece.IsWhite && piece.Position.I==0 && piece.Position.J == 4 &&
                board[0,7] is Rook  &&
               (board[0, 5] == null && board[0, 6] == null))
            {
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = 0, J = 6 });
            }

            if (!piece.IsWhite && piece.Position.I == 7 && piece.Position.J == 4 &&
                board[7, 7] is Rook  &&
                (board[7, 5] == null && board[7, 6] == null))
            {
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = 7, J = 6 });
            }

            return moves;
        }

        public override void UpdateSupportingPieces(Piece[,] board, Piece piece, SupportingPiece[,] supportingPieces)
        {
            int i = piece.Position.I;
            int j = piece.Position.J;

            if (i + 1 <= Constants.MaxIndex)
            {
                AddPiece(supportingPieces, piece, i + 1, j);
            }

            if (i - 1 >= Constants.MinIndex)
            {
                AddPiece(supportingPieces, piece, i - 1, j);
            }

            if (j + 1 <= Constants.MaxIndex)
            {
                AddPiece(supportingPieces, piece, i, j + 1);
            }

            if (j - 1 >= Constants.MinIndex)
            {
                AddPiece(supportingPieces, piece, i, j - 1);
            }

            if (i - 1 >= Constants.MinIndex && j + 1 <= Constants.MaxIndex)
            {
                AddPiece(supportingPieces, piece, i - 1, j + 1);
            }

            if (i + 1 <= Constants.MaxIndex && j - 1 >= Constants.MinIndex)
            {
                AddPiece(supportingPieces, piece, i + 1, j - 1);
            }

            if (i + 1 <= Constants.MaxIndex && j + 1 <= Constants.MaxIndex)
            {
                AddPiece(supportingPieces, piece, i + 1, j + 1);
            }

            if (i - 1 >= Constants.MinIndex && j - 1 >= Constants.MinIndex)
            {
                AddPiece(supportingPieces, piece, i - 1, j - 1);
            }
        }
    }
}
