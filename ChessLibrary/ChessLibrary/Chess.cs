namespace ChessLibrary
{
    public class Chess
    {
        public string Fen { get; private set; }

        public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            this.Fen = fen;
        }

        public Chess Move(string move)
        {
            Chess nextChess = new Chess(Fen);
            return nextChess;
        }

        public char GetFigureAt(int x, int y)
        {
            return '.';
        }   
    }
}