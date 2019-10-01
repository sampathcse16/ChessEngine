using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChessAlgorithm;
using ChessMoves;

namespace ChessEngine
{
    public partial class Form1 : Form
    {
        
        delegate void textboxDelegate(string s);
        delegate void makeMoveDelegate(CustomPanel customPanel, CustomPictureBox customPictureBox, bool val);
        private bool isRightMove = false;
        Random random = new Random();
        private CustomPanel[,] _chessBoardPanels;
        private Piece[,] board = new Piece[8,8];
        private Dictionary<int,CustomPanel> customPanelsDictionary = new Dictionary<int, CustomPanel>();

        public Dictionary<int, long> depthKeys = new Dictionary<int, long>();
             
        HashSet<long> randomNumbers = new HashSet<long>();
        Dictionary<long, ZorbistData> zorbistKeys = new Dictionary<long, ZorbistData>();
        Dictionary<long, ZorbistData> previousZorbistKeys = new Dictionary<long, ZorbistData>();
        Dictionary<string, int> nonMaximizerMovesCost = new Dictionary<string, int>();
        Dictionary<string, int> maximizerMovesCost = new Dictionary<string, int>();

        Cell[,] boradValues = new Cell[8, 8];

        const int tileSize = 60;
        const int gridSize = 8;

        private bool loadFromImage = false;
        private bool isComputerMove = false;
        private bool isWhiteTurn = true;

        private string[,] boardInput =
            {
                {"BR","  ","  ","BQ","BK","BB","  ","BR"},
                {"BP","BP","BP","  ","  ","BP","BP","BP"},
                {"  ","  ","BN","  ","  ","BN","  ","  "},
                {"  ","  ","  ","WP","BP","  ","  ","  "},
                {"  ","  ","BP","  ","WP","  ","  ","  "},
                {"  ","  ","WN","  ","WB","WP","  ","  "},
                {"WP","WP","  ","  ","  ","WP","  ","WP"},
                {"WR","  ","  ","WQ","WK","WB","  ","WR"}
            };
        string[,] boardToLoad = new string[8,8];

        public Form1()
        {
            UpdateDepthKeys();
            TransformBoard();
            InitializeComponent();
            LoadChessBoard();
        }



        public void LoadChessBoard()
        {
            InitilaizeBoardValues();

            var color1 = Color.Gray;
            var color2 = Color.White;
            int counter = 0;
            _chessBoardPanels = new CustomPanel[gridSize, gridSize];

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    
                    var newPanel = new CustomPanel()
                    {
                        Size = new Size(tileSize, tileSize),
                        Location = new Point(tileSize * i, tileSize * j),
                        AllowDrop = true
                    };

                    CustomPictureBox pictureBox = new CustomPictureBox();
                    counter++;
                    
                    if (counter % 8 != 0)
                    {
                        newPanel.I = gridSize - counter % gridSize;
                        newPanel.J = counter / gridSize;
                    }
                    else
                    {
                        newPanel.I = 0;
                        newPanel.J = (counter / gridSize) - 1;
                    }

                    pictureBox.I = newPanel.I;
                    pictureBox.J = newPanel.J;
                    pictureBox.Size = new Size(tileSize, tileSize);
                    customPanelsDictionary.Add(newPanel.I*10+newPanel.J, newPanel);

                    if(loadFromImage)
                    UpdateBoard(pictureBox, boardToLoad);
                    else
                    {
                        UpdateBoard(pictureBox);
                    }
                    pictureBox.MouseDown += Panel_MouseDown;
                    newPanel.Controls.Add(pictureBox);
                    foreach (Control c in newPanel.Controls)
                    {
                        c.MouseDown += c_MouseDown;
                    }
                    newPanel.DragOver += panel1_DragOver;
                    newPanel.DragDrop += panel1_DragDrop;

                    // add to Form's Controls so that they show up
                    Controls.Add(newPanel);

                    // add to our 2d array of panels for future use
                    _chessBoardPanels[i, j] = newPanel;

                    // color the backgrounds
                    if (i % 2 == 0)
                        newPanel.BackColor = j % 2 != 0 ? color1 : color2;
                    else
                        newPanel.BackColor = j % 2 != 0 ? color2 : color1;
                }
            }

        }

        public void UpdateBoard(CustomPictureBox pictureBox, string[,] boardToLoad = null)
        {

            if (boardToLoad == null)
            {
                UpdateBoardWithDefaultValue(pictureBox);
            }
            else
            {
                if(!string.IsNullOrWhiteSpace(boardToLoad[pictureBox.I, pictureBox.J]))
                pictureBox.Image = GetPieceImage(boardToLoad[pictureBox.I, pictureBox.J]);
                board[pictureBox.I, pictureBox.J] = GetPositionPieceInstace(pictureBox.I, pictureBox.J, boardToLoad[pictureBox.I, pictureBox.J]);
            }
        }

        public void UpdateBoardWithDefaultValue(CustomPictureBox pictureBox)
        {
            if ((pictureBox.I == 0 && pictureBox.J == 0) || (pictureBox.I == 0 && pictureBox.J == 7))
            {
                pictureBox.Image = GetPieceImage("WR");
                board[pictureBox.I, pictureBox.J] = GetPositionPieceInstace(pictureBox.I, pictureBox.J, "WR");
            }

            if ((pictureBox.I == 7 && pictureBox.J == 0) || (pictureBox.I == 7 && pictureBox.J == 7))
            {
                pictureBox.Image = GetPieceImage("BR");
                board[pictureBox.I, pictureBox.J] = GetPositionPieceInstace(pictureBox.I, pictureBox.J, "BR");
            }

            if ((pictureBox.I == 0 && pictureBox.J == 1) || (pictureBox.I == 0 && pictureBox.J == 6))
            {
                pictureBox.Image = GetPieceImage("WN");
                board[pictureBox.I, pictureBox.J] = GetPositionPieceInstace(pictureBox.I, pictureBox.J, "WN");
            }

            if ((pictureBox.I == 7 && pictureBox.J == 1) || (pictureBox.I == 7 && pictureBox.J == 6))
            {
                pictureBox.Image = GetPieceImage("BN");
                board[pictureBox.I, pictureBox.J] = GetPositionPieceInstace(pictureBox.I, pictureBox.J, "BN");
            }

            if ((pictureBox.I == 0 && pictureBox.J == 2) || (pictureBox.I == 0 && pictureBox.J == 5))
            {
                pictureBox.Image = GetPieceImage("WB");
                board[pictureBox.I, pictureBox.J] = GetPositionPieceInstace(pictureBox.I, pictureBox.J, "WB");
            }

            if ((pictureBox.I == 7 && pictureBox.J == 2) || (pictureBox.I == 7 && pictureBox.J == 5))
            {
                pictureBox.Image = GetPieceImage("BB");
                board[pictureBox.I, pictureBox.J] = GetPositionPieceInstace(pictureBox.I, pictureBox.J, "BB");
            }

            if (pictureBox.I == 0 && pictureBox.J == 3)
            {
                pictureBox.Image = GetPieceImage("WQ");
                board[pictureBox.I, pictureBox.J] = GetPositionPieceInstace(pictureBox.I, pictureBox.J, "WQ");
            }

            if (pictureBox.I == 7 && pictureBox.J == 3)
            {
                pictureBox.Image = GetPieceImage("BQ");
                board[pictureBox.I, pictureBox.J] = GetPositionPieceInstace(pictureBox.I, pictureBox.J, "BQ");
            }

            if (pictureBox.I == 0 && pictureBox.J == 4)
            {
                pictureBox.Image = GetPieceImage("WK");
                board[pictureBox.I, pictureBox.J] = GetPositionPieceInstace(pictureBox.I, pictureBox.J, "WK");
            }

            if (pictureBox.I == 7 && pictureBox.J == 4)
            {
                pictureBox.Image = GetPieceImage("BK");
                board[pictureBox.I, pictureBox.J] = GetPositionPieceInstace(pictureBox.I, pictureBox.J, "BK");
            }

            if (pictureBox.I == 1)
            {
                pictureBox.Image = GetPieceImage("WP");
                board[pictureBox.I, pictureBox.J] = GetPositionPieceInstace(pictureBox.I, pictureBox.J, "WP"); 
            }

            if (pictureBox.I == 6)
            {
                pictureBox.Image = GetPieceImage("BP");
                board[pictureBox.I, pictureBox.J] = GetPositionPieceInstace(pictureBox.I, pictureBox.J, "BP");
            }
        }

        public Image GetPieceImage(string pieceName)
        {
            if (pieceName == "WP")
            {
                return new Bitmap(Image.FromFile("images\\white_pawn.png"), new Size(tileSize, tileSize));
            }

            if (pieceName == "WB")
            {
                return new Bitmap(Image.FromFile("images\\white_bishop.png"), new Size(tileSize, tileSize));
            }

            if (pieceName == "WN")
            {
                return new Bitmap(Image.FromFile("images\\white_knight.png"), new Size(tileSize, tileSize));
            }

            if (pieceName == "WQ")
            {
                return new Bitmap(Image.FromFile("images\\white_queen.png"), new Size(tileSize, tileSize));
            }

            if (pieceName == "WK")
            {
                return new Bitmap(Image.FromFile("images\\white_king.png"), new Size(tileSize, tileSize));
            }

            if (pieceName == "WR")
            {
                return new Bitmap(Image.FromFile("images\\white_rook.png"), new Size(tileSize, tileSize));
            }

            if (pieceName == "BP")
            {
                return new Bitmap(Image.FromFile("images\\black_pawn.png"), new Size(tileSize, tileSize));
            }

            if (pieceName == "BB")
            {
                return new Bitmap(Image.FromFile("images\\black_bishop.png"), new Size(tileSize, tileSize));
            }

            if (pieceName == "BN")
            {
                return new Bitmap(Image.FromFile("images\\black_knight.png"), new Size(tileSize, tileSize));
            }

            if (pieceName == "BQ")
            {
                return new Bitmap(Image.FromFile("images\\black_queen.png"), new Size(tileSize, tileSize));
            }

            if (pieceName == "BK")
            {
                return new Bitmap(Image.FromFile("images\\black_king.png"), new Size(tileSize, tileSize));
            }

            if (pieceName == "BR")
            {
                return new Bitmap(Image.FromFile("images\\black_rook.png"), new Size(tileSize, tileSize));
            }

            return null;
        }

        public Piece GetPositionPieceInstace(int i, int j, string pieceName)
        {
            if (pieceName == "WP")
            {
                return new Pawn { Position = new Position { I = i, J = j }, IsWhite = true };
            }

            if (pieceName == "WB")
            {
                return new Bishop { Position = new Position { I = i, J = j }, IsWhite = true };
            }

            if (pieceName == "WN")
            {
                return new Knight { Position = new Position { I = i, J = j }, IsWhite = true };
            }

            if (pieceName == "WQ")
            {
                return new Queen { Position = new Position { I = i, J = j }, IsWhite = true };
            }

            if (pieceName == "WK")
            {
                return new King { Position = new Position { I = i, J = j }, IsWhite = true };
            }

            if (pieceName == "WR")
            {
                return new Rook { Position = new Position { I = i, J = j }, IsWhite = true };
            }

            if (pieceName == "BP")
            {
                return new Pawn { Position = new Position { I = i, J = j }, IsWhite = false };
            }

            if (pieceName == "BB")
            {
                return new Bishop { Position = new Position { I = i, J = j }, IsWhite = false };
            }

            if (pieceName == "BN")
            {
                return new Knight { Position = new Position { I = i, J = j }, IsWhite = false };
            }

            if (pieceName == "BQ")
            {
                return new Queen { Position = new Position { I = i, J = j }, IsWhite = false };
            }

            if (pieceName == "BK")
            {
                return new King { Position = new Position { I = i, J = j }, IsWhite = false };
            }

            if (pieceName == "BR")
            {
                return new Rook { Position = new Position { I = i, J = j }, IsWhite = false };
            }

            return null;
        }

        public void TransformBoard()
        {
            for (int i = 7, k=0; i >= 0; i--,k++)
            {
                for (int j = 0; j < 8; j++)
                {
                    boardToLoad[k, j] = boardInput[i, j];
                }
            }
        }

        public void UpdateDepthKeys()
        {
            for (int i = 1; i <= 100; i++)
            {
                depthKeys.Add(i,GetNextRandomNumber(random, randomNumbers));
            }
        }

        public void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            CustomPictureBox pictureBox = (CustomPictureBox)sender;
            if (pictureBox.Image == null || board[pictureBox.I, pictureBox.J]!=null && isWhiteTurn !=board[pictureBox.I,pictureBox.J].IsWhite) return;
            ClearCurrentPiecePanelColors();
            pictureBox.BackColor = Color.Wheat;
        }

        void c_MouseDown(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            c.DoDragDrop(c, DragDropEffects.Move);
        }

        void panel1_DragDrop(object sender, DragEventArgs e)
        {
            CustomPanel panel = (CustomPanel)sender;
            Control c = e.Data.GetData(e.Data.GetFormats()[0]) as Control;
            if (c != null)
            {
                MakeMove(panel, (CustomPictureBox)c);
            }
        }

        void MakeMove(CustomPanel _panel, CustomPictureBox _pictureBox, bool isTesting =false)
        {
            CustomPanel panel = (CustomPanel)_panel;
            CustomPictureBox pictureBox = (CustomPictureBox)_pictureBox;

            if (pictureBox.Image == null || (panel.I == pictureBox.I && panel.J == pictureBox.J)
                                         || !IsValidMove(new Position { I = pictureBox.I, J = pictureBox.J }, new Position { I = panel.I, J = panel.J })
                                         || !MoveGenerator.IsMoveValid(new Move{From = new Position { I = pictureBox.I, J = pictureBox.J }, To = new Position { I = panel.I, J = panel.J } }, board, board[pictureBox.I, pictureBox.J].IsWhite).IsValid
                                         || isWhiteTurn != board[pictureBox.I, pictureBox.J].IsWhite)
            {
                if (pictureBox.Image != null)
                    pictureBox.BackColor = Color.Transparent;
                return;
            }

            ClearPanelColors();
            bool isCastling = (pictureBox.I == 0 && pictureBox.J == 4 && panel.I == 0 && panel.J == 6)||
                              (pictureBox.I == 7 && pictureBox.J == 4 && panel.I == 7 && panel.J == 6);


            CustomPanel previousPanel = customPanelsDictionary[pictureBox.I * 10 + pictureBox.J];
            CustomPictureBox perviousPictureBox = (CustomPictureBox)panel.Controls[0];
            perviousPictureBox.I = previousPanel.I;
            perviousPictureBox.J = previousPanel.J;
            perviousPictureBox.Image = null;
            perviousPictureBox.BackColor = Color.Wheat;
            perviousPictureBox.BorderStyle = BorderStyle.FixedSingle;
            previousPanel.Controls.Add(perviousPictureBox);

            pictureBox.I = panel.I;
            pictureBox.J = panel.J;
            pictureBox.BackColor = Color.Wheat;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            panel.Controls.Clear();
            panel.Controls.Add(pictureBox);

            if (isWhiteTurn && isCastling)
            {
                CustomPanel rookPanel = customPanelsDictionary[0*10 + 7];
                CustomPanel rookTargetPanel = customPanelsDictionary[0 * 10 + 5];
                CustomPictureBox rookPictureBox = (CustomPictureBox)rookPanel.Controls[0];

                CustomPictureBox rookPreviousPictureBox = (CustomPictureBox)rookTargetPanel.Controls[0];
                rookPreviousPictureBox.I = rookPanel.I;
                rookPreviousPictureBox.J = rookPanel.J;
                rookPreviousPictureBox.Image = null;
                rookPreviousPictureBox.BackColor = Color.Wheat;
                rookPreviousPictureBox.BorderStyle = BorderStyle.FixedSingle;
                rookPanel.Controls.Clear();
                rookPanel.Controls.Add(rookPreviousPictureBox);

                
                rookPictureBox.I = 0;
                rookPictureBox.J = 5;
                rookTargetPanel.Controls.Clear();
                rookTargetPanel.Controls.Add(rookPictureBox);

                Piece piece = board[0, 7];
                board[0,7] = null;
                piece.Position.I = 0;
                piece.Position.J = 5;
                board[rookTargetPanel.I, rookTargetPanel.J] = piece;
            }

            if (!isWhiteTurn && isCastling)
            {
                CustomPanel rookPanel = customPanelsDictionary[7 * 10 + 7];
                CustomPanel rookTargetPanel = customPanelsDictionary[7 * 10 + 5];
                CustomPictureBox rookPictureBox = (CustomPictureBox)rookPanel.Controls[0];
                CustomPictureBox rookPreviousPictureBox = (CustomPictureBox)rookTargetPanel.Controls[0];
                rookPreviousPictureBox.I = rookPanel.I;
                rookPreviousPictureBox.J = rookPanel.J;
                rookPreviousPictureBox.Image = null;
                rookPreviousPictureBox.BackColor = Color.Wheat;
                rookPreviousPictureBox.BorderStyle = BorderStyle.FixedSingle;
                rookPanel.Controls.Clear();
                rookPanel.Controls.Add(rookPreviousPictureBox);
                
                
               
                rookPictureBox.I = 7;
                rookPictureBox.J = 5;
                rookTargetPanel.Controls.Clear();
                rookTargetPanel.Controls.Add(rookPictureBox);

                Piece piece = board[7, 7];
                board[7,7] = null;
                piece.Position.I = 7;
                piece.Position.J = 5;
                board[rookTargetPanel.I, rookTargetPanel.J] = piece;
            }

            if (!isTesting)
            {
                Piece piece = board[perviousPictureBox.I, perviousPictureBox.J];
                board[perviousPictureBox.I, perviousPictureBox.J] = null;
                piece.Position.I = panel.I;
                piece.Position.J = panel.J;
                board[panel.I, panel.J] = piece;
            }

            isWhiteTurn = !isWhiteTurn;
            if (isWhiteTurn)
            {
                whiteRadioButton.Checked = true;
            }
            else
            {
                blackRadioButton.Checked = true;
            }
            MovesCounter++;
            UpdateComputerMoveStatus();
            if (isComputerMove && !isTesting)
            {
                MakeComputerMove();
            }

        }
       
        void MakeComputerMove()
        {
            
            var watch = System.Diagnostics.Stopwatch.StartNew();
            BestMove bestMove = null;
            Thread thread = new Thread(() =>{
               bestMove = GetBestMove(board, 4, Int32.MinValue, Int32.MaxValue, true,4, isWhiteTurn);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                using (StreamWriter writer =
                    new StreamWriter("log.txt", true))
                {
                    writer.WriteLine($"Moves Count::{AllMovesCounter}");
                    writer.WriteLine($"Time::{elapsedMs}ms");
                    writer.WriteLine($"Move Generator Calls Count::{MoveGenerator.MoveGeneratorCallsCounter}");
                }
                CustomPanel previousPanel = customPanelsDictionary[bestMove.Move.From.I * 10 + bestMove.Move.From.J];
                CustomPictureBox perviousPictureBox = (CustomPictureBox)previousPanel.Controls[0];
                CustomPanel panel = customPanelsDictionary[bestMove.Move.To.I * 10 + bestMove.Move.To.J];
                this.Invoke(new textboxDelegate(UpdateTitle), $"Chess:: {bestMove.Value}");
                this.Invoke(new makeMoveDelegate(MakeMove), panel, perviousPictureBox, false);
            });
             
            thread.Start();
           
        }

        public void UpdateTitle(string text)
        {
            this.Text = text;
        }

        public void UpdateMoveCost(List<Move> allMoves, long zorbistKey, bool maximizingPlayer, bool isWhite, bool includePositionalCost= true)
        {
            SupportingPiece[,] supportingPieceInfo = new SupportingPiece[8, 8];
            MoveGenerator.UpdateAllSupportingPieces(board, !isWhite, supportingPieceInfo);

            foreach (Move move in allMoves)
            {
                if (move.IsKilling && supportingPieceInfo[move.To.I, move.To.J] == null)
                {
                    move.Cost = board[move.To.I, move.To.J].GetPieceCost(board[move.To.I, move.To.J]) * 100;
                    if (!maximizingPlayer)
                    {
                        move.Cost = move.Cost * -1;
                    }
                }
                else if (move.IsKilling && supportingPieceInfo[move.To.I, move.To.J] != null)
                {
                    int currentPieceValue = board[move.From.I, move.From.J]
                        .GetPieceCost(board[move.From.I, move.From.J]);
                    int opponentPieceValue = board[move.To.I, move.To.J].GetPieceCost(board[move.To.I, move.To.J]);
                    if (opponentPieceValue > currentPieceValue)
                    {
                        move.Cost = (opponentPieceValue - currentPieceValue) * 100;
                        if (!maximizingPlayer)
                        {
                            move.Cost = move.Cost * -1;
                        }
                    }

                    if (opponentPieceValue < currentPieceValue)
                    {
                        move.Cost = (currentPieceValue - opponentPieceValue) * -1*100;

                        if (!maximizingPlayer)
                        {
                            move.Cost = move.Cost * -1;
                        }
                    }
                }
                else if (supportingPieceInfo[move.From.I, move.From.J] != null || supportingPieceInfo[move.To.I, move.To.J] != null)
                {
                    int currentPieceValue = board[move.From.I, move.From.J]
                        .GetPieceCost(board[move.From.I, move.From.J]);
                    if (supportingPieceInfo[move.From.I, move.From.J] != null)
                    {
                        foreach (Piece piece in supportingPieceInfo[move.From.I, move.From.J]?.Pieces)
                        {
                            int opponentPieceValue = piece.GetPieceCost(piece);
                            if (opponentPieceValue < currentPieceValue)
                            {
                                move.Cost = currentPieceValue*100;

                                if (!maximizingPlayer)
                                {
                                    move.Cost = move.Cost * -1;
                                }
                            }
                        }
                    }

                    if (supportingPieceInfo[move.To.I, move.To.J] != null)
                    {
                        foreach (Piece piece in supportingPieceInfo[move.To.I, move.To.J]?.Pieces)
                        {
                            int opponentPieceValue = piece.GetPieceCost(piece);
                            if (opponentPieceValue < currentPieceValue)
                            {
                                move.Cost = currentPieceValue*-1*100;
                                if (!maximizingPlayer)
                                {
                                    move.Cost = move.Cost * -1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    move.Cost = MoveGenerator.GetMoveCost(move, board);
                }
                //zorbistKey ^= boradValues[move.From.I, move.From.J].Pieces[move.PieceName];
                //if (board[move.To.I, move.To.J] != null)
                //{
                //    zorbistKey ^= boradValues[move.To.I, move.To.J].Pieces[board[move.To.I, move.To.J].GetPieceName(board[move.To.I, move.To.J])];
                //}
                //zorbistKey ^= boradValues[move.To.I, move.To.J].Pieces[move.PieceName];

                //if (zorbistKeys.ContainsKey(zorbistKey))
                //{
                //    move.Cost += zorbistKeys[zorbistKey].Value * 101;
                //}

            }
        }
        public void UpdateMoveLatestCost(List<Move> allMoves, bool maximizingPlayer, int depth, bool isWhite)
        {
            SupportingPiece[,] supportingPieceInfo = new SupportingPiece[8, 8];
            MoveGenerator.UpdateAllSupportingPieces(board, !isWhite, supportingPieceInfo);
            foreach (Move move in allMoves)
            {
                if (move.PieceName == "WQ")
                {

                }
                bool found = false;


                //if (!found)
                {
                    if (move.IsKilling && supportingPieceInfo[move.To.I, move.To.J] == null)
                    {
                        move.Cost = board[move.To.I, move.To.J].GetPieceCost(board[move.To.I, move.To.J]);
                        if (!maximizingPlayer)
                        {
                            move.Cost = move.Cost * -1;
                        }
                    }
                    else if (move.IsKilling && supportingPieceInfo[move.To.I, move.To.J] != null)
                    {
                        int currentPieceValue = board[move.From.I, move.From.J]
                            .GetPieceCost(board[move.From.I, move.From.J]);
                        int opponentPieceValue = board[move.To.I, move.To.J].GetPieceCost(board[move.To.I, move.To.J]);
                        if (opponentPieceValue > currentPieceValue)
                        {
                            move.Cost = opponentPieceValue - currentPieceValue;

                            if (!maximizingPlayer)
                            {
                                move.Cost = move.Cost * -1;
                            }
                        }

                        if (opponentPieceValue < currentPieceValue)
                        {
                            move.Cost = (currentPieceValue - opponentPieceValue)*-1;

                            if (!maximizingPlayer)
                            {
                                move.Cost = move.Cost * -1;
                            }
                        }
                    }
                    else if (supportingPieceInfo[move.From.I, move.From.J] != null || supportingPieceInfo[move.To.I, move.To.J] != null)
                    {
                        int currentPieceValue = board[move.From.I, move.From.J]
                            .GetPieceCost(board[move.From.I, move.From.J]);
                        if (supportingPieceInfo[move.From.I, move.From.J] != null)
                        {
                            foreach (Piece piece in supportingPieceInfo[move.From.I, move.From.J]?.Pieces)
                            {
                                int opponentPieceValue = piece.GetPieceCost(piece);
                                if (opponentPieceValue < currentPieceValue)
                                {
                                    move.Cost = currentPieceValue;

                                    if (!maximizingPlayer)
                                    {
                                        move.Cost = move.Cost * -1;
                                    }
                                }
                            }
                        }

                        if (supportingPieceInfo[move.To.I, move.To.J] != null)
                        {
                            foreach (Piece piece in supportingPieceInfo[move.To.I, move.To.J]?.Pieces)
                            {
                                int opponentPieceValue = piece.GetPieceCost(piece);
                                if (opponentPieceValue < currentPieceValue)
                                {
                                    move.Cost = currentPieceValue * -1;
                                    if (!maximizingPlayer)
                                    {
                                        move.Cost = move.Cost * -1;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string key = $"{depth}{move.From.I}{move.From.J}{move.To.I}{move.To.J}{move.IsKilling}{board[move.To.I, move.To.J]?.GetPieceName(board[move.To.I, move.To.J])}";
                        if (!maximizingPlayer)
                        {
                            if (nonMaximizerMovesCost.ContainsKey(key))
                            {
                                move.Cost = nonMaximizerMovesCost[key];
                                found = true;
                            }
                        }
                        else
                        {
                            if (maximizerMovesCost.ContainsKey(key))
                            {
                                move.Cost = maximizerMovesCost[key];
                                found = true;
                            }
                        }
                    }
                }
            }
        }
        bool IsValidMove(Position from, Position to)
        {
            Piece piece = board[from.I, from.J];
            if (piece != null)
            {
                List<Move> moves = piece.GetValidMoves(board, piece);
                foreach (var move in moves)
                {
                    if (move.To.I == to.I && move.To.J == to.J)
                        return true;
                }
            }
            return false;
        }

        void panel1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void validMovesHighlighter_Click(object sender, EventArgs e)
        {
            ClearPanelColors();
            Piece[,] board = new Piece[8,8];

            foreach (var panel in _chessBoardPanels)
            {
                CustomPictureBox pictureBox = (CustomPictureBox)panel.Controls[0];
                if (pictureBox.Image != null)
                {
                    board[panel.I,panel.J] = new King{Position = new Position{I = panel.I, J= panel.J}, IsWhite = true};
                    List<Move> validMoves = new King().GetValidMoves(board, board[panel.I, panel.J]);
                    HighLightPanels(validMoves);
                }
            }
        }

        public void HighLightPanels(List<Move> validMoves)
        {
            foreach (var move in validMoves)
            {
                int key = move.To.I * 10 + move.To.J;
                CustomPanel panel = customPanelsDictionary[key];
                CustomPictureBox pictureBox = (CustomPictureBox)panel.Controls[0];
                pictureBox.BackColor = Color.Green;
                pictureBox.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        public void ClearPanelColors()
        {
            foreach (var panel in _chessBoardPanels)
            {
                CustomPictureBox pictureBox = (CustomPictureBox)panel.Controls[0];
                pictureBox.BackColor = Color.Transparent;
                pictureBox.BorderStyle = BorderStyle.None;
            }
        }

        public void ClearCurrentPiecePanelColors()
        {
            foreach (var panel in _chessBoardPanels)
            {
                CustomPictureBox pictureBox = (CustomPictureBox)panel.Controls[0];
                if (board[pictureBox.I, pictureBox.J] != null &&
                    board[pictureBox.I, pictureBox.J].IsWhite == isWhiteTurn)
                {
                    pictureBox.BackColor = Color.Transparent;
                    pictureBox.BorderStyle = BorderStyle.None;
                }
            }
        }

        public void TogglePanelDragDrop()
        {
            foreach (var panel in _chessBoardPanels)
            {
                CustomPictureBox pictureBox = (CustomPictureBox)panel.Controls[0];
                if (board[pictureBox.I, pictureBox.J] != null)
                {
                    if(board[pictureBox.I, pictureBox.J].IsWhite == isWhiteTurn)
                    {
                        panel.AllowDrop = true;
                    }
                    else
                    {
                        panel.AllowDrop = false;
                    }
                }
            }
        }

        /*****************************************************************************************************************************/

        public static int WhiteValue { get; set; }
        public static int BlackValue { get; set; }
        public static int Counter { get; set; }
        public static int MovesCounter { get; set; }
        public static int AllMovesCounter { get; set; }
        public static Dictionary<string, int> HashedBoard = new Dictionary<string, int>();
        public BestMove GetBestMove(Piece[,] board, int depth, int α, int β, bool maximizingPlayer, int intialDepth, bool isWhite)
        {
            Counter = 0;
            AllMovesCounter = 0;
            MoveGenerator.MoveGeneratorCallsCounter = 0;
            HashedBoard.Clear();
            nonMaximizerMovesCost.Clear();
            maximizerMovesCost.Clear();
            BoardValue(board, isWhite,true);
            long zorbistKey = GenerateZorbistKey();
            previousZorbistKeys = zorbistKeys;
            zorbistKeys = new Dictionary<long, ZorbistData>();
            return AlphaBeta(board, depth, α, β, maximizingPlayer, null, zorbistKey, isWhite, new Stack<string>(), null,intialDepth);
        }

        public  List<Move> GetAllValidMoves(Piece[,] board, bool isWhite)
        {
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

         BoardData BoardValue(Piece[,] board, bool isWhite, bool isInitial = false)
        {
            //Counter++;
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
                           
                            if (board[i, j].IsWhite)
                            {
                                whiteValue += 10000;
                            }
                            else
                            {
                                blackValue += 10000;
                            }
                        }

                       
                       
                    }
                }
            }

            if (isInitial)
            {
                WhiteValue = whiteValue;
                BlackValue = blackValue;
            }
            int finalValue = 0;

            if (!isWhite)
            {
                finalValue = (blackValue - BlackValue) + (WhiteValue - whiteValue);
            }
            else
            {
                finalValue = (whiteValue - WhiteValue) + (BlackValue - blackValue);
            }
            

            return new BoardData { Value = finalValue };
        }

        BestMove AlphaBetaForKillingMoves(Piece[,] board, int depth, int α, int β, bool maximizingPlayer, long zorbistKey, Move _move, bool isWhite, Stack<string> movePath, bool runAllMoves=false, List<Move> currentMoves=null)
        {
            
            Counter++;
            AllMovesCounter++;
            if (depth==9 && runAllMoves)
                return new BestMove { Value = BoardValue(board, isWhite).Value };

            if (maximizingPlayer)
            {
                int value = Int32.MinValue;
                List<Move> moves = null;
                if (currentMoves != null)
                {
                    moves = currentMoves;
                }
                else
                {
                    moves = MoveGenerator.GetAllMoves(board, isWhite);
                }
                BestMove bestMove = new BestMove();
                if(runAllMoves)
                    moves = moves.Where(x => x.IsKilling).ToList();
                else
                moves = moves.Where(x => x.IsKilling && x.To.I == _move.To.I && x.To.J==_move.To.J).ToList();
               
                if (moves.Count == 0)
                    return new BestMove { Value = BoardValue(board, isWhite).Value };

                UpdateMoveLatestCost(moves, maximizingPlayer, depth, isWhite);
                moves = moves.OrderByDescending(x=>x.Cost).ToList();
                //List<string> stackList = movePath.ToList();
                //if (stackList[stackList.Count - 1] == "5534" && stackList[stackList.Count - 2] == "3175"&& stackList[stackList.Count - 3] == "1201" && stackList[stackList.Count - 4] == "0001")
                //{
                //    if (depth == 5 && _move.From.I == 0 && _move.From.J == 0 && _move.To.I == 0 && _move.To.J == 1)
                //    {
                //        using (StreamWriter writer =
                //            new StreamWriter("log.txt", true))
                //        {
                //            writer.WriteLine(
                //                $"#######################################################################################");
                //        }
                //    }
                //}

                int count = moves.Count;
                for (int i = 0; i < count; i++)
                {
                   moves.Add(new Move());
                }

                foreach (Move move in moves)
                {
                    MoveValidation moveValidation = null;

                    //if (move.From != null)
                    {
                        moveValidation =  MoveGenerator.IsMoveValid(move, board, isWhite);
                        if (!moveValidation.IsValid)
                        {
                            continue;
                        }
                    }
                    if (move.From != null && move.IsKilling && board[move.To.I, move.To.J] is King)
                    {

                    }
                    if (move.From != null && move.From.I == move.To.I && move.From.J == move.To.J)
                        continue;

                    //long zorbistKeyBackup = zorbistKey;
                    //if (move.From != null)
                    //{
                    //    zorbistKey ^= boradValues[move.From.I, move.From.J].Pieces[move.PieceName];
                    //    if (board[move.To.I, move.To.J] != null)
                    //    {
                    //        zorbistKey ^= boradValues[move.To.I, move.To.J]
                    //            .Pieces[board[move.To.I, move.To.J].GetPieceName(board[move.To.I, move.To.J])];
                    //    }

                    //    zorbistKey ^= boradValues[move.To.I, move.To.J].Pieces[move.PieceName];
                    //    //zorbistKey ^= depthKeys[depth];
                    //}

                    //if (false && zorbistKeys.ContainsKey(zorbistKey))
                    //{
                    //    value = Math.Max(value, zorbistKeys[zorbistKey].Value);
                    //    zorbistKey = zorbistKeyBackup;
                    //}
                    //else
                    {
                        Piece piece = null;
                        if (move.From != null)
                        {
                            piece = board[move.To.I, move.To.J];
                            MakeTempMove(move);
                        }
                        //movePath.Push($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::");
                        int tempValue =
                            AlphaBetaForKillingMoves(board, depth + 1, α, β, false, zorbistKey, _move, isWhite, movePath, runAllMoves, moveValidation!=null? moveValidation.Moves:null).Value;
                        value = Math.Max(value,tempValue);
                        //stackList = movePath.ToList();
                        //if (stackList[stackList.Count - 1] == "5534" && stackList[stackList.Count - 2] == "3175" &&
                        //    stackList[stackList.Count - 3] == "1201" && stackList[stackList.Count - 4] == "0001")
                        //{
                        //    if (move.From != null && depth == 5 && _move.From.I == 0 && _move.From.J == 0 && _move.To.I == 0 &&
                        //        _move.To.J == 1)
                        //    {
                        //        using (StreamWriter writer =
                        //            new StreamWriter("log.txt", true))
                        //        {
                        //            writer.WriteLine(
                        //                $"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::{tempValue}");
                        //        }
                        //    }
                        //}

                        //if (!zorbistKeys.ContainsKey(zorbistKey))
                        //{
                        //    string position = string.Empty;
                        //    //for (int i = 7; i >= 0; i--)
                        //    //{
                        //    //    for (int j = 0; j < 8; j++)
                        //    //    {
                        //    //        if (board[i, j] != null)
                        //    //            position += board[i, j].GetPieceName(board[i, j]) + ",";
                        //    //        else
                        //    //        {
                        //    //            position += "\"\",";
                        //    //        }
                        //    //    }

                        //    //    position += "\n";
                        //    //}
                        //    //zorbistKeys.Add(zorbistKey, new ZorbistData { BoardPosition = position, Value = value, Path = string.Join("",movePath)});
                        //}
                        ////movePath.Pop();
                        //zorbistKey = zorbistKeyBackup;
                        if (move.From != null)
                        {
                            string key = $"{depth}{move.From.I}{move.From.J}{move.To.I}{move.To.J}{move.IsKilling}{piece?.GetPieceName(piece)}";
                            if (!maximizerMovesCost.ContainsKey(key))
                                maximizerMovesCost.Add(key, tempValue);
                            else
                            {
                                maximizerMovesCost[key] = tempValue;
                            }
                            RevertTempMove(move, piece);
                        }
                    }

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
                List<Move> moves = null;
                if (currentMoves != null)
                {
                    moves = currentMoves;
                }
                else
                {
                    moves = MoveGenerator.GetAllMoves(board, !isWhite);
                }

                if (runAllMoves)
                    moves = moves.Where(x => x.IsKilling).ToList();
                else
                    moves = moves.Where(x => x.IsKilling && x.To.I == _move.To.I && x.To.J == _move.To.J).ToList();
                if (moves.Count == 0)
                    return new BestMove { Value = BoardValue(board, isWhite).Value };
                UpdateMoveLatestCost(moves, maximizingPlayer, depth, !isWhite);
                moves = moves.OrderBy(x => x.Cost).ToList();
                int count = moves.Count;
                for (int i = 0; i < count; i++)
                {
                    moves.Add(new Move());
                }

                foreach (Move move in moves)
                {
                    MoveValidation moveValidation = null;

                    //if (move.From != null)
                    {
                        moveValidation = MoveGenerator.IsMoveValid(move, board, !isWhite);
                        if (!moveValidation.IsValid)
                        {
                            continue;
                        }
                    }

                    
                    if (move.From != null && move.IsKilling && board[move.To.I, move.To.J] is King)
                    {

                    }
                    if (move.From != null && move.From.I == move.To.I && move.From.J == move.To.J)
                        continue;

                    //long zorbistKeyBackup = zorbistKey;
                    //if (move.From != null)
                    //{
                    //    zorbistKey ^= boradValues[move.From.I, move.From.J].Pieces[move.PieceName];
                    //    if (board[move.To.I, move.To.J] != null)
                    //    {
                    //        zorbistKey ^= boradValues[move.To.I, move.To.J]
                    //            .Pieces[board[move.To.I, move.To.J].GetPieceName(board[move.To.I, move.To.J])];
                    //    }

                    //    zorbistKey ^= boradValues[move.To.I, move.To.J].Pieces[move.PieceName];
                    //    //zorbistKey ^= depthKeys[depth];
                    //}

                    //if (false && zorbistKeys.ContainsKey(zorbistKey))
                    //{
                    //    value = Math.Min(value, zorbistKeys[zorbistKey].Value);
                    //    zorbistKey = zorbistKeyBackup;
                    //}
                    //else
                    {
                        Piece piece = null;
                        if (move.From != null)
                        {
                            piece = board[move.To.I, move.To.J];
                            MakeTempMove(move);
                        }

                       
                        //movePath.Push($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::");
                        int tempValue =
                            AlphaBetaForKillingMoves(board, depth + 1, α, β, true, zorbistKey, _move, isWhite, movePath, runAllMoves, moveValidation != null ? moveValidation.Moves : null).Value;

                        

                        value = Math.Min(value, tempValue);
                       
                        //if (!zorbistKeys.ContainsKey(zorbistKey))
                        //{
                        //    string position = string.Empty;
                        //    //for (int i = 7; i >= 0; i--)
                        //    //{
                        //    //    for (int j = 0; j < 8; j++)
                        //    //    {
                        //    //        if (board[i, j] != null)
                        //    //            position += board[i, j].GetPieceName(board[i, j]) + ",";
                        //    //        else
                        //    //        {
                        //    //            position += "\"\",";
                        //    //        }
                        //    //    }

                        //    //    position += "\n";
                        //    //}
                        //    //zorbistKeys.Add(zorbistKey, new ZorbistData { BoardPosition = position, Value = value, Path = string.Join("", movePath) });
                        //}
                        ////movePath.Pop();
                        //zorbistKey = zorbistKeyBackup;
                        if (move.From != null)
                        {
                            string key = $"{depth}{move.From.I}{move.From.J}{move.To.I}{move.To.J}{move.IsKilling}{piece?.GetPieceName(piece)}";
                            if (!nonMaximizerMovesCost.ContainsKey(key))
                                nonMaximizerMovesCost.Add(key, tempValue);
                            else
                            {
                                nonMaximizerMovesCost[key] = tempValue;
                            }
                            RevertTempMove(move, piece);
                        }
                    }

                    β = Math.Min(β, value);
                    if (α >= β)
                        break;
                }

                return new BestMove { Value = value };
            }
        }

        BestMove AlphaBeta(Piece[,] board, int depth, int α, int β, bool maximizingPlayer, Move _move, long zorbistKey,bool isWhite, Stack<string> movePath, List<Move> currentMoves=null, int initialDepth =4)
        {
          
            Counter++;
            AllMovesCounter++;
            if (depth == 0)
            {
                if (_move != null && _move.From.I == 1 && _move.From.J == 4 && _move.To.I == 4 && _move.To.J == 1)
                {

                }

                MoveValidation mooveValidation = MoveGenerator.IsKingInCheckByPiece(board, board[_move.To.I, _move.To.J]);
                if (mooveValidation.IsValid)
                {
                    BestMove move = AlphaBetaForMoreMoves(board, depth + 2, α, β, maximizingPlayer, _move, zorbistKey, isWhite, movePath, null);
                    return move;
                }


               
                if (!_move.IsKilling)
                {
                    return new BestMove { Value = BoardValue(board, isWhite).Value };
                    //BestMove bestMove = AlphaBetaForKillingMoves(board, 5, Int32.MinValue, Int32.MaxValue, maximizingPlayer, zorbistKey,
                    //    _move, isWhite, movePath, true, null);
                    //return bestMove;
                }
                else
                {
                    List<Move> moves = MoveGenerator.GetAllMoves(board, isWhite);
                    BestMove bestMove = AlphaBetaForKillingMoves(board, 5, Int32.MinValue, Int32.MaxValue, maximizingPlayer, zorbistKey, _move, isWhite, movePath, false, moves);
                    //using (StreamWriter writer =
                    //    new StreamWriter("log.txt", true))
                    //{
                    //    writer.WriteLine($"{movePath}={bestMove.Value}");
                    //}
                    if (bestMove.Value < 0)
                    {
                        //BoardData boardData = BoardValue(board);
                        //if (boardData.Value < 0)
                        //{
                        //    return bestMove;
                        //}
                        bestMove = AlphaBetaForKillingMoves(board, 5, Int32.MinValue, Int32.MaxValue, true, zorbistKey,
                            _move, isWhite, movePath, true, moves);
                    }

                    return bestMove;
                }
            }

            if (maximizingPlayer)
            {

                int value = Int32.MinValue;
                List<Move> moves = null;
                if(currentMoves != null)
                {
                    moves = currentMoves;
                }
                else
                {
                    moves = MoveGenerator.GetAllMoves(board, isWhite);
                }

                if (depth == initialDepth)
                {
                    //AlphaBetaForInitialMove(board, 2, α, β, maximizingPlayer, null, zorbistKey, isWhite, new Stack<string>(), null, 2);
                    UpdateMoveCost(moves, zorbistKey, maximizingPlayer, isWhite);
                    moves = moves.OrderByDescending(x => x.Cost).ToList();
                }
                else
                {
                    UpdateMoveLatestCost(moves, maximizingPlayer, depth, isWhite);
                    moves = moves.OrderByDescending(x => x.Cost).ToList();
                }

                BestMove bestMove = new BestMove();
                bestMove.Value = value;
                List<Move> allMoves = null;
                if (depth == initialDepth)
                {
                    List<string> stackList = movePath.ToList();
                    

                    if (allMoves == null)
                    {
                        allMoves = new List<Move>();
                    }
                    using (StreamWriter writer =
                        new StreamWriter("log.txt", true))
                    {
                        writer.WriteLine($"#######################################################################################");
                    }
                }

                if (depth == 2)
                {

                    List<string> stackList = movePath.ToList();
                    //if (stackList[stackList.Count - 1] == "5534" && stackList[stackList.Count - 2] == "3175")
                    {
                        //using (StreamWriter writer =
                        //    new StreamWriter("log.txt", true))
                        //{
                        //    writer.WriteLine($"{_move.PieceName}->({_move.From.I},{_move.From.J}) to ({_move.To.I},{_move.To.J})::α = {α}, β={β}");
                        //    writer.WriteLine($"###################################################################################");
                        //}
                    }
                }

                foreach (Move move in moves)
                {
                    if(MovesCounter==1 && move.PieceName=="BN" &&  move.From.I == 7 && move.From.J == 1 && move.To.I == 5 && move.To.J == 2)
                    {
                        continue;
                    }
                    MoveValidation moveValidation = MoveGenerator.IsMoveValid(move, board, isWhite);
                    if (!moveValidation.IsValid)
                    {
                        continue;
                    }


                    if (depth==4 && move.From.I == 5 && move.From.J == 2 && move.To.I == 4 && move.To.J == 0)
                    {
                        isRightMove = true;
                    }
                    else
                    {
                        isRightMove = false;
                    }

                    if (depth == 2)
                    {
                        if (move.From.I == 5 && move.From.J == 5 && move.To.I == 6 && move.To.J == 3)
                        {
                                isRightMove = true;
                        }
                        else
                        {
                                isRightMove = false;
                        }
                    }

                    //long zorbistKeyBackup = zorbistKey;
                    //zorbistKey ^= boradValues[move.From.I, move.From.J].Pieces[move.PieceName];
                    //if (board[move.To.I, move.To.J] != null)
                    //{
                    //    zorbistKey ^= boradValues[move.To.I, move.To.J].Pieces[board[move.To.I, move.To.J].GetPieceName(board[move.To.I, move.To.J])];
                    //}
                    //zorbistKey ^= boradValues[move.To.I, move.To.J].Pieces[move.PieceName];
                    //zorbistKey ^= depthKeys[depth];
                    //if (false && zorbistKeys.ContainsKey(zorbistKey))
                    //{
                    //    value = Math.Max(value, zorbistKeys[zorbistKey].Value);
                    //    zorbistKey = zorbistKeyBackup;
                    //}
                    //else
                    {
                        Piece piece = board[move.To.I, move.To.J];
                        MakeTempMove(move);
                        //movePath.Push($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::");
                        movePath.Push($"{move.From.I}{move.From.J}{move.To.I}{move.To.J}");
                        int tempValue = AlphaBeta(board, depth - 1, α, β, false, move, zorbistKey, isWhite, movePath, moveValidation.Moves, initialDepth).Value;
                        
                        value = Math.Max(value, tempValue);
                        if (depth == initialDepth)
                        {
                            move.Value = tempValue;
                            allMoves.Add(move);
                            using (StreamWriter writer =
                                new StreamWriter("log.txt", true))
                            {
                                writer.WriteLine($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::{tempValue}==>{Counter}");
                                Counter=0;
                            }
                        }
                        else
                        {
                            string key = $"{depth}{move.From.I}{move.From.J}{move.To.I}{move.To.J}{move.IsKilling}{piece?.GetPieceName(piece)}";
                            if (!maximizerMovesCost.ContainsKey(key))
                            maximizerMovesCost.Add(key, tempValue);
                            else
                            {
                                maximizerMovesCost[key] = tempValue;
                            }
                        }
                        if (depth == 2)
                        {
                            List<string> stackList = movePath.ToList();
                            //if (stackList[stackList.Count - 1] == "5534" && stackList[stackList.Count - 2] == "3175")
                            {
                                //using (StreamWriter writer =
                                //    new StreamWriter("log.txt", true))
                                //{
                                //    writer.WriteLine($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::{tempValue}");
                                //}
                            }
                        }


                        //if (!zorbistKeys.ContainsKey(zorbistKey))
                        //{
                        //    string position = string.Empty;
                        //    //for (int i = 7; i >=0; i--)
                        //    //{
                        //    //    for (int j = 0; j<8; j++)
                        //    //    {
                        //    //        if (board[i, j] != null)
                        //    //            position += board[i, j].GetPieceName(board[i, j]) + ",";
                        //    //        else
                        //    //        {
                        //    //            position += "\"\",";
                        //    //        }
                        //    //    }

                        //    //    position += "\n";
                        //    //}
                        //    zorbistKeys.Add(zorbistKey, new ZorbistData { BoardPosition = position, Value = tempValue, Path = string.Join("", movePath) });
                        //}

                        movePath.Pop();
                        //zorbistKey = zorbistKeyBackup;

                        RevertTempMove(move, piece);
                    }

                   
                    bestMove.Value = value;

                    if (value > α)
                    {
                        bestMove.Move = move;
                    }

                    α = Math.Max(α, value);
                    if (α >= β)
                        break;
                }


                if (depth == initialDepth)
                {
                    bestMove.AllMoves = allMoves;
                }
                return bestMove;
            }

            else
            {
                int value = Int32.MaxValue;
                List<Move> moves = null;
                if (currentMoves != null)
                {
                    moves = currentMoves;
                }
                else
                {
                    moves = MoveGenerator.GetAllMoves(board, !isWhite);
                }
                UpdateMoveLatestCost(moves, maximizingPlayer, depth, !isWhite);
                moves = moves.OrderBy(x => x.Cost).ToList();

                //if (depth == 1)
                //{
                //    bool isRightMove = true;
                //    List<string> stackList = movePath.ToList();
                //    if (stackList[stackList.Count-1]=="6151" && stackList[stackList.Count-2]=="2031" && stackList[stackList.Count - 3] == "7071")
                //    {
                //        using (StreamWriter writer =
                //            new StreamWriter("log.txt", true))
                //        {
                //            writer.WriteLine($"{_move.PieceName}->({_move.From.I},{_move.From.J}) to ({_move.To.I},{_move.To.J})");
                //            writer.WriteLine($"###################################################################################");
                //        }
                //    }
                //}

                foreach (Move move in moves)
                {
                    MoveValidation moveValidation = MoveGenerator.IsMoveValid(move, board, !isWhite);
                    if (!moveValidation.IsValid)
                    {
                        continue;
                    }

                    if (move.IsKilling && board[move.To.I, move.To.J] is King)
                    {

                    }
                    if (depth == 3)
                    {
                        if (move.PieceName == "WQ" && move.From.I == 0 && move.From.J == 3 && move.To.I == 3 && move.To.J == 0)
                        {
                            isRightMove = true;
                        }
                        else
                        {
                            isRightMove = false;
                        }
                    }

                    if (depth == 1)
                    {
                        if (move.PieceName == "WQ" && move.From.I == 3&& move.From.J == 0 && move.To.I == 4 && move.To.J == 0)
                        {

                        }
                    }

                    // movePath+=$"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::";

                    //long zorbistKeyBackup = zorbistKey;
                    //zorbistKey ^= boradValues[move.From.I, move.From.J].Pieces[move.PieceName];
                    //if (board[move.To.I, move.To.J] != null)
                    //{
                    //    zorbistKey ^= boradValues[move.To.I, move.To.J].Pieces[board[move.To.I, move.To.J].GetPieceName(board[move.To.I, move.To.J])];
                    //}
                    //zorbistKey ^= boradValues[move.To.I, move.To.J].Pieces[move.PieceName];
                    ////zorbistKey ^= depthKeys[depth]; 
                    //if (false && zorbistKeys.ContainsKey(zorbistKey))
                    //{
                    //    value = Math.Min(value, zorbistKeys[zorbistKey].Value);
                    //    zorbistKey = zorbistKeyBackup;
                    //}
                    //else
                    {
                        Piece piece = board[move.To.I, move.To.J];
                        MakeTempMove(move);
                        //movePath.Push($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::");
                        movePath.Push($"{move.From.I}{move.From.J}{move.To.I}{move.To.J}");
                        int tempValue = AlphaBeta(board, depth - 1, α, β, true, move, zorbistKey, isWhite, movePath, moveValidation.Moves, initialDepth).Value;

                        if (tempValue < 0) { }
                        string key = $"{depth}{move.From.I}{move.From.J}{move.To.I}{move.To.J}{move.IsKilling}{piece?.GetPieceName(piece)}";

                        if (!nonMaximizerMovesCost.ContainsKey(key))
                            nonMaximizerMovesCost.Add(key, tempValue);
                        else
                        {
                            nonMaximizerMovesCost[key] = tempValue;
                        }

                        if (depth == 3 && tempValue<=0)
                        {

                        }

                        //if (depth == 1)
                        //{
                        //    List<string> stackList = movePath.ToList();
                        //    if (stackList[stackList.Count - 1] == "6151" && stackList[stackList.Count - 2] == "2031" && stackList[stackList.Count - 3] == "7071")
                        //    {
                        //        using (StreamWriter writer =
                        //            new StreamWriter("log.txt", true))
                        //        {
                        //            writer.WriteLine($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::{tempValue}");
                        //        }
                        //    }
                        //}

                        value = Math.Min(value, tempValue);
                        
                        //if (!zorbistKeys.ContainsKey(zorbistKey))
                        //{
                        //    string position = string.Empty;
                        //    //for (int i = 7; i >= 0; i--)
                        //    //{
                        //    //    for (int j = 0; j < 8; j++)
                        //    //    {
                        //    //        if (board[i, j] != null)
                        //    //            position += board[i, j].GetPieceName(board[i, j]) + ",";
                        //    //        else
                        //    //        {
                        //    //            position += "\"\",";
                        //    //        }
                        //    //    }

                        //    //    position += "\n";
                        //    //}
                        //    zorbistKeys.Add(zorbistKey, new ZorbistData { BoardPosition = position, Value = tempValue, Path = string.Join("", movePath) });
                        //}

                        movePath.Pop();
                        //zorbistKey = zorbistKeyBackup;
                        RevertTempMove(move, piece);
                        
                    }

                    β = Math.Min(β, value);
                    if (α >= β)
                        break;
                }

                return new BestMove { Value = value };
            }
        }

        BestMove AlphaBetaForMoreMoves(Piece[,] board, int depth, int α, int β, bool maximizingPlayer, Move _move, long zorbistKey,bool isWhite, Stack<string> movePath, List<Move> currentMoves=null)
        {

            Counter++;
            AllMovesCounter++;
            if (depth == 0)
            {
                if (_move != null && _move.From.I == 1 && _move.From.J == 4 && _move.To.I == 4 && _move.To.J == 1)
                {

                }
                if (!_move.IsKilling)
                {
                    BoardData boardData = BoardValue(board, isWhite);
                    //using (StreamWriter writer =
                    //    new StreamWriter("log.txt", true))
                    //{
                    //    writer.WriteLine($"{movePath}={boardData.Value}");
                    //}

                    return new BestMove { Value = boardData.Value };
                }
                else
                {
                    BestMove bestMove = AlphaBetaForKillingMoves(board, 5, Int32.MinValue, Int32.MaxValue, maximizingPlayer, zorbistKey, _move, isWhite, movePath);
                    //using (StreamWriter writer =
                    //    new StreamWriter("log.txt", true))
                    //{
                    //    writer.WriteLine($"{movePath}={bestMove.Value}");
                    //}
                    if (bestMove.Value < 0)
                    {
                        //BoardData boardData = BoardValue(board);
                        //if (boardData.Value < 0)
                        //{
                        //    return bestMove;
                        //}
                        bestMove = AlphaBetaForKillingMoves(board, 5, Int32.MinValue, Int32.MaxValue, maximizingPlayer, zorbistKey,
                            _move, isWhite, movePath, true);
                    }

                    return bestMove;
                }
            }

            if (maximizingPlayer)
            {
                int value = Int32.MinValue;
                List<Move> moves = null;
                if (currentMoves != null)
                {
                    moves = currentMoves;
                }
                else
                {
                    moves = MoveGenerator.GetAllMoves(board, isWhite);
                }

                UpdateMoveLatestCost(moves, maximizingPlayer, depth, isWhite);

                moves = moves.OrderByDescending(x => x.Cost).ToList();
                BestMove bestMove = new BestMove();
                bestMove.Value = value;
                List<Move> allMoves = null;
                if (depth == 4)
                {
                    if (allMoves == null)
                    {
                        allMoves = new List<Move>();
                    }
                    //using (StreamWriter writer =
                    //    new StreamWriter("log.txt", true))
                    //{
                    //    writer.WriteLine($"#######################################################################################");
                    //}
                }

                if (depth == 2)
                {
                    List<string> stackList = movePath.ToList();
                    if (stackList[stackList.Count - 1] == "6151" && stackList[stackList.Count - 2] == "2031")
                    {
                        using (StreamWriter writer =
                            new StreamWriter("log.txt", true))
                        {
                            writer.WriteLine($"{_move.PieceName}->({_move.From.I},{_move.From.J}) to ({_move.To.I},{_move.To.J})::α = {α}, β={β}");
                            writer.WriteLine($"###################################################################################");
                        }
                    }
                }

                foreach (Move move in moves)
                {
                    MoveValidation moveValidation = MoveGenerator.IsMoveValid(move, board, isWhite);
                    if (!moveValidation.IsValid)
                    {
                        continue;
                    }
                    if (depth == 4 && move.From.I == 6 && move.From.J == 1 && move.To.I == 5 && move.To.J == 1)
                    {
                        isRightMove = true;
                    }
                    else
                    {
                        isRightMove = false;
                    }

                    if (depth == 2)
                    {
                        if (isRightMove && move.From.I == 7 && move.From.J == 0 && move.To.I == 7 && move.To.J == 1)
                        {
                            isRightMove = true;
                        }
                        else
                        {
                            isRightMove = false;
                        }
                    }

                    //long zorbistKeyBackup = zorbistKey;
                    //zorbistKey ^= boradValues[move.From.I, move.From.J].Pieces[move.PieceName];
                    //if (board[move.To.I, move.To.J] != null)
                    //{
                    //    zorbistKey ^= boradValues[move.To.I, move.To.J].Pieces[board[move.To.I, move.To.J].GetPieceName(board[move.To.I, move.To.J])];
                    //}
                    //zorbistKey ^= boradValues[move.To.I, move.To.J].Pieces[move.PieceName];
                    //zorbistKey ^= depthKeys[depth];
                    //if (false && zorbistKeys.ContainsKey(zorbistKey))
                    //{
                    //    value = Math.Max(value, zorbistKeys[zorbistKey].Value);
                    //    zorbistKey = zorbistKeyBackup;
                    //}
                    //else
                    {
                        Piece piece = board[move.To.I, move.To.J];
                        MakeTempMove(move);
                        //movePath.Push($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::");
                        movePath.Push($"{move.From.I}{move.From.J}{move.To.I}{move.To.J}");
                        int tempValue = AlphaBetaForMoreMoves(board, depth - 1, α, β, false, move, zorbistKey, isWhite, movePath).Value;

                        value = Math.Max(value, tempValue);
                        if (depth == 4)
                        {
                            move.Value = tempValue;
                            allMoves.Add(move);
                            //using (StreamWriter writer =
                            //    new StreamWriter("log.txt", true))
                            //{
                            //    writer.WriteLine($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::{tempValue}");
                            //}
                        }

                        string key = $"{depth}{move.From.I}{move.From.J}{move.To.I}{move.To.J}{move.IsKilling}{piece?.GetPieceName(piece)}";
                        if (!maximizerMovesCost.ContainsKey(key))
                            maximizerMovesCost.Add(key, tempValue);
                        else
                        {
                            maximizerMovesCost[key] = tempValue;
                        }

                        if (depth == 2)
                        {
                            List<string> stackList = movePath.ToList();
                            if (stackList[stackList.Count - 1] == "6151" && stackList[stackList.Count - 2] == "2031")
                            {
                                using (StreamWriter writer =
                                    new StreamWriter("log.txt", true))
                                {
                                    writer.WriteLine($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::{tempValue}");
                                }
                            }
                        }


                        //if (!zorbistKeys.ContainsKey(zorbistKey))
                        //{
                        //    string position = string.Empty;
                        //    //for (int i = 7; i >=0; i--)
                        //    //{
                        //    //    for (int j = 0; j<8; j++)
                        //    //    {
                        //    //        if (board[i, j] != null)
                        //    //            position += board[i, j].GetPieceName(board[i, j]) + ",";
                        //    //        else
                        //    //        {
                        //    //            position += "\"\",";
                        //    //        }
                        //    //    }

                        //    //    position += "\n";
                        //    //}
                        //    //zorbistKeys.Add(zorbistKey, new ZorbistData { BoardPosition = position, Value = tempValue, Path = string.Join("", movePath) });
                        //}

                        movePath.Pop();
                        //zorbistKey = zorbistKeyBackup;

                        RevertTempMove(move, piece);
                    }


                    bestMove.Value = value;

                    if (value > α)
                    {
                        bestMove.Move = move;
                    }

                    α = Math.Max(α, value);
                    if (α >= β)
                        break;
                }


                if (depth == 4)
                {
                    bestMove.AllMoves = allMoves;
                }
                return bestMove;
            }

            else
            {
                int value = Int32.MaxValue;
                List<Move> moves = null;
                if (currentMoves != null)
                {
                    moves = currentMoves;
                }
                else
                {
                    moves = MoveGenerator.GetAllMoves(board, !isWhite);
                }
                UpdateMoveLatestCost(moves, maximizingPlayer, depth, !isWhite);
                moves = moves.OrderBy(x => x.Cost).ToList();
                //if (depth == 1)
                //{
                //    bool isRightMove = true;
                //    List<string> stackList = movePath.ToList();
                //    if (stackList[stackList.Count-1]=="6151" && stackList[stackList.Count-2]=="2031" && stackList[stackList.Count - 3] == "7071")
                //    {
                //        using (StreamWriter writer =
                //            new StreamWriter("log.txt", true))
                //        {
                //            writer.WriteLine($"{_move.PieceName}->({_move.From.I},{_move.From.J}) to ({_move.To.I},{_move.To.J})");
                //            writer.WriteLine($"###################################################################################");
                //        }
                //    }
                //}

                foreach (Move move in moves)
                {
                    MoveValidation moveValidation = MoveGenerator.IsMoveValid(move, board, !isWhite);
                    if (!moveValidation.IsValid)
                    {
                        continue;
                    }
                    if (depth == 3)
                    {
                        if (move.PieceName == "WP" && move.From.I == 2 && move.From.J == 0 && move.To.I == 3 && move.To.J == 1)
                        {
                            isRightMove = true;
                        }
                        else
                        {
                            isRightMove = false;
                        }
                    }

                    if (depth == 1)
                    {
                        if (_move.PieceName == "BR" && _move.From.I == 7 && _move.From.J == 0 && _move.To.I == 7 && _move.To.J == 1)
                        {

                        }
                    }

                    // movePath+=$"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::";

                    //long zorbistKeyBackup = zorbistKey;
                    //zorbistKey ^= boradValues[move.From.I, move.From.J].Pieces[move.PieceName];
                    //if (board[move.To.I, move.To.J] != null)
                    //{
                    //    zorbistKey ^= boradValues[move.To.I, move.To.J].Pieces[board[move.To.I, move.To.J].GetPieceName(board[move.To.I, move.To.J])];
                    //}
                    //zorbistKey ^= boradValues[move.To.I, move.To.J].Pieces[move.PieceName];
                    ////zorbistKey ^= depthKeys[depth];
                    //if (false && zorbistKeys.ContainsKey(zorbistKey))
                    //{
                    //    value = Math.Min(value, zorbistKeys[zorbistKey].Value);
                    //    zorbistKey = zorbistKeyBackup;
                    //}
                    //else
                    {
                        Piece piece = board[move.To.I, move.To.J];
                        MakeTempMove(move);
                        //movePath.Push($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::");
                        movePath.Push($"{move.From.I}{move.From.J}{move.To.I}{move.To.J}");
                        int tempValue = AlphaBetaForMoreMoves(board, depth - 1, α, β, true, move, zorbistKey, isWhite, movePath).Value;
                        if (depth == 3 && tempValue <= 0)
                        {

                        }
                        string key = $"{depth}{move.From.I}{move.From.J}{move.To.I}{move.To.J}{move.IsKilling}{piece?.GetPieceName(piece)}";
                        if (!nonMaximizerMovesCost.ContainsKey(key))
                            nonMaximizerMovesCost.Add(key, tempValue);
                        else
                        {
                            nonMaximizerMovesCost[key] = tempValue;
                        }
                        //if (depth == 1)
                        //{
                        //    List<string> stackList = movePath.ToList();
                        //    if (stackList[stackList.Count - 1] == "6151" && stackList[stackList.Count - 2] == "2031" && stackList[stackList.Count - 3] == "7071")
                        //    {
                        //        using (StreamWriter writer =
                        //            new StreamWriter("log.txt", true))
                        //        {
                        //            writer.WriteLine($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::{tempValue}");
                        //        }
                        //    }
                        //}

                        value = Math.Min(value, tempValue);

                        if (!zorbistKeys.ContainsKey(zorbistKey))
                        {
                            string position = string.Empty;
                            //for (int i = 7; i >= 0; i--)
                            //{
                            //    for (int j = 0; j < 8; j++)
                            //    {
                            //        if (board[i, j] != null)
                            //            position += board[i, j].GetPieceName(board[i, j]) + ",";
                            //        else
                            //        {
                            //            position += "\"\",";
                            //        }
                            //    }

                            //    position += "\n";
                            //}
                            //zorbistKeys.Add(zorbistKey, new ZorbistData { BoardPosition = position, Value = tempValue, Path = string.Join("", movePath) });
                        }

                        movePath.Pop();
                        //zorbistKey = zorbistKeyBackup;
                        RevertTempMove(move, piece);

                    }

                    β = Math.Min(β, value);
                    if (α >= β)
                        break;
                }

                return new BestMove { Value = value };
            }
        }
        BestMove AlphaBetaForInitialMove(Piece[,] board, int depth, int α, int β, bool maximizingPlayer, Move _move,  long zorbistKey, bool isWhite, Stack<string> movePath, List<Move> currentMoves = null, int initialDepth = 4)
        {

            Counter++;
            AllMovesCounter++;
            if (depth == 0)
            {
                if (_move != null && _move.From.I == 1 && _move.From.J == 4 && _move.To.I == 4 && _move.To.J == 1)
                {

                }

                MoveValidation mooveValidation = MoveGenerator.IsKingInCheckByPiece(board, board[_move.To.I, _move.To.J]);
                if (mooveValidation.IsValid)
                {
                    BestMove move = AlphaBetaForMoreMoves(board, depth + 2, α, β, true, _move, zorbistKey, isWhite, movePath, null);
                    return move;
                }

                if (!_move.IsKilling)
                {

                    BoardData boardData = BoardValue(board, isWhite);
                    //using (StreamWriter writer =
                    //    new StreamWriter("log.txt", true))
                    //{
                    //    writer.WriteLine($"{movePath}={boardData.Value}");
                    //}

                    return new BestMove { Value = boardData.Value };
                }
                else
                {
                    List<Move> moves = MoveGenerator.GetAllMoves(board, isWhite);
                    BestMove bestMove = AlphaBetaForKillingMoves(board, 5, Int32.MinValue, Int32.MaxValue, true, zorbistKey, _move, isWhite, movePath, false, moves);
                    //using (StreamWriter writer =
                    //    new StreamWriter("log.txt", true))
                    //{
                    //    writer.WriteLine($"{movePath}={bestMove.Value}");
                    //}
                    if (bestMove.Value < 0)
                    {
                        //BoardData boardData = BoardValue(board);
                        //if (boardData.Value < 0)
                        //{
                        //    return bestMove;
                        //}
                        bestMove = AlphaBetaForKillingMoves(board, 5, Int32.MinValue, Int32.MaxValue, true, zorbistKey,
                            _move, isWhite, movePath, true, moves);
                    }

                    return bestMove;
                }
            }

            if (maximizingPlayer)
            {

                int value = Int32.MinValue;
                List<Move> moves = null;
                if (currentMoves != null)
                {
                    moves = currentMoves;
                }
                else
                {
                    moves = MoveGenerator.GetAllMoves(board, isWhite);
                }
                if (depth == initialDepth)
                    UpdateMoveCost(moves, zorbistKey, maximizingPlayer, isWhite);
                else
                    UpdateMoveLatestCost(moves, maximizingPlayer, depth, isWhite);
                moves = moves.OrderByDescending(x => x.Cost).ToList();
                BestMove bestMove = new BestMove();
                bestMove.Value = value;
                List<Move> allMoves = null;
                if (depth == initialDepth)
                {
                    List<string> stackList = movePath.ToList();


                    if (allMoves == null)
                    {
                        allMoves = new List<Move>();
                    }
                    ////using (StreamWriter writer =
                    ////    new StreamWriter("log.txt", true))
                    ////{
                    ////    writer.WriteLine($"#######################################################################################");
                    ////}
                }

                if (depth == 2)
                {
                    List<string> stackList = movePath.ToList();
                    //if (stackList[stackList.Count - 1] == "5534" && stackList[stackList.Count - 2] == "3175")
                    {
                        //using (StreamWriter writer =
                        //    new StreamWriter("log.txt", true))
                        //{
                        //    writer.WriteLine($"{_move.PieceName}->({_move.From.I},{_move.From.J}) to ({_move.To.I},{_move.To.J})::α = {α}, β={β}");
                        //    writer.WriteLine($"###################################################################################");
                        //}
                    }
                }

                foreach (Move move in moves)
                {
                    MoveValidation moveValidation = MoveGenerator.IsMoveValid(move, board, isWhite);
                    if (!moveValidation.IsValid)
                    {
                        continue;
                    }

                    if (move.IsKilling && board[move.To.I, move.To.J] is King)
                    {

                    }

                    if (depth == 4 && move.From.I == 5 && move.From.J == 4 && move.To.I == 4 && move.To.J == 3)
                    {
                        isRightMove = true;
                    }
                    else
                    {
                        isRightMove = false;
                    }

                    if (depth == 2)
                    {
                        if (isRightMove && move.From.I == 7 && move.From.J == 0 && move.To.I == 7 && move.To.J == 1)
                        {
                            isRightMove = true;
                        }
                        else
                        {
                            isRightMove = false;
                        }
                    }

                    long zorbistKeyBackup = zorbistKey;
                    zorbistKey ^= boradValues[move.From.I, move.From.J].Pieces[move.PieceName];
                    if (board[move.To.I, move.To.J] != null)
                    {
                        zorbistKey ^= boradValues[move.To.I, move.To.J].Pieces[board[move.To.I, move.To.J].GetPieceName(board[move.To.I, move.To.J])];
                    }
                    zorbistKey ^= boradValues[move.To.I, move.To.J].Pieces[move.PieceName];
                    //zorbistKey ^= depthKeys[depth];
                    if (false && zorbistKeys.ContainsKey(zorbistKey))
                    {
                        value = Math.Max(value, zorbistKeys[zorbistKey].Value);
                        zorbistKey = zorbistKeyBackup;
                    }
                    else
                    {
                        Piece piece = board[move.To.I, move.To.J];
                        MakeTempMove(move);
                        //movePath.Push($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::");
                        movePath.Push($"{move.From.I}{move.From.J}{move.To.I}{move.To.J}");
                        int tempValue = AlphaBetaForInitialMove(board, depth - 1, α, β, false, move, zorbistKey, isWhite, movePath, moveValidation.Moves, initialDepth).Value;

                        value = Math.Max(value, tempValue);
                        if (depth == initialDepth)
                        {
                            move.Value = tempValue;
                            allMoves.Add(move);
                            //using (StreamWriter writer =
                            //    new StreamWriter("log.txt", true))
                            //{
                            //    writer.WriteLine($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::{tempValue}==>{Counter}");
                            //    Counter = 0;
                            //}
                        }
                        else
                        {
                            string key = $"{depth}{move.From.I}{move.From.J}{move.To.I}{move.To.J}{move.IsKilling}{piece?.GetPieceName(piece)}";
                            if (!maximizerMovesCost.ContainsKey(key))
                                maximizerMovesCost.Add(key, tempValue);
                            else
                            {
                                maximizerMovesCost[key] = tempValue;
                            }
                        }
                        if (depth == 2)
                        {
                            List<string> stackList = movePath.ToList();
                            //if (stackList[stackList.Count - 1] == "5534" && stackList[stackList.Count - 2] == "3175")
                            {
                                //using (StreamWriter writer =
                                //    new StreamWriter("log.txt", true))
                                //{
                                //    writer.WriteLine($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::{tempValue}");
                                //}
                            }
                        }


                        if (!zorbistKeys.ContainsKey(zorbistKey))
                        {
                            string position = string.Empty;
                            //for (int i = 7; i >=0; i--)
                            //{
                            //    for (int j = 0; j<8; j++)
                            //    {
                            //        if (board[i, j] != null)
                            //            position += board[i, j].GetPieceName(board[i, j]) + ",";
                            //        else
                            //        {
                            //            position += "\"\",";
                            //        }
                            //    }

                            //    position += "\n";
                            //}
                            zorbistKeys.Add(zorbistKey, new ZorbistData { BoardPosition = position, Value = tempValue, Path = string.Join("", movePath) });
                        }

                        movePath.Pop();
                        zorbistKey = zorbistKeyBackup;

                        RevertTempMove(move, piece);
                    }


                    bestMove.Value = value;

                    if (value > α)
                    {
                        bestMove.Move = move;
                    }

                    α = Math.Max(α, value);
                    if (α >= β)
                        break;
                }


                if (depth == initialDepth)
                {
                    bestMove.AllMoves = allMoves;
                }
                return bestMove;
            }

            else
            {
                int value = Int32.MaxValue;
                List<Move> moves = null;
                if (currentMoves != null)
                {
                    moves = currentMoves;
                }
                else
                {
                    moves = MoveGenerator.GetAllMoves(board, !isWhite);
                }
                UpdateMoveLatestCost(moves, maximizingPlayer, depth, !isWhite);
                moves = moves.OrderBy(x => x.Cost).ToList();

                //if (depth == 1)
                //{
                //    bool isRightMove = true;
                //    List<string> stackList = movePath.ToList();
                //    if (stackList[stackList.Count-1]=="6151" && stackList[stackList.Count-2]=="2031" && stackList[stackList.Count - 3] == "7071")
                //    {
                //        using (StreamWriter writer =
                //            new StreamWriter("log.txt", true))
                //        {
                //            writer.WriteLine($"{_move.PieceName}->({_move.From.I},{_move.From.J}) to ({_move.To.I},{_move.To.J})");
                //            writer.WriteLine($"###################################################################################");
                //        }
                //    }
                //}

                foreach (Move move in moves)
                {
                    MoveValidation moveValidation = MoveGenerator.IsMoveValid(move, board, !isWhite);
                    if (!moveValidation.IsValid)
                    {
                        continue;
                    }

                    if (move.IsKilling && board[move.To.I, move.To.J] is King)
                    {

                    }
                    if (depth == 3)
                    {
                        if (move.PieceName == "WQ" && move.From.I == 4 && move.From.J == 7 && move.To.I == 6 && move.To.J == 5)
                        {
                            isRightMove = true;
                        }
                        else
                        {
                            isRightMove = false;
                        }
                    }

                    if (depth == 1)
                    {
                        if (_move.PieceName == "BR" && _move.From.I == 7 && _move.From.J == 0 && _move.To.I == 7 && _move.To.J == 1)
                        {

                        }
                    }

                    // movePath+=$"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::";

                    long zorbistKeyBackup = zorbistKey;
                    zorbistKey ^= boradValues[move.From.I, move.From.J].Pieces[move.PieceName];
                    if (board[move.To.I, move.To.J] != null)
                    {
                        zorbistKey ^= boradValues[move.To.I, move.To.J].Pieces[board[move.To.I, move.To.J].GetPieceName(board[move.To.I, move.To.J])];
                    }
                    zorbistKey ^= boradValues[move.To.I, move.To.J].Pieces[move.PieceName];
                    //zorbistKey ^= depthKeys[depth]; 
                    if (false && zorbistKeys.ContainsKey(zorbistKey))
                    {
                        value = Math.Min(value, zorbistKeys[zorbistKey].Value);
                        zorbistKey = zorbistKeyBackup;
                    }
                    else
                    {
                        Piece piece = board[move.To.I, move.To.J];
                        MakeTempMove(move);
                        //movePath.Push($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::");
                        movePath.Push($"{move.From.I}{move.From.J}{move.To.I}{move.To.J}");
                        int tempValue = AlphaBetaForInitialMove(board, depth - 1, α, β, true, move, zorbistKey, isWhite, movePath, moveValidation.Moves, initialDepth).Value;
                        string key = $"{depth}{move.From.I}{move.From.J}{move.To.I}{move.To.J}{move.IsKilling}{piece?.GetPieceName(piece)}";

                        if (!nonMaximizerMovesCost.ContainsKey(key))
                            nonMaximizerMovesCost.Add(key, tempValue);
                        else
                        {
                            nonMaximizerMovesCost[key] = tempValue;
                        }

                        if (depth == 3 && tempValue <= 0)
                        {

                        }

                        //if (depth == 1)
                        //{
                        //    List<string> stackList = movePath.ToList();
                        //    if (stackList[stackList.Count - 1] == "6151" && stackList[stackList.Count - 2] == "2031" && stackList[stackList.Count - 3] == "7071")
                        //    {
                        //        using (StreamWriter writer =
                        //            new StreamWriter("log.txt", true))
                        //        {
                        //            writer.WriteLine($"{move.PieceName}->({move.From.I},{move.From.J}) to ({move.To.I},{move.To.J})::{tempValue}");
                        //        }
                        //    }
                        //}

                        value = Math.Min(value, tempValue);

                        if (!zorbistKeys.ContainsKey(zorbistKey))
                        {
                            string position = string.Empty;
                            //for (int i = 7; i >= 0; i--)
                            //{
                            //    for (int j = 0; j < 8; j++)
                            //    {
                            //        if (board[i, j] != null)
                            //            position += board[i, j].GetPieceName(board[i, j]) + ",";
                            //        else
                            //        {
                            //            position += "\"\",";
                            //        }
                            //    }

                            //    position += "\n";
                            //}
                            zorbistKeys.Add(zorbistKey, new ZorbistData { BoardPosition = position, Value = tempValue, Path = string.Join("", movePath) });
                        }

                        movePath.Pop();
                        zorbistKey = zorbistKeyBackup;
                        RevertTempMove(move, piece);

                    }

                    β = Math.Min(β, value);
                    if (α >= β)
                        break;
                }

                return new BestMove { Value = value };
            }
        }
        public void MakeTempMove(Move move)
        {
            UpdateBoardWithMove(move);

            if (move.From.I == 0 && move.From.I == 4 && move.From.I == 0 && move.From.I == 6)
            {
                Move move1 = new Move { From = new Position { I = 0, J = 7 }, To = new Position { I = 0, J = 5 } };
                UpdateBoardWithMove(move1);
            }

            if (move.From.I == 7 && move.From.I == 4 && move.From.I == 7 && move.From.I == 6)
            {
                Move move1 = new Move { From = new Position { I = 7, J = 7 }, To = new Position { I = 7, J = 5 } };
                UpdateBoardWithMove(move1);
            }
        }

        public void RevertTempMove(Move move, Piece piece)
        {
            board[move.From.I, move.From.J] = board[move.To.I, move.To.J];
            board[move.From.I, move.From.J].Position.I = move.From.I;
            board[move.From.I, move.From.J].Position.J = move.From.J;
            board[move.To.I, move.To.J] = piece;

            if (move.From.I == 0 && move.From.I == 4 && move.From.I == 0 && move.From.I == 6)
            {
                Move move1 = new Move { From = new Position { I = 0, J = 5 }, To = new Position { I = 0, J = 7 } };
                UpdateBoardWithMove(move1);
            }

            if (move.From.I == 7 && move.From.I == 4 && move.From.I == 7 && move.From.I == 6)
            {
                Move move1 = new Move { From = new Position { I = 7, J = 7 }, To = new Position { I = 7, J = 5 } };
                UpdateBoardWithMove(move1);
            }
        }

        public void UpdateBoardWithMove(Move move)
        {
            board[move.To.I, move.To.J] = board[move.From.I, move.From.J];
            board[move.To.I, move.To.J].Position.I = move.To.I;
            board[move.To.I, move.To.J].Position.J = move.To.J;
            board[move.From.I, move.From.J] = null;
        }

        public void InitilaizeBoardValues()
        {

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Cell cell = new Cell();
                    cell.Pieces = new Dictionary<string, long>();
                    cell.Pieces.Add("WR", GetNextRandomNumber(random, randomNumbers));
                    cell.Pieces.Add("WN", GetNextRandomNumber(random, randomNumbers));
                    cell.Pieces.Add("WB", GetNextRandomNumber(random, randomNumbers));
                    cell.Pieces.Add("WQ", GetNextRandomNumber(random, randomNumbers));
                    cell.Pieces.Add("WK", GetNextRandomNumber(random, randomNumbers));
                    cell.Pieces.Add("WP", GetNextRandomNumber(random, randomNumbers));
                    cell.Pieces.Add("BR", GetNextRandomNumber(random, randomNumbers));
                    cell.Pieces.Add("BN", GetNextRandomNumber(random, randomNumbers));
                    cell.Pieces.Add("BB", GetNextRandomNumber(random, randomNumbers));
                    cell.Pieces.Add("BQ", GetNextRandomNumber(random, randomNumbers));
                    cell.Pieces.Add("BK", GetNextRandomNumber(random, randomNumbers));
                    cell.Pieces.Add("BP", GetNextRandomNumber(random, randomNumbers));

                    boradValues[i, j] = cell;
                }
            }
        }

        long GetNextRandomNumber(Random random, HashSet<long> randomNumbers)
        {
            while (true)
            {
                long randomNumber = LongRandom(0, Int64.MaxValue, random);
                if (randomNumbers.Contains(randomNumber))
                    continue;
                randomNumbers.Add(randomNumber);
                return randomNumber;
            }
        }

        long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }

        long GenerateZorbistKey()
        {
            long zorbistKey = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j]==null)
                        continue;

                    zorbistKey ^= boradValues[i, j].Pieces[board[i, j].GetPieceName(board[i, j])];
                }
            }

            return zorbistKey;
        }

        private void snapShot_Click(object sender, EventArgs e)
        {
            string boardPosition = string.Empty;
            boardPosition += "{";
            for (int i = 7; i >= 0; i--)
            {
                boardPosition += "{";
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] != null)
                        boardPosition += "\""+board[i, j].GetPieceName(board[i, j]) + "\"";
                    else
                    {
                        boardPosition += "\"  \"";
                    }

                    if (j != 7)
                    {
                        boardPosition += ",";
                    }

                }

                boardPosition += "}";
                if (i != 0)
                {
                    boardPosition += ",";
                }

                boardPosition += "\n";
            }

            boardPosition += "}";
            snapShotTextBox.Text = boardPosition;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            isWhiteTurn = whiteRadioButton.Checked;

            UpdateComputerMoveStatus();
            if (isComputerMove)
                MakeComputerMove();
        }

        private void UpdateComputerMoveStatus()
        {
            if (radioButton1.Checked)
            {
                if (whiteRadioButton.Checked)
                {
                    isComputerMove = true;
                }
                else
                {
                    isComputerMove = false;
                }
            }
            else if (radioButton2.Checked)
            {
                if (blackRadioButton.Checked)
                {
                    isComputerMove = true;
                }
                else
                {
                    isComputerMove = false;
                }
            }
            else if (radioButton3.Checked)
            {
                isComputerMove = true;
            }
        }
    }
}
