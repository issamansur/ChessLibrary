namespace ChessLibrary
{
    public class Chess
    {
        public string Fen { get; private set; }
        private Board Board { get; set;}
        private Moves Moves { get; init; }
        private List<FigureMoving> FigureMovings { get; init; }
        
        private bool IsNoMoves() => FigureMovings.Count == 0;

        public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            Fen = fen;
            Board = new Board(fen);
            Moves = new Moves(Board);
            FigureMovings = new List<FigureMoving>();
        }

        private Chess(Board board)
        {
            Board = board;
            Fen = board.Fen;
            Moves = new Moves(Board);
            FigureMovings = new List<FigureMoving>();
            
            FindAllMoves();
        }

        public Chess Move(string move)
        {            
            FigureMoving fm = new FigureMoving(move);
            
            // Check on end the game.
            if (IsNoMoves())
            {
                return this;
            }     
                
            // Check on valid. If no - return Chess without changes 
            if (!Moves.CanMove(fm) || Board.IsCheckAfterMove(fm))
                return this;
            
            Board nextBoard = Board.Move(fm);
            Chess nextChess = new Chess(nextBoard);
            return nextChess;
        }

        public char GetFigureAt(int x, int y)
        {
            Square square = new Square(x, y);
            Figure figure = Board.GetFigureAt(square);
            return figure == Figure.None ? '.' : (char)figure;
        }

        private void FindAllMoves()
        {
            foreach (FigureOnSquare fs in Board.YieldFigures())
            {
                foreach (Square to in Square.YieldSquares())
                {
                    FigureMoving fm = new FigureMoving(fs, to);
                    if (Moves.CanMove(fm) && !Board.IsCheckAfterMove(fm))
                    {
                        FigureMovings.Add(fm);
                    }
                }
            }
        }

        public List<string> GetAllMoves()
        {
            FindAllMoves();
            List<string> moves = new List<string>();
            foreach (FigureMoving fm in FigureMovings)
            {
                moves.Add(fm.ToString());
            }

            return moves;
        }

        public bool IsCheck()
        {
            return Board.IsCheckNow();
        }


        public bool IsMate()
        {
            return IsNoMoves() && IsCheck();
        }

        public bool IsStalemate()
        {
            return IsNoMoves() && !IsCheck();
        }
    }
}