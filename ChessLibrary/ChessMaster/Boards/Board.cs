using System.Text;
using ChessMaster.Figures;
using ChessMaster.States;

namespace ChessMaster.Boards;

public class Board
{
    // FEN parts
    private string _fen;
    private readonly Figure?[,] _figures;

    // Active color of the current player
    public Color ActiveColor { get; set; }

    // Castling state of the current board
    public Castling Castling { get; private set; }

    // Field for en passant capture
    public Field? EnPassantTargetSquare { get; private set; }

    // Number of half moves since the last pawn advance or capture
    private int _halfMoveClock;

    // Number of the full move. It starts at 1, and is incremented after Black's move
    public int FullMoveNumber { get; private set; }

    // Indexers to access figures on the board
    public Figure? this[int x, int y] => _figures[x, y];
    public Figure? this[Field field] => _figures[field.X, field.Y];

    // Constructor to initialize the board with a FEN string
    public Board(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
    {
        _fen = fen;
        _figures = new Figure?[8, 8];

        // Initialize the board with the FEN string
        FenInit(fen);
    }

    // Method to initialize the board with a FEN string
    private void FenInit(string fen)
    {
        // Get fen parts and check
        var parts = fen.Split();
        if (parts.Length != 6)
        {
            throw new ArgumentException("Invalid fen: must contain 6 data parts");
        }

        // 1. Set "Piece placement" part to "Board"
        SetFigures(parts[0]);

        // 2. Set "Active color" part
        ActiveColor = parts[1] switch
        {
            "w" => Color.White,
            "b" => Color.Black,
            _ => throw new ArgumentException("Invalid fen (Active Color): invalid color"),
        };

        // 3. Set "Castling Availability" part
        Castling = new Castling(parts[2]);

        // 4. Set "En passant target square" part
        EnPassantTargetSquare = parts[3] switch
        {
            "-" => null,
            _ => Field.FromString(parts[3]),
        };

        // 5. Set "HalfMove clock" part
        _halfMoveClock = int.Parse(parts[4]);

        // 6. Set "FullMove number" part
        FullMoveNumber = int.Parse(parts[5]);
    }

    // Method to set figures on the board from a FEN string
    private void SetFigures(string boardFen)
    {
        var rows = boardFen.Split('/');
        if (rows.Length != 8)
        {
            throw new ArgumentException("Invalid fen (Piece Placement): must contain 8 rows");
        }

        for (var y = 0; y < 8; y++)
        {
            var row = rows[y];
            var x = 0;
            foreach (var c in row)
            {
                if (char.IsDigit(c))
                {
                    x += int.Parse(c.ToString());
                }
                else
                {
                    _figures[x, 7 - y] = Figure.FromChar(c);
                    x++;
                }
                if (x > 8)
                    throw new ArgumentException($"Invalid fen (Piece Placement): ...{row}...");
            }

            if (x != 8)
            {
                throw new ArgumentException($"Invalid fen (Piece Placement): ...{row}...");
            }
        }
    }

    // Method to check if a field is on the board
    private static bool IsFieldOnBoard(Field field)
    {
        return field is { X: >= 0, X: < 8, Y: >= 0, Y: < 8 };
    }

    public Field GetKingField(Color kingColor)
    {
        // Find the king's position
        Field? kingField = null;
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (this[x, y] is King king && king.Color == kingColor)
                {
                    kingField = new Field(x, y);
                    break;
                }
            }
        }

        return kingField ?? throw new InvalidOperationException();
    }

    // Method to get available moves for certain color
    public List<Move> GetAvailableMoves(Color sideColor)
    {
        List<Move> availableMoves = new List<Move>();

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Figure? ourFigure = this[x, y];
                // If it our figure, do cycle to all fields and try to move
                if (ourFigure?.Color == sideColor)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            Move triedMove = new Move(ourFigure!, new Field(x, y), new Field(i, j));
                            if (CanMove(triedMove))
                            {
                                availableMoves.Add(triedMove);
                            }
                        }
                    }
                }
            }
        }

        return availableMoves;
    }

    // Method to check if the king of a certain color is in check
    public bool IsCheck(Color kingColor)
    {
        Field kingField = GetKingField(kingColor);

        // Check if any of the opponent's pieces can move to the king's position
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Figure? figure = this[x, y];
                if (figure != null && figure.Color != kingColor)
                {
                    Move move = new Move(figure, new Field(x, y), kingField);
                    if (figure.CanMove(this, move))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    
    // Method to check if the king of a certain color is in checkmate
    public bool IsCheckmate(Color kingColor)
    {
        return IsCheck(kingColor) && GetAvailableMoves(kingColor).Count is 0;
    }
    
    // Method to check if the king of a certain color is in stalemate
    public bool IsStalemate(Color kingColor)
    {
        return !IsCheck(kingColor) && GetAvailableMoves(kingColor).Count is 0;
    }

    // Method to set a figure on a field
    private void SetFigure(Field field, Figure? figure)
    {
        _figures[field.X, field.Y] = figure;
    }

    // Method to check if a figure can move from one field to another in a certain direction
    public bool CanMoveFromTo(Field from, Field to, Field direction)
    {
        Field current = from;

        do
        {
            current += direction;
            if (current == to)
            {
                return true;
            }
        }
        while (IsFieldOnBoard(current) && this[current] == null);

        return false;
    }

    // Method to check if a figure can move from one field to another
    public bool CanMoveFromTo(Move move)
    {
        return CanMoveFromTo(move.From, move.To, move.Direction);
    }

    // Method to make a hard move figure (for check a ... check?!)
    private Board MoveFigure(Move move)
    {
        // Create new Board from FEN by copy.
        Board board = new Board(_fen);

        // Default move
        board.SetFigure(move.From, null);
        board.SetFigure(move.To, move.Figure);

        // Implement effects in the following scenarios:
        // 1. Castling (Move Rook)
        // 2. En Passant capture (Remove opponent's Pawn)
        // 3. Promotion (Replace Pawn with another piece when it reaches the end of the board)

        if (move is { Figure: King, AbsDiffX: 2 })
        {
            Field rockPosition = Castling.GetRockPosition(move.To);
            Figure rook = this[rockPosition]!;

            board.SetFigure(rockPosition, null);
            board.SetFigure(move.From + move.Direction, rook);
        }

        if (move.Figure is Pawn && move is { AbsDiffX: 1, AbsDiffY: 1 })
        {
            if (EnPassantTargetSquare is not null)
            {
                Field field = EnPassantTargetSquare ?? throw new InvalidOperationException();
                board.SetFigure(field, null);
            }
        }

        if (move.Figure is Pawn && move.To is { Y: 0 or 7 })
        {
            board.SetFigure(move.To, move.CapturedFigure);
        }


        // Update properties for FEN (and create new FEN)
        board.UpdateState(move);

        // Return new Board with new FEN
        return board;
    }

    // Method to check if a figure can make a certain move
    public bool CanMove(Move move)
    {
        // Validate the following conditions:
        // 1. There is a figure on the current field.
        // 2. The figure on the current field is of the active color.
        // 3. The figure on the target field is not of the active color.
        // 4. The move is not to the same field where the figure currently is.
        // 5. If the figure is not a Pawn, it cannot have a captured figure in the move.
        // 6. The figure should be able to walk like that.
        // 7. There should be no check after the move
        if (this[move.From] != move.Figure ||
            move.Figure.Color != ActiveColor ||
            (this[move.To] != null && this[move.To]?.Color == ActiveColor) ||
            move.From == move.To ||
            move.CapturedFigure is not null && move.Figure is not Pawn ||
            !move.Figure.CanMove(this, move))
        {
            return false;
        }

        return !MoveFigure(move).IsCheck(ActiveColor);
    }

    // Method to make a move on the board
    public Board Move(Move move)
    {
        // Check if the move is valid
        if (!CanMove(move))
        {
            throw new ArgumentException("Invalid move");
        }

        // Return new Board with new FEN
        return MoveFigure(move);
    }

    // Method to update the state of the board after a move
    private void UpdateState(Move move)
    {
        // Update ActiveColor
        ActiveColor = ActiveColor.ChangeColor();

        // Update Castling
        Castling = Castling.Update(move);

        // Update EnPassantTargetSquare
        EnPassantTargetSquare = null;
        if (move is { Figure: Pawn, AbsDiffY: 2 })
        {
            EnPassantTargetSquare = move.From + move.Direction;
        }

        // Update HalfMoveClock
        _halfMoveClock = move.Figure is Pawn ? 0 : _halfMoveClock + 1;

        // Update FullMoveNumber
        if (ActiveColor == Color.White)
        {
            FullMoveNumber++;
        }

        // Update FEN
        _fen = ToFen();
    }

    // Method to convert the current state of the board to a FEN string
    private string ToFen()
    {
        var fen = new StringBuilder();

        // 1. "Piece placement" part
        for (var y = 0; y < 8; y++)
        {
            var empty = 0;
            for (var x = 0; x < 8; x++)
            {
                var figure = _figures[x, 7 - y];
                if (figure == null)
                {
                    empty++;
                }
                else
                {
                    if (empty > 0)
                    {
                        fen.Append(empty);
                        empty = 0;
                    }
                    fen.Append(figure);
                }
            }
            if (empty > 0)
            {
                fen.Append(empty);
            }
            if (y < 7)
            {
                fen.Append('/');
            }
        }
        fen.Append(' ');

        // 2. "Active color" part
        fen.Append(ActiveColor.ToStr());
        fen.Append(' ');

        // 3. "Castling Availability" part
        fen.Append(Castling);
        fen.Append(' ');

        // 4. "En passant target square" part
        fen.Append(EnPassantTargetSquare?.ToString() ?? "-");
        fen.Append(' ');

        // 5. "HalfMove clock" part
        fen.Append(_halfMoveClock);
        fen.Append(' ');

        // 6. "FullMove number" part
        fen.Append(FullMoveNumber);

        return fen.ToString();
    }

    // Overrides
    // TODO: override ToString
}
