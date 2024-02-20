namespace ChessMaster;

using Figures;
using Boards;
using States;

public class Chess
{
    // Fields and properties
    public Figure? this[int x, int y] => _board[x, y];
    private Board _board;
    
    public GameState GameState { get; private set; }
    
    // Additional parts of chess game
    /*
    public List<Move> Moves { get; private set; }
    public List<string> History { get; private set; }
    */
    
    // Constructor
    public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
    {
        GameState = GameState.None;
        _board = new Board(fen);
    }

    // Methods
    public void Start()
    {
        GameState = GameState.Playing;
    }
    
    public void Move(string stringMove)
    {
        if (GameState is GameState.Checkmate or GameState.Stalemate)
        {
            return;
        }
        
        var move = Boards.Move.FromString(stringMove);
        _board = _board.Move(move);
        if (_board.IsCheckmate(_board.ActiveColor))
        {
            GameState = GameState.Checkmate;
        }
        else if (_board.IsStalemate(_board.ActiveColor))
        {
            GameState = GameState.Stalemate;
        }
        else
        if (_board.IsCheck(_board.ActiveColor))
        {
            GameState = GameState.Check;
        }
    }
}