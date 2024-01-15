using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    public class FigureOnSquare
    {
        public Figure Figure { get; private set; }
        public Square Square { get; private set; }

        public FigureOnSquare(Figure figure, Square square)
        {
            Figure = figure;
            Square = square;
        }
    }
}
