namespace ChessLibrary
{
    public struct Square: IEquatable<Square>
    {
        public static Square None = new Square(-1, -1);
        public static string Alphabet = "abcdefgh";

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
            return (X is >= 0 and < 8) && (Y is >= 0 and < 8);
        }

        public static bool operator ==(Square square1, Square square2)
        {
            return square1.Equals(square2);
        }
        
        public static bool operator !=(Square square1, Square square2)
        {
            return !square1.Equals(square2);
        }
        
        public bool Equals(Square square)
        {
            return X == square.X && Y == square.Y;
        }

        public override string ToString()
        {
            return $"{Alphabet[X]}{Y}";
        }
    }
}
