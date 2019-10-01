using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessMoves;
using NUnit.Framework;

namespace TestChessEngine
{
    public class PawnTest
    {
        public Piece pawn;

        [SetUp]
        public void Initialize()
        {
            pawn = new Pawn();
        }

        [Test]
        public void GetValidMovesCountTest()
        {
            int i = 1, j = 5;
            Piece[,] board = GetBoardWithPawnInPosition(i, j, true);
            List<Move> moves = pawn.GetValidMoves(board, board[i,j]);
            Assert.AreEqual(2, moves.Count);

            i = 2; j = 5;
            board = GetBoardWithPawnInPosition(i, j, true);
            moves = pawn.GetValidMoves(board, board[i, j]);
            Assert.AreEqual(1, moves.Count);

            i = 7; j = 5;
            board = GetBoardWithPawnInPosition(i, j, false);
            moves = pawn.GetValidMoves(board, board[i, j]);
            Assert.AreEqual(2, moves.Count);

            i = 6; j = 5;
            board = GetBoardWithPawnInPosition(i, j, false);
            moves = pawn.GetValidMoves(board, board[i, j]);
            Assert.AreEqual(1, moves.Count);

            i = 1; j = 5;
            board = GetBoardWithPawnInPosition(i, j, true);
            board[i+1,j] = new Pawn{IsWhite = false};
            moves = pawn.GetValidMoves(board, board[i, j]);
            Assert.AreEqual(0, moves.Count);

            i = 1; j = 5;
            board = GetBoardWithPawnInPosition(i, j, true);
            board[i + 2, j] = new Pawn { IsWhite = false };
            moves = pawn.GetValidMoves(board, board[i, j]);
            Assert.AreEqual(1, moves.Count);

            i = 1; j = 0;
            board = GetBoardWithPawnInPosition(i, j, true);
            board[i + 1, j + 1] = new Pawn { IsWhite = false };
            moves = pawn.GetValidMoves(board, board[i, j]);
            Assert.AreEqual(3, moves.Count);

            i = 1; j = 1;
            board = GetBoardWithPawnInPosition(i, j, true);
            board[i + 1, j + 1] = new Pawn { IsWhite = false };
            board[i + 1, j - 1] = new Pawn { IsWhite = false };
            moves = pawn.GetValidMoves(board, board[i, j]);
            Assert.AreEqual(4, moves.Count);

            i = 7; j = 5;
            board = GetBoardWithPawnInPosition(i, j, false);
            board[i - 1, j] = new Pawn { IsWhite = true };
            moves = pawn.GetValidMoves(board, board[i, j]);
            Assert.AreEqual(0, moves.Count);

            i = 7; j = 5;
            board = GetBoardWithPawnInPosition(i, j, false);
            board[i - 2, j] = new Pawn { IsWhite = true };
            moves = pawn.GetValidMoves(board, board[i, j]);
            Assert.AreEqual(1, moves.Count);

            i = 7; j = 7;
            board = GetBoardWithPawnInPosition(i, j, false);
            board[i-1, j-1] = new Pawn { IsWhite = true };
            moves = pawn.GetValidMoves(board, board[i, j]);
            Assert.AreEqual(3, moves.Count);
        }

        private Piece[,] GetBoardWithPawnInPosition(int i, int j, bool isWhite)
        {
            Piece[,] board = new Piece[8, 8];
            Piece currentPawn = new Pawn { Position = new Position { I = i, J = j }, IsWhite = isWhite };
            board[i, j] = currentPawn;
            return board;
        }
    }
}
