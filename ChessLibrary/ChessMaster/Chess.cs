using System.Runtime.InteropServices;
using ChessMaster.Figures;

namespace ChessMaster;

using ChessMaster.Boards;
using ChessMaster.States;

public class Chess
{
    // Fields and properties
    public Figure? this[int x, int y] => _board[x, y];
    private Board _board;
    
    
    // Additional parts of chess game
    /*
    public GameState GameState { get; private set; }
    public List<Move> Moves { get; private set; }
    public List<string> History { get; private set; }
    */
    
    // Constructor
    public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
    {
        //GameState = GameState.None;
        _board = new Board(fen);
    }

    // Methods
    public void Start()
    {
        //GameState = GameState.Playing;
    }
    
    public void Move(string stringMove)
    {
        var move = Boards.Move.FromString(stringMove);
        _board = _board.Move(move);
    }
}