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
            squares = new PictureBox[boardWidth, boardHeight];
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
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var pb = new PictureBox();
                    pb.Width = tileSize;
                    pb.Height = tileSize;
                    pb.Location = new Point(col * tileSize, row * tileSize);
                    pb.BorderStyle = BorderStyle.FixedSingle;

                    bool isDark = (row + col) % 2 == 1;
                    pb.BackColor = isDark ? Color.SaddleBrown : Color.Beige;

                    pb.Tag = (row, col);

                    Controls.Add(pb);
                    squares[row, col] = pb;
                }
            }
        }

        private void boardView_Paint(object sender, PaintEventArgs e)
        {
            // opcjonalne rysowanie
        }
    }


}
