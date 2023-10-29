using System;
using System.Collections.Generic;
using System.Linq;
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

        public Board(string fen)
        {
            Fen = fen;
            Figures = new Figure[8, 8];
            Init();
            string[] parts = fen.Split();
            if (parts.Length != 6)
                throw new Exception("Invalid FEN string: " + fen);

            MoveColor = parts[1] == "b" ? Color.Black : Color.White;
            MoveNumber = int.Parse(parts[5]);
        }

        void Init()
        {
            MoveColor = Color.White;
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
            next.SetFigureAt(fm.From, Figure.None);
            next.SetFigureAt(fm.To, fm.Promotion == Figure.None ? fm.Figure : fm.Promotion);
            if (MoveColor == Color.Black)
                next.MoveNumber++;
            next.MoveColor = MoveColor.FlipColor();
            return next;
        }
    }
}
