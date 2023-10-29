using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    struct Square
    {
        public static Square None = new Square(-1, -1);

        public int X { get; private set; }
        public int Y { get; private set; }

        public Square(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Square(string e2)
        {
            if (e2.Length == 2 &&
                e2[0] >= 'a' &&
                e2[0] <= 'h' &&
                e2[1] >= '1' &&
                e2[1] <= '8')
            {
                X = e2[0] - 'a';
                Y = e2[1] - '1';
            }
            else
                this = None;
        }

        public bool OnBoard()
        {
            return X >= 0 && X < 8 && Y >= 0 && Y < 8;
        }
    }
}
