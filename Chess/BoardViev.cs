using ChessDotNet;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UiPiece = Chess.Piece;
namespace Chess
{
    public class BoardView : Panel
    {
        public Board Board { get; set; }
        public bool IsWhiteOrientation { get; set; } = true;

        private const int tileSize = 90;
        private Point? selectedSquare = null;
        private ChessGameLogic gameLogic = new ChessGameLogic();
        private List<Point> legalMoves = new List<Point>();
        public event Action<UiPiece> PieceCaptured;

        public BoardView(Point location)
        {
            Location = location;
            Size = new Size(8 * tileSize, 8 * tileSize);
            //DoubleBuffered = true; // płynne rysowanie
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true); UpdateStyles();
        }

        // ============================
        // RYSOWANIE
        // ============================
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawBoard(e.Graphics);
            DrawPieces(e.Graphics);
           // DrawCoordinates(e.Graphics);
           DrawSquareCoordinates(e.Graphics);
            if (selectedSquare != null)
                HighlightSquare(e.Graphics, selectedSquare.Value);
            // RYSOWANIE ZIELONYCH OBRAMÓWEK
            using (Pen pen = new Pen(Color.FromArgb(0, 180, 0), 5))

            {
                foreach (var move in legalMoves)
                {
                    Point p = BoardToScreen(move.X, move.Y);

                    Rectangle rect = new Rectangle(
                        p.X,
                        p.Y,
                        tileSize,
                        tileSize
                    );

                    e.Graphics.DrawRectangle(pen, rect);
                }
            }


        }

        private void DrawBoard(Graphics g)
        {
            for (int file = 0; file < 8; file++)
            {
                for (int rank = 0; rank < 8; rank++)
                {
                    bool dark = (file + rank) % 2 == 0;
                    Color color = dark ? Color.SaddleBrown : Color.Beige;

                    Point p = BoardToScreen(file, rank);

                    using var brush = new SolidBrush(color);
                    g.FillRectangle(brush, p.X, p.Y, tileSize, tileSize);
                }
            }
        }

        private void DrawPieces(Graphics g)
        {
            if (Board == null)
                return;

            for (int file = 0; file < 8; file++)
            {
                for (int rank = 0; rank < 8; rank++)
                {
                    Piece piece = Board.Squares[file, rank];
                    if (piece == Piece.None)
                        continue;

                    Image img = GetPieceImage(piece);
                    if (img == null)
                        continue;

                    Point p = BoardToScreen(file, rank);
                    g.DrawImage(img, p.X, p.Y, tileSize, tileSize);
                }
            }
        }

        private void DrawCoordinates(Graphics g)
        {
            using Font font = new Font("Consolas", 12, FontStyle.Bold);

            for (int file = 0; file < 8; file++)
            {
                string letter = ((char)('a' + file)).ToString();
                Point p = BoardToScreen(file, 7);
                g.DrawString(letter, font, Brushes.Black, p.X + 3, p.Y + 3);
            }

            for (int rank = 0; rank < 8; rank++)
            {
                string number = (rank + 1).ToString();
                Point p = BoardToScreen(0, rank);
                g.DrawString(number, font, Brushes.Black, p.X + 3, p.Y + 3);
            }
        }

        // ============================
        // MAPOWANIE WSPÓŁRZĘDNYCH
        // ============================
        private Point BoardToScreen(int file, int rank)
        {
            int col, row;

            if (IsWhiteOrientation)
            {
                col = file;
                row = 7 - rank;
            }
            else
            {
                col = 7 - file;
                row = rank;
            }

            return new Point(col * tileSize, row * tileSize);
        }

        public Point? ScreenToBoard(int x, int y)
        {
            int col = x / tileSize;
            int row = y / tileSize;

            if (col < 0 || col > 7 || row < 0 || row > 7)
                return null;

            int file, rank;

            if (IsWhiteOrientation)
            {
                file = col;
                rank = 7 - row;
            }
            else
            {
                file = 7 - col;
                rank = row;
            }

            return new Point(file, rank);
        }


        private static readonly Dictionary<Piece, Image> imageCache = new();
        public event Action<string, bool> MoveMade;

        private Image GetPieceImage(Piece piece)
        {
            if (imageCache.TryGetValue(piece, out var img))
                return img;

            img = piece switch
            {
                Piece.WhitePawn => Properties.Resources.white_pawn_svg,
                Piece.WhiteKnight => Properties.Resources.white_knight_svg,
                Piece.WhiteBishop => Properties.Resources.white_bishop_svg,
                Piece.WhiteRook => Properties.Resources.white_rook_svg,
                Piece.WhiteQueen => Properties.Resources.white_queen_svg,
                Piece.WhiteKing => Properties.Resources.white_king_svg,

                Piece.BlackPawn => Properties.Resources.black_pawn_svg,
                Piece.BlackKnight => Properties.Resources.black_knight_svg,
                Piece.BlackBishop => Properties.Resources.black_bishop_svg,
                Piece.BlackRook => Properties.Resources.black_rook_svg,
                Piece.BlackQueen => Properties.Resources.black_queen_svg,
                Piece.BlackKing => Properties.Resources.black_king_svg,
            };

            imageCache[piece] = img;
            return img;
        }

        private void DrawSquareCoordinates(Graphics g)
        {
            using Font font = new Font("Consolas", 10, FontStyle.Bold);
            using Brush brush = new SolidBrush(Color.FromArgb(150, 0, 0, 0)); // Półprzezroczysty czarny

            for (int file = 0; file < 8; file++)
            {
                for (int rank = 0; rank < 8; rank++)
                {
                    // Obliczamy pozycję na ekranie
                    Point p = BoardToScreen(file, rank);

                    // Notacja szachowa
                    char fileChar = (char)('a' + file);
                    int rankNum = rank + 1;
                    string notation = $"{fileChar}{rankNum}";

                    // Rysujemy w lewym dolnym rogu pola z marginesem
                    int offsetX = 4; // Margines od lewej krawędzi
                    int offsetY = tileSize - 18; // Margines od dołu (18 pikseli od dołu)

                    g.DrawString(notation, font, brush,
                        p.X + offsetX,
                        p.Y + offsetY);
                }
            }
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (IsGameOver) return;

            if (Board == null)
                return;

            Point? boardPos = ScreenToBoard(e.X, e.Y);
            if (boardPos == null)
                return;

            Point clicked = boardPos.Value;

            // 1. Jeśli NIC nie jest wybrane – wybieramy bierkę
            if (selectedSquare == null)
            {
                Piece clickedPiece = Board.GetPiece(clicked.X, clicked.Y);

                if (clickedPiece == Piece.None)
                    return;

                // CZYSZCZENIE POPRZEDNICH OBRAMÓWEK
                legalMoves.Clear();

                selectedSquare = clicked;

                // WYLICZ LEGALNE RUCHY
                legalMoves = gameLogic.GetLegalMoves(clicked);

                Invalidate();
                return;
            }



            // 2. Jeśli coś było wybrane – próbujemy wykonać ruch
            Point from = selectedSquare.Value;
            Point to = clicked;

            // Jeśli kliknięto to samo pole – anuluj wybór
            if (from == to)
            {
                selectedSquare = null;
                legalMoves.Clear();
                Invalidate();
                return;
            }
            Piece targetPiece = Board.GetPiece(to.X, to.Y);
            bool capture = targetPiece != Piece.None;


            // Próba wykonania ruchu przez silnik
            if (gameLogic.TryMakeMove(from, to))
            {
                string notation = gameLogic.GetMoveNotation(from, to);
                bool whiteMoved = gameLogic.WhoseTurn == Player.Black;
                // bo po ruchu tura się zmienia
                if (capture)
                {
                    PieceCaptured?.Invoke(targetPiece);
                }

                MoveMade?.Invoke(notation, whiteMoved);

                gameLogic.SyncBoard(Board);
            }

            selectedSquare = null;
            legalMoves.Clear();
            Invalidate();
            var end = gameLogic.CheckGameEnd();
            if (end != null)
            {
                GameEnded?.Invoke(end.Value.winner, end.Value.reason);
                return;
            }

        }

        private void HighlightSquare(Graphics g, Point square)
        {
            Point p = BoardToScreen(square.X, square.Y);

            using Brush brush = new SolidBrush(Color.FromArgb(120, 0, 180, 0)); // półprzezroczysty zielony
            g.FillRectangle(brush, p.X, p.Y, tileSize, tileSize);
        }

        public void RestartGame()
        {
            gameLogic = new ChessGameLogic();
            Board = new Board();
            selectedSquare = null;
            legalMoves.Clear();

        }
        public event Action<GameWinner, GameEndReason> GameEnded;

        public bool IsGameOver { get;  set; } = false;

        public void DisableInteraction()
        {
            IsGameOver = true;
        }



    }
}
