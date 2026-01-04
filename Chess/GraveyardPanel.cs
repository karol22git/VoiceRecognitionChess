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
    }
}
