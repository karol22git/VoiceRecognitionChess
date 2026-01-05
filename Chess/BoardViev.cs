/*using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chess
{
    public class BoardView : Panel
    {
        public PictureBox[,] squares { get; private set; }
        public bool IsWhiteOrientation { get; set; } = true; // Ustawiamy na sztywno false

        private const int boardHeight = 8;
        private const int boardWidth = 8;
        private const int tileSize = 90;
        public Board Board { get; set; }

        public BoardView(Point p)
        {
            Location = p;
            Size = new Size(boardWidth * tileSize, boardHeight * tileSize);
            InitializeDefault();

            // DEBUG: wypisz w konsoli
            Debug.WriteLine($"Konstruktor BoardView: IsWhiteOrientation = {IsWhiteOrientation}");
        }

        private void InitializeDefault()
        {
            squares = new PictureBox[boardHeight, boardWidth];
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!DesignMode)
            {
                Debug.WriteLine("OnHandleCreated - rozpoczynam RebuildBoard");
                RebuildBoard();
            }
        }

        public void RebuildBoard()
        {
            Debug.WriteLine($"=== REBUILD BOARD ===");
            Debug.WriteLine($"IsWhiteOrientation przed rebuild: {IsWhiteOrientation}");

            Controls.Clear();
            CreateBoardUI();
            Invalidate();
        }

        private void CreateBoardUI()
        {
            // ZAWSZE rysuj w ten sam sposób - bez transformacji
            for (int row = 0; row < boardHeight; row++)
            {
                for (int col = 0; col < boardWidth; col++)
                {
                    PictureBox pb = new PictureBox();
                    pb.Width = tileSize;
                    pb.Height = tileSize;

                    // ZAWSZE: rząd 0 na górze, rząd 7 na dole
                    pb.Location = new Point(col * tileSize, row * tileSize);

                    // Kolor zawsze według (row + col)
                    bool isDark = (row + col) % 2 == 1;
                    pb.BackColor = isDark ? Color.SaddleBrown : Color.Beige;

                    pb.Tag = (row, col);
                    pb.Paint += DrawSquareNotation;

                    Controls.Add(pb);
                    squares[row, col] = pb;
                }
            }
        }

        private void DrawSquareNotation(object sender, PaintEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            var (row, col) = ((int row, int col))pb.Tag;

            string notation;

            if (IsWhiteOrientation)
            {
                // Gracz białymi: standard
                // row=7 (dół) -> 8-7=1, col=0 (lewo) -> 'a'
                notation = $"{(char)('a' + col)}{8 - row}";
            }
            else
            {
                // Gracz czarnymi: siedzi na dole, ale widzi planszę odwróconą
                // Jego lewy dolny to prawy górny z perspektywy białych

                // Odwracamy wszystko:
                char file = (char)('a' + (7 - col));  // Odwrócone kolumny
                int rank = row + 1;                    // Nie odwracamy rzędów

                notation = $"{file}{rank}";
            }

            using Font font = new Font("Consolas", 12, FontStyle.Bold);
            e.Graphics.DrawString(notation, font, Brushes.Red, 3, pb.Height - 18);

            // Dodaj test: jaki powinien być wynik
            string expected;
            if (IsWhiteOrientation)
                expected = $"{(char)('a' + col)}{8 - row}";
            else
                expected = $"{(char)('a' + (7 - col))}{row + 1}";

            e.Graphics.DrawString($"Exp: {expected}", font, Brushes.Purple, 3, 5);
        }


        // Klasa pomocnicza do debugowania
        public static class Debug
        {
            public static void WriteLine(string message)
            {
                System.Diagnostics.Debug.WriteLine(message);
                Console.WriteLine(message);
            }
        }
       // protected override void OnPaint(PaintEventArgs e)
       // {
       //     base.OnPaint(e);
       //
       //     DrawBoard(e.Graphics);
       //     DrawPieces(e.Graphics);
       // }
       // private void DrawPieces(Graphics g)
       // {
       //     int size = tileSize;
       //
       //     for (int x = 0; x < 8; x++)
       //     {
       //         for (int y = 0; y < 8; y++)
       //         {
       //             Piece piece = Board.Squares[x, y];
       //             if (piece == Piece.None)
       //                 continue;
       //
       //             Image img = GetPieceImage(piece);
       //             g.DrawImage(img, x * size, y * size, size, size);
       //         }
       //     }
       // }

   // }
    private Image GetPieceImage(Piece piece)
        {
            return piece switch
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

                _ => null
            };
        }

    }
}*/

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chess
{
    public class BoardView : Panel
    {
        public Board Board { get; set; }
        public bool IsWhiteOrientation { get; set; } = true;

        private const int tileSize = 90;

        public BoardView(Point location)
        {
            Location = location;
            Size = new Size(8 * tileSize, 8 * tileSize);
            DoubleBuffered = true; // płynne rysowanie
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

        // ============================
        // MAPOWANIE ENUM → OBRAZEK
        // ============================
        private Image GetPieceImage(Piece piece)
        {
            return piece switch
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

                _ => null
            };
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


    }
}
