using ChessDotNet;
using System.Drawing;

// aliasy:
using EnginePiece = ChessDotNet.Piece;       // figura z silnika
using UiPiece = Chess.Piece;                 // Twoja enumka do rysowania

namespace Chess
{
    public class ChessGameLogic
    {
        private ChessGame game = new ChessGame();

        public bool IsMoveLegal(Point from, Point to)
        {
            Move move = CreateMove(from, to);
            return game.IsValidMove(move);
        }
        public bool TryMakeMove(Point from, Point to)
        {
            Move move = CreateMove(from, to);

            // Sprawdź, czy to pionek
            var piece = game.GetPieceAt(new Position(ToSquare(from)));
            if (piece != null && char.ToLower(piece.GetFenCharacter()) == 'p')
            {
                // Białe dochodzą na rank 8 (to.Y == 7)
                // Czarne dochodzą na rank 1 (to.Y == 0)
                if ((piece.Owner == Player.White && to.Y == 7) ||
                    (piece.Owner == Player.Black && to.Y == 0))
                {
                    // PROMOCJA — używamy CHAR, nie PieceType
                    char promotion = AskPromotionChar(piece.Owner);
                    Move promoMove = new Move(ToSquare(from), ToSquare(to), piece.Owner, promotion);

                    if (game.IsValidMove(promoMove))
                    {
                        game.MakeMove(promoMove, true);
                        return true;
                    }

                    return false;
                }
            }

            // Normalny ruch
            if (!game.IsValidMove(move))
                return false;

            game.MakeMove(move, true);
            return true;
        }
        private char AskPromotionChar(Player owner)
        {
            using (var dlg = new PromotionDialog(owner))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    return dlg.SelectedPromotionChar;

                return 'q'; // domyślnie hetman
            }
        }

        public string GetMoveNotation(Point from, Point to)
        {
            string fromStr = ToSquare(from);
            string toStr = ToSquare(to);

            // sprawdź promocję
            var piece = game.GetPieceAt(new Position(toStr));
            if (piece != null && piece.IsPromotionResult)
            {
                char promoted = char.ToUpper(piece.GetFenCharacter());
                return $"{fromStr}-{toStr}={promoted}";
            }

            return $"{fromStr}-{toStr}";
        }

        //public bool TryMakeMove(Point from, Point to)
        //{
        //    Move move = CreateMove(from, to);
        //
        //    if (!game.IsValidMove(move))
        //        return false;
        //
        //    game.MakeMove(move, true);
        //    return true;
        //}

        private Move CreateMove(Point from, Point to)
        {
            string fromStr = ToSquare(from);
            string toStr = ToSquare(to);

            return new Move(fromStr, toStr, game.WhoseTurn);
        }
        public List<Point> GetLegalMoves(Point from)
        {
            string fromStr = ToSquare(from);
            Position fromPos = new Position(fromStr);

            // Pobierz legalne ruchy z silnika
            var moves = game.GetValidMoves(fromPos);

            List<Point> result = new List<Point>();

            foreach (var move in moves)
            {
                string sq = move.NewPosition.ToString().ToLower();

                int file = sq[0] - 'a';
                int rank = sq[1] - '1';

                result.Add(new Point(file, rank));
            }

            return result;
        }


        public void SyncBoard(Board board)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    string square = ToSquare(new Point(x, y));
                    Position pos = new Position(square);
                    EnginePiece piece = game.GetPieceAt(pos);

                    if (piece == null)
                    {
                        board.Squares[x, y] = UiPiece.None;
                        continue;
                    }

                    board.Squares[x, y] = ConvertPiece(piece);
                }
            }
        }

        private UiPiece ConvertPiece(EnginePiece p)
        {
            bool white = p.Owner == Player.White;
            char fen = char.ToLower(p.GetFenCharacter());

            return fen switch
            {
                'p' => white ? UiPiece.WhitePawn : UiPiece.BlackPawn,
                'n' => white ? UiPiece.WhiteKnight : UiPiece.BlackKnight,
                'b' => white ? UiPiece.WhiteBishop : UiPiece.BlackBishop,
                'r' => white ? UiPiece.WhiteRook : UiPiece.BlackRook,
                'q' => white ? UiPiece.WhiteQueen : UiPiece.BlackQueen,
                'k' => white ? UiPiece.WhiteKing : UiPiece.BlackKing,
                _ => UiPiece.None
            };
        }


        public string CheckGameEnd()
        {
            if (game.IsCheckmated(Player.White))
                return "Czarny wygrywa (mat na białym)";

            if (game.IsCheckmated(Player.Black))
                return "Biały wygrywa (mat na czarnym)";

            if (game.IsStalemated(Player.White) || game.IsStalemated(Player.Black))
                return "Remis (pat)";

            if (game.IsDraw())
                return "Remis";

            return null; // gra trwa
        }




        public Player WhoseTurn => game.WhoseTurn;

        private string ToSquare(Point p)
        {
            char file = (char)('a' + p.X);
            int rank = p.Y + 1;
            return $"{file}{rank}";
        }
    }
}
