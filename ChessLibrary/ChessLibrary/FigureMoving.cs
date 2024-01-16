using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    public class FigureMoving
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
            if (move.Length is < 5 or > 6)
                throw new Exception("Invalid move format: " + move);

            Figure = (Figure)move[0];
            From = new Square(move.Substring(1, 2));
            To = new Square(move.Substring(3, 2));
            Promotion = move.Length == 6 ? (Figure)move[5] : Figure.None;
        }

        public int DeltaX => To.X - From.X;
        public int DeltaY => To.Y - From.Y;
        public int AbsDeltaX => Math.Abs(DeltaX);
        public int AbsDeltaY => Math.Abs(DeltaY);
        public int SignDeltaX => Math.Sign(DeltaX);
        public int SignDeltaY => Math.Sign(DeltaY);

        public override string ToString()
        {
            string move = $"{(char)Figure}{From}{To}";
            if (Promotion != Figure.None)
                move += (char)Promotion;
            return move;
        }
    }
}
