using System.Text;
using System.Text.RegularExpressions;

namespace ChessMaster.Domain.Utils;

using Boards;
using States;
using Figures;

public class Builders
{
    public static Board BoardBuild(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
    {
        // Get fen parts and check
        var parts = fen.Split();
        if (parts.Length != 6)
        {
            throw new ArgumentException("Invalid fen: must contain 6 data parts");
        }

        Figure?[,] figures = Parsers.StringToFigures(parts[0]);
        
        Castling castling = Parsers.StringToCastling(parts[2]);
        
        string? enPassantString = parts[3];
        Field? enPassant = enPassantString == "-"? null : Parsers.StringToField(parts[3]);
        
        return new Board(figures, castling, enPassant);
    }

    public static Chess ChessBuild(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
    {
        // Get fen parts and check
        var parts = fen.Split();
        if (parts.Length != 6)
        {
            throw new ArgumentException("Invalid fen: must contain 6 data parts");
        }
        
        // 1. Set "Piece placement" part to "Board"
        Figure?[,] figures = Parsers.StringToFigures(parts[0]);
        
        // 2. Set "Active color" part
        Color activeColor = Parsers.StringToColor(parts[1]);
        
        // 3. Set "Castling Availability" part
        Castling castling = Parsers.StringToCastling(parts[2]);

        // 4. Set "En passant target square" part
        Field enPassantTargetSquare = (parts[3] switch
        {
            "-" => null,
            _ => Parsers.StringToField(parts[3]),
        })!;
        
        // 5. Set "HalfMove clock" part
        int halfMoveClock = int.Parse(parts[4]);

        // 6. Set "FullMove number" part
        int fullMoveNumber = int.Parse(parts[5]);

        // Create Board 
        Board board = new Board(
            figures,
            castling,
            enPassantTargetSquare
        );

        return new Chess(
            board,
            activeColor,
            halfMoveClock,
            fullMoveNumber
        );
    }

    // Method to convert the current state of the board to a FEN string
    private static string ToFen(Chess chess)
    {
        Figure?[,] figures = chess.Board.Figures;
        Color activeColor = chess.ActiveColor;
        Castling castling = chess.Board.Castling;
        Field? enPassantTargetSquare = chess.Board.EnPassantTargetSquare;
        int halfMoveClock = chess.HalfMoveClock;
        int fullMoveNumber = chess.FullMoveNumber;
        
        var fen = new StringBuilder();

        // 1. "Piece placement" part
        fen.Append(Parsers.FiguresToString(figures));
        fen.Append(' ');

        // 2. "Active color" part
        fen.Append(Parsers.ColorToString(activeColor));
        fen.Append(' ');

        // 3. "Castling Availability" part
        fen.Append(Parsers.CastlingToString(castling));
        fen.Append(' ');

        // 4. "En passant target square" part
        fen.Append(enPassantTargetSquare == null ? "-": enPassantTargetSquare);
        fen.Append(' ');

        // 5. "HalfMove clock" part
        fen.Append(halfMoveClock);
        fen.Append(' ');

        // 6. "FullMove number" part
        fen.Append(fullMoveNumber);

        return fen.ToString();
    }
}