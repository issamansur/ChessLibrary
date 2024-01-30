namespace ChessMaster;

public class Chess
{
    private Board _board;
    public State State { get; private set; }
    public Color ActiveColor { get; private set; }
    private Castling _castling;
    public Field EnPassantTargetSquare { get; private set; }
    private int _halfmoveClock;
    public int FullmoveNumber { get; private set; }

    public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
    {
        State = State.None;
        FenInit(fen);
    }

    private void FenInit(string fen)
    {
        // Get fen parts and check
        var parts = fen.Split();
        if (parts.Length != 6)
        {
            throw new ArgumentException("Invalid fen: must contain 6 data parts");
        }

        // Set "Piece placement" part to "Board"
        _board = new Board(fen);

        // Set "Active color" part
        ActiveColor = parts[1] switch
        {
            "w" => Color.White,
            "b" => Color.Black,
            _ => throw new ArgumentException("Invalid fen (Active Color): invalid color"),
        };

        // Set "Castling Availability" part
        _castling = new Castling(parts[2]);

        // Set "En passant target square" part
        EnPassantTargetSquare = parts[3] switch
        {
            "-" => Field.None,
            _ => Field.FromString(parts[3]),
        };

        // Set "Halfmove clock" part
        _halfmoveClock = int.Parse(parts[4]);

        // Set "Fullmove number" part
        FullmoveNumber = int.Parse(parts[5]);
    }

    public void Move(string move)
    {
        // TODO
    }
}