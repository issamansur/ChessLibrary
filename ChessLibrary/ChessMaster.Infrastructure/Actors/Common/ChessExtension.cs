using ChessMaster.ChessLibrary;
using ChessMaster.ChessLibrary.Utils;

namespace ChessMaster.Infrastructure.Actors.Common;

public static class ChessExtension
{
    public static string GetFen(this Chess chess)
    {
        return Builders.ToFen(chess);
    }
    
    public static Chess ToChess(this string fen)
    {
        return Builders.ChessBuild(fen);
    }
}