namespace ChessLibrary
{
    public class Chess
    {
        public string Fen { get; private set; }
        Board Board { get; set;}

        public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            Fen = fen;
            Board = new Board(fen);
        }

        Chess(Board board)
        {
            Board = board;
        }

        public Chess Move(string move)
        {
            FigureMoving fm = new FigureMoving(move);
            Board nextBoard = Board.Move(fm);
            Chess nextChess = new Chess(nextBoard);
            return nextChess;
        }

        public char GetFigureAt(int x, int y)
        {
            Square square = new Square(x, y);
            Figure figure = Board.GetFigureAt(square);
            return (char)figure;
        }   
    }
}