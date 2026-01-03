using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess;
namespace Chess
{
    public class Board
    {
        public Piece[,] Squares { get; private set; }

        public Board(int boardWidth = 8, int boardHeight = 8)
        {
            Squares = new Piece[boardWidth, boardHeight];
            //SetupInitialPosition();
        }

        public void Move(string from, string to)
        {
        }

        public bool IsLegalMove(string from, string to)
        {
            return true;
        }
    }

}
