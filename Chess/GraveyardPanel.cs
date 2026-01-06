using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class GraveyardPanel : Panel
    {
        public PictureBox[,] squares { get; private set; }
        private static int boardHeight = 2;
        private static int boardWidth = 8;
        private static int tileSize = 30;
        public GraveyardPanel(Point p)
        {
            Location = p;
            Size = new Size(boardWidth * tileSize, boardHeight * tileSize);
            InitializeDefault();
        }
        private void InitializeDefault()
        {
            squares = new PictureBox[boardHeight, boardWidth];
            this.Paint += boardView_Paint;
            CreateBoardUI();
        }
        private void boardView_Paint(object sender, PaintEventArgs e)
        {
            // opcjonalne rysowanie
        }
        public void Clear()
        {
            for (int row = 0; row < boardHeight; row++)
                for (int col = 0; col < boardWidth; col++)
                    squares[row, col].Image = null;
        }


        private void CreateBoardUI()
        {
            for (int row = 0; row < boardHeight; row++)
            {
                for (int col = 0; col < boardWidth; col++)
                {
                    var pb = new PictureBox();
                    pb.Width = tileSize;
                    pb.Height = tileSize;
                    pb.Location = new Point(col * tileSize, row * tileSize);
                    pb.BorderStyle = BorderStyle.FixedSingle;

                    // bool isDark = (row + col) % 2 == 1;
                    // pb.BackColor = isDark ? Color.SaddleBrown : Color.Beige;
                    bool isDark = (row + col) % 2 == 1;
                    pb.BackColor = isDark ? Color.DarkSlateGray : Color.LightGray;

                    pb.Tag = (row, col);

                    Controls.Add(pb);
                    squares[row, col] = pb;
                }
            }
        }
        public void AddPiece(Piece piece)
        {
            Image img = GetPieceImage(piece);

            for (int row = 0; row < boardHeight; row++)
            {
                for (int col = 0; col < boardWidth; col++)
                {
                    if (squares[row, col].Image == null)
                    {
                        squares[row, col].Image = img;
                        squares[row, col].SizeMode = PictureBoxSizeMode.StretchImage;
                        return;
                    }
                }
            }
            Invalidate();
        }
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
}
