using System.Text;
using ChessMaster.Figures;
using ChessMaster.States;

namespace ChessMaster.Boards;

public class Board
{
    // FEN parts
    private string _fen;
    private readonly Figure?[,] _figures;
    public Color ActiveColor { get; private set; }
    public Castling Castling { get; private set; }
    public Field? EnPassantTargetSquare { get; private set; }
    private int _halfMoveClock;
    public int FullMoveNumber { get; private set; }
    
    // Indexers
    public Figure? this[int x, int y] => _figures[x, y];
    public Figure? this[Field field] => _figures[field.X, field.Y];
    
    // Constructors
    public Board(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
    {
        _fen = fen;
        _figures = new Figure?[8, 8];
        
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

    // Methods
    private static bool IsFieldOnBoard(Field field)
    {
        return field is { X: >= 0, X: < 8, Y: >= 0, Y: < 8 };
    }

    private void SetFigure(Field field, Figure? figure)
    {
        _figures[field.X, field.Y] = figure;
    }
    
    public bool CanMoveFromTo(Move move)
    {
        Field current = move.From;
        
        do
        {
            current += move.Direction;
            if (current == move.To)
            {
                return true;
            }
        } 
        while (IsFieldOnBoard(current) && this[current] == null);

        return false;
    }
    
    public bool CanMove(Move move)
    {
        // Check on:
        // 1. On this field is figure +
        // 2. This figure is active color +
        // 3. On this field is figure of active color +
        // 4. Move is not to the same field +
        // 5. Move cannot have CapturedFigure if not a Pawn +
        if (this[move.From] != move.Figure || 
            move.Figure.Color != ActiveColor || 
            (this[move.To] != null && this[move.To]?.Color == ActiveColor) ||
            move.From == move.To ||
            move.CapturedFigure is not null && move.Figure is not Pawn)
        {
            return false;
        }

        return move.Figure.CanMove(this, move);
    }
    
    public Board Move(Move move)
    {
        // Check to avaibility move
        if (!CanMove(move))
        {
            throw new ArgumentException("Invalid move");
        }

        // Default move
        SetFigure(move.From, null);
        SetFigure(move.To, move.Figure);

        // Add effects in this situation:
        // 1. Castling (Move Rock)
        // 2. Taking at en Passant (Remove enemy Pawn)
        // 3. Capturing (change Pawn to Figure)

        if (move is { Figure: King, AbsDiffX: 2 })
        {
            Field rockPosition = Castling.GetRockPosition(move.To);
            Figure rook = this[rockPosition]!;

            SetFigure(rockPosition, null);
            SetFigure(move.From + move.Direction, rook);
        }

        if (move.Figure is Pawn && move is { AbsDiffX: 1, AbsDiffY: 1 })
        {
            if (EnPassantTargetSquare is not null)
            {
                Field field = EnPassantTargetSquare ?? throw new InvalidOperationException();
                SetFigure(field, null);
            }
        }

        if (move.Figure is Pawn && move.To is { Y: 0 or 7 })
        {
            SetFigure(move.To, move.CapturedFigure);
        }


        // Update properties for FEN (and create new FEN)
        UpdateState(move);

        // Return new Board with new FEN
        return new Board(_fen);
    }
    
    private void UpdateState(Move move)
    {
        // Update ActiveColor
        ActiveColor = ActiveColor.ChangeColor();
        
        // Update Castling
        Castling.Update(move);
        
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