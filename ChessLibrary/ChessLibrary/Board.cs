using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Board
    {
        public string Fen { get; private set; }
        Figure[,] Figures;
        public Color MoveColor { get; private set; }
        public int MoveNumber { get; private set; }
        public bool CanCastleA1 { get; private set; }
        public bool CanCastleH1 { get; private set; }
        public bool CanCastleA8 { get; private set; }
        public bool CanCastleH8 { get; private set; }

        public Square EnPassant { get; private set; }
        public int HalfMoveClock { get; private set; }

        public Board(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            Fen = fen;
            Figures = new Figure[8, 8];
            HalfMoveClock = 0;
            CanCastleA1 = true;
            CanCastleH1 = true;
            CanCastleA8 = true;
            CanCastleH8 = true;
            EnPassant = Square.None;

            InitByFen(Fen);
        }

        public Figure GetFigureAt(Square square)
        {
            if (!square.OnBoard())
                return Figure.None;
            return Figures[square.X, square.Y];
        }

        private void SetFigureAt(Square square, Figure figure)
        {
            if (!square.OnBoard())
                return;
            Figures[square.X, square.Y] = figure;
        }

        public Board Move(FigureMoving fm)
        {
            Board next = new Board(Fen);
            if (next.GetFigureAt(fm.From) != fm.Figure)
                return next;

            if (next.GetFigureAt(fm.From) == Figure.BlackPawn || next.GetFigureAt(EnPassant) == Figure.WhitePawn)
                next.HalfMoveClock = 0;
            else
                next.HalfMoveClock++;

            next.SetFigureAt(fm.From, Figure.None);
            next.SetFigureAt(fm.To, fm.Promotion != Figure.None ? fm.Promotion : fm.Figure);
            
            if (MoveColor == Color.Black)
                next.MoveNumber++;
            next.MoveColor = MoveColor.FlipColor();
            
            return next;
        }

        void InitByFen(string fen)
        {
            string[] parts = fen.Split();
            if (parts.Length != 6)
                return;

            // 1. Piece placement data
            string coordinates = parts[0];
            string[] horisontals = coordinates.Split('/');
            
            for (int horizonal = 0; horizonal < 8; horizonal++)
            {
                int vertical = 0;
                foreach (char figure in horisontals[7-horizonal])
                {
                    if (Char.IsDigit(figure))
                        vertical += figure - '0';
                    else
                    {
                        Figures[vertical, horizonal] = (Figure)figure;
                        vertical++;
                    }
                }
            }

            // 2. Active color
            MoveColor = parts[1] == "b" ? Color.Black : Color.White;

            // 3. Castling availability
            CanCastleA1 = parts[2].Contains('Q');
            CanCastleH1 = parts[2].Contains('K');
            CanCastleA8 = parts[2].Contains('q');
            CanCastleH8 = parts[2].Contains('k');

            // 4. En Passant
            EnPassant = parts[3] == "-" ? Square.None : new Square(parts[3]);

            // 5. HalfMove Clock
            HalfMoveClock = int.Parse(parts[4]);

            // 6. Fullmove number
            MoveNumber = int.Parse(parts[5]);
        }
    }
}
