using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMoves
{
    public class Bishop:Piece
    {
        public override List<Move> GetValidMoves(Piece[,] board, Piece piece)
        {
            List<Move> moves = new List<Move>();
            int i = piece.Position.I;
            int j = piece.Position.J;

            for (int k = i + 1, l=j+1; k < 8 && l<8; k++,l++)
            {
                if (board[k, l] == null || board[k, l].IsWhite != piece.IsWhite)
                    AddMove(board, moves, new Position{I = piece.Position.I, J = piece.Position.J},  new Position { I = k, J = l }, board[k, l] != null);

                if (board[k, l] != null)
                    break;
            }

            for (int k = i - 1, l = j-1; k >= 0 && l>=0; k--,l--)
            {
                if (board[k, l] == null || board[k, l].IsWhite != piece.IsWhite)
                    AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = k, J = l }, board[k, l] != null);

                if (board[k, l] != null)
                    break;
            }

            for (int k = i + 1 ,l= j - 1; k < 8 && l>=0; k++,l--)
            {
                if (board[k,l] == null || board[k, l].IsWhite != piece.IsWhite)
                    AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = k, J = l }, board[k, l] != null);

                if (board[k, l] != null)
                    break;
            }

            for (int k = i - 1, l = j + 1; k >=0 && l < 8; k--, l++)
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
