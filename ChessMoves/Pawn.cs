using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMoves
{
    public class Pawn : Piece
    {
        public override List<Move> GetValidMoves(Piece[,] board, Piece piece)
        {
            int i = piece.Position.I;
            int j = piece.Position.J;

            const int initialPositionForWhite = 1;
            const int initialPositionForBlack = 6;
             
            List<Move> moves = new List<Move>();
            Move move = null;

            if (piece.IsWhite)
            {
                if (i == initialPositionForWhite)
                {
                    bool isValid = true;
                    for (int row = i+1; row <=i + 2; row++)
                    {
                        if (row <= Constants.MaxIndex && board[row, j] != null)
                        {
                            isValid = false;
                            break;
                        }
                    }
                    
                    if(isValid)
                    AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i + 2, J = j });
                }

                if(i+1 <= Constants.MaxIndex && board[i + 1, j] == null)
                AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i + 1, J = j });

                if (i + 1 <= Constants.MaxIndex && j-1>= Constants.MinIndex && board[i + 1, j - 1]!=null && !board[i + 1, j - 1].IsWhite)
                {
                    AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i + 1, J = j-1 }, true);
                }

                if (i + 1 <= Constants.MaxIndex && j + 1 <= Constants.MaxIndex && board[i + 1, j + 1] != null && !board[i + 1, j + 1].IsWhite)
                {
                    AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i + 1, J = j+1 }, true);
                }
            }
            else
            {
                if (i == initialPositionForBlack)
                {
                    bool isValid = true;
                    for (int row = i-1; row >= i - 2; row--)
                    {
                        if (row>= Constants.MinIndex && board[row, j] != null)
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid)
                        AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i - 2, J = j });
                }

                if (i-1>=Constants.MinIndex && board[i - 1, j] == null)
                    AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i - 1, J = j });

                if (i - 1 >= Constants.MinIndex && j-1>=Constants.MinIndex && board[i - 1, j - 1] != null && board[i - 1, j - 1].IsWhite)
                {
                    AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i - 1, J = j - 1 }, true);
                }

                if (i - 1 >= Constants.MinIndex && j+1<=Constants.MaxIndex && board[i - 1, j + 1] != null && board[i - 1, j + 1].IsWhite)
                {
                    AddMove(board, moves, new Position { I = piece.Position.I, J = piece.Position.J }, new Position { I = i - 1, J = j + 1 }, true);
                }
            }

            return moves;
        }

        public override void UpdateSupportingPieces(Piece[,] board, Piece piece, SupportingPiece[,] supportingPieces)
        {
            int i = piece.Position.I;
            int j = piece.Position.J;


            if (piece.IsWhite)
            {
                if (i + 1 <= Constants.MaxIndex && j - 1 >= Constants.MinIndex)
                {
                    AddPiece(supportingPieces, piece, i + 1, j - 1);
                }

                if (i + 1 <= Constants.MaxIndex && j + 1 <= Constants.MaxIndex)
                {
                    AddPiece(supportingPieces, piece, i + 1, j + 1);
                }
            }
            else
            {
               
                if (i - 1 >= Constants.MinIndex && j - 1 >= Constants.MinIndex)
                {
                    AddPiece(supportingPieces, piece, i - 1, j - 1);
                }

                if (i - 1 >= Constants.MinIndex && j + 1 <= Constants.MaxIndex)
                {
                    AddPiece(supportingPieces, piece, i - 1, j + 1);
                }
            }

            
        }

    }
}
