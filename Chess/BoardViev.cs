using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class BoardView : Panel
    {
        public PictureBox[,] squares { get; private set; }

        private static int boardHeight = 8;
        private static int boardWidth = 8;
        private static int tileSize = 90;

        public BoardView()
        {
            InitializeDefault();
        }
        public BoardView(Point p)
        {
            Location = p;
            Size = new Size(boardWidth*tileSize, boardHeight*tileSize);
            InitializeDefault();
        }
        public BoardView(Point p, Size s, int index)
        {
            Location = p;
            Size = s;
            TabIndex = index;

            InitializeDefault();
        }

        private void InitializeDefault()
        {
            squares = new PictureBox[boardHeight, boardWidth];
            this.Paint += boardView_Paint;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (!DesignMode)
            {
                Controls.Clear();
                CreateBoardUI();
            }
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

                    bool isDark = (row + col) % 2 == 1;
                    pb.BackColor = isDark ? Color.SaddleBrown : Color.Beige;

                    pb.Tag = (row, col);
                    pb.Paint += DrawSquareNotation;

                    Controls.Add(pb);
                    squares[row, col] = pb;
                }
            }
        }

        private void boardView_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using var font = new Font("Consolas", 14, FontStyle.Bold);
            using var brush = new SolidBrush(Color.Red);

            for (int row = 0; row < boardHeight; row++)
            {
                for (int col = 0; col < boardWidth; col++)
                {
                    string file = ((char)('a' + col)).ToString();
                    string rank = (8 - row).ToString(); // klasyczna notacja szachowa

                    string text = file + rank;

                    int x = col * tileSize + 4;                 // lewy dolny róg
                    int y = (row + 1) * tileSize - 20;

                    g.DrawString(text, font, brush, x, y);
                }
            }
        }
        private void DrawSquareNotation(object sender, PaintEventArgs e)
        {
            var pb = sender as PictureBox;
            var (row, col) = ((int row, int col))pb.Tag;

            string file = ((char)('a' + col)).ToString();
            string rank = (8 - row).ToString();
            string text = file + rank;

            using var font = new Font("Consolas", 12, FontStyle.Bold);
            using var brush = new SolidBrush(Color.Red);

            // lewy dolny róg
            int x = 3;
            int y = pb.Height - 18;

            e.Graphics.DrawString(text, font, brush, x, y);
        }

    }



}
