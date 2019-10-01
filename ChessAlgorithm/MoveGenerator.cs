using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessMoves;

namespace ChessAlgorithm
{
    public static class MoveGenerator
    {
        public static int MoveGeneratorCallsCounter = 0;
        public static int[,] WP =
        {
            {0,0,0,0,0,0,0,0},
            {5,10,10,-20,-20,10,10,5},
            {5,-5,-10,0,0,-10,-5,5},
            {0,0,0,20,20,0,0,0},
            {5,5,10,25,25,10,5,5},
            {10,10,20,30,30,20,10,10},
            {50,50,50,50,50,50,50,50},
            {0,0,0,0,0,0,0,0}
        };

        public static int[,] BP =
        {
            {0, 0, 0, 0, 0, 0, 0, 0},
            {50, 50, 50, 50, 50, 50, 50, 50},
            {10, 10, 20, 30, 30, 20, 10, 10},
            {5, 5, 10, 25, 25, 10, 5, 5},
            {0, 0, 0, 20, 20, 0, 0, 0},
            {5, -5, -10, 0, 0, -10, -5, 5},
            {5, 10, 10, -20, -20, 10, 10, 5},
            {0, 0, 0, 0, 0, 0, 0, 0}
        };

        public static int[,] WR =
        {
            {0, 0, 0, 5, 5, 0, 0, 0},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {5, 10, 10, 10, 10, 10, 10, 5},
            {0, 0, 0, 0, 0, 0, 0, 0,}
        };

        public static int[,] BR =
        {
            {0,  0,  0,  0,  0,  0,  0,  0},
            {5, 10, 10, 10, 10, 10, 10,  5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {0,  0,  0,  5,  5,  0,  0,  0}
        };

        public static int[,] WN =
        {
            {-50,-40,-30,-30,-30,-30,-40,-50},
            {-40,-20,0,5,5,0,-20,-40},
            {-30,5,10,15,15,10,5,-30},
            {-30,0,15,20,20,15,0,-30},
            {-30,5,15,20,20,15,5,-30},
            {-30,0,10,15,15,10,0,-30},
            {-40,-20,0,0,0,0,-20,-40},
            {-50,-40,-30,-30,-30,-30,-40,-50}
        };

        public static int[,] BN =
        {
            {-50,-40,-30,-30,-30,-30,-40,-50},
            {-40,-20,  0,  0,  0,  0,-20,-40},
            {-30,  0, 10, 15, 15, 10,  0,-30},
            {-30,  5, 15, 20, 20, 15,  5,-30},
            {-30,  0, 15, 20, 20, 15,  0,-30},
            {-30,  5, 10, 15, 15, 10,  5,-30},
            {-40,-20,  0,  5,  5,  0,-20,-40},
            {-50,-40,-30,-30,-30,-30,-40,-50}
        };

        public static int[,] WB =
        {
            {-20,-10,-10,-10,-10,-10,-10,-20},
            {-10,5,0,0,0,0,5,-10},
            {-10,10,10,10,10,10,10,-10},
            {-10,0,10,10,10,10,0,-10},
            {-10,5,5,10,10,5,5,-10},
            {-10,0,5,10,10,5,0,-10},
            {-10,0,0,0,0,0,0,-10},
            {-20,-10,-10,-10,-10,-10,-10,-20}
        };

        public static int[,] BB =
        {
            {-20,-10,-10,-10,-10,-10,-10,-20},
            {-10,  0,  0,  0,  0,  0,  0,-10},
            {-10,  0,  5, 10, 10,  5,  0,-10},
            {-10,  5,  5, 10, 10,  5,  5,-10},
            {-10,  0, 10, 10, 10, 10,  0,-10},
            {-10, 10, 10, 10, 10, 10, 10,-10},
            {-10,  5,  0,  0,  0,  0,  5,-10},
            {-20,-10,-10,-10,-10,-10,-10,-20}
        };

        public static int[,] WQ =
        {
            {-20,-10,-10,-5,-5,-10,-10,-20},
            {-10,0,5,0,0,0,0,-10},
            {-10,5,5,5,5,5,0,-10},
            {0,0,5,5,5,5,0,-5},
            {-5,0,5,5,5,5,0,-5},
            {-10,0,5,5,5,5,0,-10},
            {-10,0,0,0,0,0,0,-10},
            {-20,-10,-10,-5,-5,-10,-10,-20}
        };

        public static int[,] BQ =
        {
            {-20,-10,-10, -5, -5,-10,-10,-20},
            {-10,  0,  0,  0,  0,  0,  0,-10},
            {-10,  0,  5,  5,  5,  5,  0,-10},
            { -5,  0,  5,  5,  5,  5,  0, -5},
            {  0,  0,  5,  5,  5,  5,  0, -5},
            {-10,  5,  5,  5,  5,  5,  0,-10},
            {-10,  0,  5,  0,  0,  0,  0,-10},
            {-20,-10,-10, -5, -5,-10,-10,-20}
        };

        public static int[,] WK =
        {
            {20,30,10,0,0,10,30,20},
            {20,20,0,0,0,0,20,20},
            {-10,-20,-20,-20,-20,-20,-20,-10},
            {-20,-30,-30,-40,-40,-30,-30,-20},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30}
        };

        public static int[,] BK =
        {
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-20,-30,-30,-40,-40,-30,-30,-20},
            {-10,-20,-20,-20,-20,-20,-20,-10},
            { 20, 20,  0,  0,  0,  0, 20, 20},
            { 20, 30, 10,  0,  0, 10, 30, 20}
        };
        public static int WhiteValue { get; set; }
        public static int BlackValue { get; set; }
        public static int Counter { get; set; }
        public static Dictionary<string, int> HashedBoard = new Dictionary<string, int>();
        public static BestMove GetBestMove(Piece[,] board, int depth, int α, int β, bool maximizingPlayer)
        {
            Counter = 0;
            HashedBoard.Clear();
            BoardValue(board, true);
            return AlphaBeta(board, depth, α, β, maximizingPlayer, null);
        }

        public static List<Move> GetAllValidMoves(Piece[,] board, bool isWhite)
        {
            List<Move> validMoves = new List<Move>();
            List<Move> moves = GetAllMoves(board, isWhite);
            if (IsKingInCheck(board, isWhite))
            {

                foreach (var move in moves)
                {
                    Piece piece = board[move.To.I, move.To.J];
                    MakeTempMove(board, move);
                    if (!IsKingInCheck(board, isWhite))
                    {
                        validMoves.Add(move);
                    }
                    RevertTempMove(board, move, piece);
                }

                return validMoves;
            }

            foreach (var move in moves)
            {
                if (!(board[move.From.I, move.From.J] is King))
                {
                    validMoves.Add(move);
                    continue;
                }

                Piece piece = board[move.To.I, move.To.J];
                MakeTempMove(board, move);
                if (!IsKingInCheck(board, isWhite))
                {
                    validMoves.Add(move);
                }
                RevertTempMove(board, move, piece);
            }
            return moves;
        }

        public static MoveValidation IsMoveValid(Move move, Piece[,] board, bool isWhite)
        {
            MoveGeneratorCallsCounter++;
            bool isValid = false;
            
            Piece piece = null;
            if (move.From != null)
            {
                piece = board[move.To.I, move.To.J];
                MakeTempMove(board, move);
            }

            List<Move> opponentMoves = GetAllMoves(board, !isWhite);
            if (!IsKingInCheck(board, isWhite, opponentMoves))
            {
                isValid = true;
            }
            if (move.From != null)
                RevertTempMove(board, move, piece);
            return new MoveValidation{IsValid = isValid, Moves = opponentMoves};
        }

        public static List<Move> GetAllMoves(Piece[,] board, bool isWhite)
        {
            MoveGeneratorCallsCounter++;
            List<Move> moves = new List<Move>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] != null && board[i, j].IsWhite == isWhite)
                    {
                        moves.AddRange(board[i, j].GetValidMoves(board, board[i, j]));
                    }
                }
            }

            return moves;
        }

        public static void UpdateAllSupportingPieces(Piece[,] board, bool isWhite, SupportingPiece[,] supportingPieces)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] != null && board[i, j].IsWhite == isWhite)
                    {
                       board[i, j].UpdateSupportingPieces(board, board[i, j], supportingPieces);
                    }
                }
            }
        }

        public static bool IsKingInCheck(Piece[,] board, bool isWhite, List<Move> opponentMoves)
        {
            try
            {
                Position kingPosition = null;
                bool positionFound = false;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (board[i, j] != null)
                        {
                            if (board[i, j].IsWhite == isWhite && board[i, j] is King)
                            {
                                kingPosition = board[i, j].Position;
                                positionFound = true;
                                break;
                            }
                        }
                    }

                    if (positionFound)
                        break;
                }
               
                Move checkMove =
                    opponentMoves.FirstOrDefault(x => x.To.I == kingPosition.I && x.To.J == kingPosition.J);

                return checkMove != null;
            }
            catch (Exception ex)
            {
                using (StreamWriter writer =
                    new StreamWriter("log.txt", true))
                {
                    writer.WriteLine($"Error Occurred");
                }
                return false;
            }
        }
        public static bool IsKingInCheck(Piece[,] board, bool isWhite)
        {
            try
            {
                Position kingPosition = null;
                bool positionFound = false;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (board[i, j] != null)
                        {
                            if (board[i, j].IsWhite == isWhite && board[i, j] is King)
                            {
                                kingPosition = board[i, j].Position;
                                positionFound = true;
                                break;
                            }
                        }
                    }

                    if (positionFound)
                        break;
                }

                List<Move> opponentMoves = GetAllMoves(board, !isWhite);
                Move checkMove =
                    opponentMoves.FirstOrDefault(x => x.To.I == kingPosition.I && x.To.J == kingPosition.J);

                return checkMove != null;
            }
            catch (Exception ex)
            {
                using (StreamWriter writer =
                    new StreamWriter("log.txt", true))
                {
                    writer.WriteLine($"Error Occurred Not Used");
                }
                return false;
            }
        }
        public static MoveValidation IsKingInCheckByPiece(Piece[,] board, Piece piece)
        {
            try
            {
                Position kingPosition = null;
                bool positionFound = false;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (board[i, j] != null)
                        {
                            if (board[i, j].IsWhite == !piece.IsWhite && board[i, j] is King)
                            {
                                kingPosition = board[i, j].Position;
                                positionFound = true;
                                break;
                            }
                        }
                    }

                    if (positionFound)
                        break;
                }

                List<Move> opponentMoves = piece.GetValidMoves(board, piece);
                Move checkMove =
                    opponentMoves.FirstOrDefault(x => x.To.I == kingPosition.I && x.To.J == kingPosition.J);

                return new MoveValidation {IsValid = checkMove != null, Moves = opponentMoves};
            }
            catch (Exception ex)
            {
                using (StreamWriter writer =
                    new StreamWriter("log.txt", true))
                {
                    writer.WriteLine($"Error Occurred By Piece");
                }
                return null;
            }
        }
        static BoardData BoardValue(Piece[,] board, bool isInitial = false)
        {
            StringBuilder hash = new StringBuilder();
            int whiteValue = 0;
            int blackValue = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] != null)
                    {
                        if (board[i, j] is Pawn)
                        {
                            hash.Append("p");
                            if (board[i, j].IsWhite)
                            {
                                whiteValue += 1;
                            }
                            else
                            {
                                blackValue += 1;
                            }
                        }
                        else if (board[i, j] is Knight || board[i, j] is Bishop)
                        {
                            if (board[i, j] is Knight)
                                hash.Append("n");
                            else
                            {
                                hash.Append("b");
                            }
                            if (board[i, j].IsWhite)
                            {
                                whiteValue += 3;
                            }
                            else
                            {
                                blackValue += 3;
                            }
                        }
                        else if (board[i, j] is Rook)
                        {
                            hash.Append("r");
                            if (board[i, j].IsWhite)
                            {
                                whiteValue += 5;
                            }
                            else
                            {
                                blackValue += 5;
                            }
                        }
                        else if (board[i, j] is Queen)
                        {
                            hash.Append("q");

                            if (board[i, j].IsWhite)
                            {
                                whiteValue += 9;
                            }
                            else
                            {
                                blackValue += 9;
                            }
                        }
                        else if (board[i, j] is King)
                        {
                            hash.Append("k");
                            if (board[i, j].IsWhite)
                            {
                                whiteValue += 10000;
                            }
                            else
                            {
                                blackValue += 10000;
                            }
                        }

                        hash.Append(i);
                        hash.Append(j);
                    }
                }
            }

            if (isInitial)
            {
                WhiteValue = whiteValue;
                BlackValue = blackValue;
            }
            int finalValue = (blackValue - BlackValue) + (WhiteValue - whiteValue);
            string hashedString = hash.ToString();

            return new BoardData { Hash = hashedString, Value = finalValue };
        }

        static BestMove AlphaBetaForKillingMoves(Piece[,] board, int depth, int α, int β, bool maximizingPlayer)
        {
            if (maximizingPlayer)
            {
                int value = Int32.MinValue;
                List<Move> moves = GetAllValidMoves(board, false);
                BestMove bestMove = new BestMove();

                moves = moves.Where(x => x.IsKilling).ToList();
                if (moves.Count == 0)
                    return new BestMove { Value = BoardValue(board).Value };

                foreach (Move move in moves)
                {
                    if (move.From.I == move.To.I && move.From.J == move.To.J)
                        continue;

                    Piece piece = board[move.To.I, move.To.J];
                    board[move.To.I, move.To.J] = board[move.From.I, move.From.J];
                    board[move.To.I, move.To.J].Position.I = move.To.I;
                    board[move.To.I, move.To.J].Position.J = move.To.J;
                    board[move.From.I, move.From.J] = null;

                    value = Math.Max(value, AlphaBetaForKillingMoves(board, depth + 1, α, β, false).Value);

                    board[move.From.I, move.From.J] = board[move.To.I, move.To.J];
                    board[move.From.I, move.From.J].Position.I = move.From.I;
                    board[move.From.I, move.From.J].Position.J = move.From.J;
                    board[move.To.I, move.To.J] = piece;

                    bestMove.Value = value;

                    if (value > α)
                    {
                        bestMove.Move = move;
                    }

                    α = Math.Max(α, value);
                    if (α >= β)
                        break;
                }


                return bestMove;
            }

            else
            {
                int value = Int32.MaxValue;
                List<Move> moves = GetAllValidMoves(board, true);

                moves = moves.Where(x => x.IsKilling).ToList();
                if (moves.Count == 0)
                    return new BestMove { Value = BoardValue(board).Value };

                foreach (Move move in moves)
                {
                    if (move.From.I == move.To.I && move.From.J == move.To.J)
                        continue;

                    Piece piece = board[move.To.I, move.To.J];
                    board[move.To.I, move.To.J] = board[move.From.I, move.From.J];
                    board[move.To.I, move.To.J].Position.I = move.To.I;
                    board[move.To.I, move.To.J].Position.J = move.To.J;
                    board[move.From.I, move.From.J] = null;

                    value = Math.Min(value, AlphaBetaForKillingMoves(board, depth + 1, α, β, true).Value);

                    board[move.From.I, move.From.J] = board[move.To.I, move.To.J];
                    board[move.From.I, move.From.J].Position.I = move.From.I;
                    board[move.From.I, move.From.J].Position.J = move.From.J;
                    board[move.To.I, move.To.J] = piece;

                    β = Math.Min(β, value);
                    if (α >= β)
                        break;
                }

                return new BestMove { Value = value };
            }
        }

        static BestMove AlphaBeta(Piece[,] board, int depth, int α, int β, bool maximizingPlayer, Move _move)
        {
            Counter++;
            if (depth == 0)
            {
                BoardData boardData = BoardValue(board);
                if (HashedBoard.ContainsKey(boardData.Hash))
                    return new BestMove { Value = HashedBoard[boardData.Hash] };

                if (!_move.IsKilling)
                {
                    if (!HashedBoard.ContainsKey(boardData.Hash))
                        HashedBoard.Add(boardData.Hash, boardData.Value);

                    return new BestMove { Value = boardData.Value };
                }
                else
                {

                    BestMove bestMove = AlphaBetaForKillingMoves(board, 0, Int32.MinValue, Int32.MaxValue, true);
                    if (!HashedBoard.ContainsKey(boardData.Hash))
                        HashedBoard.Add(boardData.Hash, bestMove.Value);
                    return bestMove;
                }
            }

            if (maximizingPlayer)
            {
                int value = Int32.MinValue;
                List<Move> moves = GetAllValidMoves(board, false);
                BestMove bestMove = new BestMove();

                foreach (Move move in moves)
                {
                    if (move.From.I == move.To.I && move.From.J == move.To.J)
                        continue;

                    Piece piece = board[move.To.I, move.To.J];
                    board[move.To.I, move.To.J] = board[move.From.I, move.From.J];
                    board[move.To.I, move.To.J].Position.I = move.To.I;
                    board[move.To.I, move.To.J].Position.J = move.To.J;
                    board[move.From.I, move.From.J] = null;

                    value = Math.Max(value, AlphaBeta(board, depth - 1, α, β, false, move).Value);

                    board[move.From.I, move.From.J] = board[move.To.I, move.To.J];
                    board[move.From.I, move.From.J].Position.I = move.From.I;
                    board[move.From.I, move.From.J].Position.J = move.From.J;
                    board[move.To.I, move.To.J] = piece;

                    bestMove.Value = value;

                    if (value > α)
                    {
                        bestMove.Move = move;
                    }

                    α = Math.Max(α, value);
                    if (α >= β)
                        break;
                }


                return bestMove;
            }

            else
            {
                int value = Int32.MaxValue;
                List<Move> moves = GetAllValidMoves(board, true);

                foreach (Move move in moves)
                {
                    if (move.From.I == move.To.I && move.From.J == move.To.J)
                        continue;
                    Piece piece = board[move.To.I, move.To.J];
                    board[move.To.I, move.To.J] = board[move.From.I, move.From.J];
                    board[move.To.I, move.To.J].Position.I = move.To.I;
                    board[move.To.I, move.To.J].Position.J = move.To.J;
                    board[move.From.I, move.From.J] = null;

                    value = Math.Min(value, AlphaBeta(board, depth - 1, α, β, true, move).Value);

                    board[move.From.I, move.From.J] = board[move.To.I, move.To.J];
                    board[move.From.I, move.From.J].Position.I = move.From.I;
                    board[move.From.I, move.From.J].Position.J = move.From.J;
                    board[move.To.I, move.To.J] = piece;

                    β = Math.Min(β, value);
                    if (α >= β)
                        break;
                }

                return new BestMove { Value = value };
            }
        }

        public static void MakeTempMove(Piece[,] board, Move move)
        {
            UpdateBoardWithMove(board, move);

            if (move.From.I == 0 && move.From.I == 4 && move.From.I == 0 && move.From.I == 6)
            {
                Move move1 = new Move { From = new Position { I = 0, J = 7 }, To = new Position { I = 0, J = 5 } };
                UpdateBoardWithMove(board, move1);
            }

            if (move.From.I == 7 && move.From.I == 4 && move.From.I == 7 && move.From.I == 6)
            {
                Move move1 = new Move { From = new Position { I = 7, J = 7 }, To = new Position { I = 7, J = 5 } };
                UpdateBoardWithMove(board, move1);
            }
        }

        public static void RevertTempMove(Piece[,] board, Move move, Piece piece)
        {
            board[move.From.I, move.From.J] = board[move.To.I, move.To.J];
            board[move.From.I, move.From.J].Position.I = move.From.I;
            board[move.From.I, move.From.J].Position.J = move.From.J;
            board[move.To.I, move.To.J] = piece;

            if (move.From.I == 0 && move.From.I == 4 && move.From.I == 0 && move.From.I == 6)
            {
                Move move1 = new Move { From = new Position { I = 0, J = 5 }, To = new Position { I = 0, J = 7 } };
                UpdateBoardWithMove(board, move1);
            }

            if (move.From.I == 7 && move.From.I == 4 && move.From.I == 7 && move.From.I == 6)
            {
                Move move1 = new Move { From = new Position { I = 7, J = 7 }, To = new Position { I = 7, J = 5 } };
                UpdateBoardWithMove(board, move1);
            }
        }

        public static void UpdateBoardWithMove(Piece[,] board, Move move)
        {
            board[move.To.I, move.To.J] = board[move.From.I, move.From.J];
            board[move.To.I, move.To.J].Position.I = move.To.I;
            board[move.To.I, move.To.J].Position.J = move.To.J;
            board[move.From.I, move.From.J] = null;
        }


        public static int GetMoveCost(Move move, Piece[,] board)
        {
            if (move.PieceName == "WP")
            {
                return WP[move.To.I, move.To.J] - WP[move.From.I, move.From.J];
            }

            if (move.PieceName == "BP")
            {
                return BP[move.To.I, move.To.J] - BP[move.From.I, move.From.J];
            }

            if (move.PieceName == "WR")
            {
                return WR[move.To.I, move.To.J] - WR[move.From.I, move.From.J];
            }

            if (move.PieceName == "BR")
            {
                return BR[move.To.I, move.To.J] - BR[move.From.I, move.From.J];
            }

            if (move.PieceName == "WB")
            {
                return WB[move.To.I, move.To.J] - WB[move.From.I, move.From.J];
            }

            if (move.PieceName == "BB")
            {
                return BB[move.To.I, move.To.J] - BB[move.From.I, move.From.J];
            }

            if (move.PieceName == "WN")
            {
                return WN[move.To.I, move.To.J] - WN[move.From.I, move.From.J];
            }

            if (move.PieceName == "BN")
            {
                return BN[move.To.I, move.To.J] - BN[move.From.I, move.From.J];
            }

            if (move.PieceName == "WQ")
            {
                return WQ[move.To.I, move.To.J] - WQ[move.From.I, move.From.J];
            }

            if (move.PieceName == "BQ")
            {
                return BQ[move.To.I, move.To.J] - BQ[move.From.I, move.From.J];
            }

            if (move.PieceName == "WK")
            {
                return WK[move.To.I, move.To.J] - WK[move.From.I, move.From.J];
            }

            if (move.PieceName == "BK")
            {
                return BK[move.To.I, move.To.J] - BK[move.From.I, move.From.J];
            }

            return 0;
        }
    }
}
