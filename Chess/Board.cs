using System;
using System.Drawing;

namespace Chess
{
    public class Board
    {
        public Piece[,] Squares { get; } = new Piece[8, 8];

        public Board()
        {
            SetupInitialPosition();
        }

        public Piece GetPiece(int x, int y) => Squares[x, y];

        public void SetPiece(int x, int y, Piece piece)
        {
            Squares[x, y] = piece;
        }

        public void MovePiece(Point from, Point to)
        {
            Squares[to.X, to.Y] = Squares[from.X, from.Y];
            Squares[from.X, from.Y] = Piece.None;
        }

        private void SetupInitialPosition()
        {
            // Czyścimy planszę
            for (int file = 0; file < 8; file++)
            {
                for (int rank = 0; rank < 8; rank++)
                {
                    Squares[file, rank] = Piece.None;
                }
            }

            // BIAŁE (dół) - rangi 0 i 1
            // Pionki
            for (int file = 0; file < 8; file++)
            {
                Squares[file, 1] = Piece.WhitePawn;
            }

            // Figury białe (ranga 0)
            Squares[0, 0] = Piece.WhiteRook;      // a1
            Squares[1, 0] = Piece.WhiteKnight;    // b1
            Squares[2, 0] = Piece.WhiteBishop;    // c1
            Squares[3, 0] = Piece.WhiteQueen;     // d1 - KRÓLOWA na D1 (białym polu)
            Squares[4, 0] = Piece.WhiteKing;      // e1 - KRÓL na E1 (ciemnym polu)
            Squares[5, 0] = Piece.WhiteBishop;    // f1
            Squares[6, 0] = Piece.WhiteKnight;    // g1
            Squares[7, 0] = Piece.WhiteRook;      // h1

            // CZARNE (góra) - rangi 6 i 7
            // Pionki
            for (int file = 0; file < 8; file++)
            {
                Squares[file, 6] = Piece.BlackPawn;
            }

            // Figury czarne (ranga 7)
            Squares[0, 7] = Piece.BlackRook;      // a8
            Squares[1, 7] = Piece.BlackKnight;    // b8
            Squares[2, 7] = Piece.BlackBishop;    // c8
            Squares[3, 7] = Piece.BlackQueen;     // d8 - KRÓLOWA na D8 (ciemnym polu - dla czarnych to białe pole!)
            Squares[4, 7] = Piece.BlackKing;      // e8 - KRÓL na E8 (białym polu - dla czarnych to ciemne!)
            Squares[5, 7] = Piece.BlackBishop;    // f8
            Squares[6, 7] = Piece.BlackKnight;    // g8
            Squares[7, 7] = Piece.BlackRook;      // h8
        }
    }
}
