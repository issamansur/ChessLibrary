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

        public Board(string fen)
        {
            Fen = fen;
            Figures = new Figure[8, 8];
            Init();
            Figures = DecodeFen(fen);
        }

        void Init()
        {
            MoveColor = Color.White;
            MoveNumber = 0;
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
            next.SetFigureAt(fm.From, Figure.None);
            next.SetFigureAt(fm.To, fm.Promotion == Figure.None ? fm.Figure : fm.Promotion);
            if (MoveColor == Color.Black)
                next.MoveNumber++;
            next.MoveColor = MoveColor.FlipColor();
            return next;
        }

        Figure[,] DecodeFen(string fen)
        {
            Figure[,] figures = new Figure[8, 8];

            string coordinates = fen.Substring(0, fen.IndexOf(' '));
            string[] horisontals = coordinates.Split('/');
            
            for (int i = 0; i < 8; i++)
            {
                int vertical = 0;
                foreach (char figure in horisontals[7-i])
                {
                    if (Char.IsDigit(figure))
                        vertical += figure - '0';
                    else
                    {
                        figures[vertical, i] = (Figure)figure;
                        vertical++;
                    }
                }
            }
            return figures;
        }
    }
}
