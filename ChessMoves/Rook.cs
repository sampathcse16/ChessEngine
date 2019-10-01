using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMoves
{
    public class Rook:Piece
    {
        public override List<Move> GetValidMoves(Piece[,] board, Piece piece)
        {
            List<Move> moves = new List<Move>();
            int i = piece.Position.I;
            int j = piece.Position.J;

            for (int k = i+1; k < 8; k++)
            {
                if(board[k, j] == null || board[k, j].IsWhite!=piece.IsWhite)
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = k, J = j }, board[k, j] != null);

                if (board[k, j] != null)
                    break;
            }

            for (int k = i - 1; k >= 0; k--)
            {
                if (board[k, j] == null || board[k, j].IsWhite != piece.IsWhite)
                    AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = k, J = j }, board[k, j] != null);

                if (board[k, j] != null)
                    break;
            }

            for (int k = j - 1; k >= 0; k--)
            {
                if (board[i, k] == null || board[i, k].IsWhite != piece.IsWhite)
                    AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i, J = k }, board[i, k] != null);

                if (board[i, k] != null)
                    break;
            }

            for (int k = j + 1; k < 8; k++)
            {
                if (board[i, k] == null || board[i, k].IsWhite != piece.IsWhite)
                    AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i, J = k }, board[i, k] != null);

                if (board[i, k] != null)
                    break;
            }

            return moves;
        }

        public override void UpdateSupportingPieces(Piece[,] board, Piece piece, SupportingPiece[,] supportingPieces)
        {
            int i = piece.Position.I;
            int j = piece.Position.J;

            for (int k = i + 1; k < 8; k++)
            {
                    AddPiece(supportingPieces, piece, k, j);

                if (board[k, j] != null)
                    break;
            }

            for (int k = i - 1; k >= 0; k--)
            {
                    AddPiece(supportingPieces, piece, k, j);

                if (board[k, j] != null)
                    break;
            }

            for (int k = j - 1; k >= 0; k--)
            {
                    AddPiece(supportingPieces, piece, i, k);

                if (board[i, k] != null)
                    break;
            }

            for (int k = j + 1; k < 8; k++)
            {
                    AddPiece(supportingPieces, piece, i, k);

                if (board[i, k] != null)
                    break;
            }
        }
    }
}
