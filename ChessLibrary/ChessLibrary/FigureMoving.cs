using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class FigureMoving
    {
        public Figure Figure { get; private set; }
        public Square From { get; private set; }
        public Square To { get; private set; }
        public Figure Promotion { get; private set; }

        public FigureMoving(
            FigureOnSquare figure,
            Square to, 
            Figure promotion = Figure.None)
        {
            Figure = figure.Figure;
            From = figure.Square;
            To = to;
            Promotion = promotion;
        }

        public FigureMoving(string move)
        {
            if (move.Length < 5 || move.Length > 6)
                throw new Exception("Invalid move format: " + move);

            Figure = (Figure)move[0];
            From = new Square(move.Substring(1, 2));
            To = new Square(move.Substring(3, 2));
            Promotion = move.Length == 6 ? (Figure)move[5] : Figure.None;
        }   
    }
}
