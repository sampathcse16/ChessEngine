using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMoves
{
    public class Queen:Piece
    {
        public override List<Move> GetValidMoves(Piece[,] board, Piece piece)
        {
            List<Move> moves = new List<Move>();
            int i = piece.Position.I;
            int j = piece.Position.J;

            for (int k = i + 1; k < 8; k++)
            {
                if (board[k, j] == null || board[k, j].IsWhite != piece.IsWhite)
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

            for (int k = i + 1, l = j + 1; k < 8 && l < 8; k++, l++)
            {
                if (board[k, l] == null || board[k, l].IsWhite != piece.IsWhite)
                    AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = k, J = l }, board[k, l] != null);

                if (board[k, l] != null)
                    break;
            }

            for (int k = i - 1, l = j - 1; k >= 0 && l >= 0; k--, l--)
            {
                if (board[k, l] == null || board[k, l].IsWhite != piece.IsWhite)
                    AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = k, J = l }, board[k, l] != null);

                if (board[k, l] != null)
                    break;
            }

            for (int k = i + 1, l = j - 1; k < 8 && l >= 0; k++, l--)
            {
                if (board[k, l] == null || board[k, l].IsWhite != piece.IsWhite)
                    AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = k, J = l }, board[k, l] != null);

                if (board[k, l] != null)
                    break;
            }

            for (int k = i - 1, l = j + 1; k >= 0 && l < 8; k--, l++)
            {
                if (board[k, l] == null || board[k, l].IsWhite != piece.IsWhite)
                    AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = k, J = l }, board[k, l] != null);

                if (board[k, l] != null)
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

            for (int k = i + 1, l = j + 1; k < 8 && l < 8; k++, l++)
            {
                    AddPiece(supportingPieces, piece, k, l);

                if (board[k, l] != null)
                    break;
            }

            for (int k = i - 1, l = j - 1; k >= 0 && l >= 0; k--, l--)
            {
                AddPiece(supportingPieces, piece, k, l);

                if (board[k, l] != null)
                    break;
            }

            for (int k = i + 1, l = j - 1; k < 8 && l >= 0; k++, l--)
            {
                AddPiece(supportingPieces, piece, k, l);

                if (board[k, l] != null)
                    break;
            }

            for (int k = i - 1, l = j + 1; k >= 0 && l < 8; k--, l++)
            {
                    AddPiece(supportingPieces, piece, k, l);

                if (board[k, l] != null)
                    break;
            }
        }
    }
}
