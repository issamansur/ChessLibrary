using System.Text;
using ChessMaster.Domain.Figures;
using ChessMaster.Domain.States;
using ChessMaster.Domain.Utils;

namespace ChessMaster.Domain.Boards;

public class Board
{
    // Properties
    internal Figure?[,] Figures { get; init; } = new Figure?[8, 8];

    // Castling state of the current board
    internal Castling Castling { get; init; }

    // Field for en passant capture
    internal Field? EnPassantTargetSquare { get; private set; }

    // Indexers to access figures on the board
    public Figure? this[int x, int y] => Figures[x, y];
    public Figure? this[Field field] => Figures[field.X, field.Y];

    // Constructor to initialize the board
    public Board()
    {
        SetDefaultFigures();

        Castling = new Castling();
        EnPassantTargetSquare = null;
    }
    
    public Board(Figure?[,] figures, Castling castling, Field? enPassantTargetSquare)
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Figures[x, y] = figures[x, y];
            }
        }

        Castling = new Castling(
            castling.CanCastleE1C1,
            castling.CanCastleE1G1,
            castling.CanCastleE8C8,
            castling.CanCastleE8G8
            );

        EnPassantTargetSquare = enPassantTargetSquare is not null ? 
            new Field(enPassantTargetSquare.X, enPassantTargetSquare.Y) : null;
    }
    
    // Method to set figures on the board by default 
    private void SetDefaultFigures()
    {
        string boardFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        
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
                    Figures[x, 7 - y] = Parsers.CharToFigure(c);
                    x++;
                }
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
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (this[x, y] is King king && king.Color == kingColor)
                {
                    var kingField = new Field(x, y);
                    return kingField;
                }
            }
        }

        throw new InvalidOperationException();
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
        Figures[field.X, field.Y] = figure;
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
        Board board = new Board(Figures, Castling, EnPassantTargetSquare);

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

        if (move is { Figure: Pawn, To.Y: 0 or 7 })
        {
            board.SetFigure(move.To, move.PromotedFigure);
        }

        // Update properties for FEN (and create new FEN)
        board.UpdateState(move);

        // Return new Board with new FEN
        return board;
    }

    // Method to check if a figure can make a certain move
    public bool CanMove(Move move)
    {
        Figure? figure = this[move.From];
        // Validate the following conditions:
        // 1. There is a figure on the current field.
        // 2. The figure on the current field is of the active color.
        // 3. The figure on the target field is not of the active color (or moving Figure Color).
        // 4. The move is not to the same field where the figure currently is.
        // 5. If the figure is not a Pawn, it cannot have a captured figure in the move.
        // 6. The figure should be able to walk like that.
        // 7. There should be no check after the move
        if (move.Figure != figure ||
            move.From == move.To ||
            //move.Figure.Color != ActiveColor ||
            this[move.To]?.Color == figure.Color ||
            move.PromotedFigure is not null && figure is not Pawn ||
            !figure.CanMove(this, move))
        {
            return false;
        }

        return !MoveFigure(move).IsCheck(move.Figure.Color);
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

        // Update Castling
        Castling.Update(move);

        // Update EnPassantTargetSquare
        EnPassantTargetSquare = null;
        if (move is { Figure: Pawn, AbsDiffY: 2 })
        {
            EnPassantTargetSquare = move.From + move.Direction;
        }
    }
}
