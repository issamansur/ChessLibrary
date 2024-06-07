using ChessMaster.ChessModels;
using ChessMaster.ChessModels.Utils;

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
    
    public static async Task SafeMove(this Chess chess, string move)
    {
        try
        {
            chess.Move(move);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}