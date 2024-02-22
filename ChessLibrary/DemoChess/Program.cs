using System.Text;
using ChessMaster.Domain.States;
using ChessMaster.Domain.Utils;
using Chess = ChessMaster.Domain.Chess;
using Figure = ChessMaster.Domain.Figures.Figure;

Console.OutputEncoding = Encoding.UTF8;

Chess chess = new Chess();

while (true)
{
    Console.Clear();
    ChessToAscii(chess);

    switch (chess.GameState)
    {
        case GameState.Check:
            Console.WriteLine("Check");
            break;
        case GameState.Checkmate:
            Console.WriteLine("Checkmate");
            break;
        case GameState.Stalemate:
            Console.WriteLine("Stalemate");
            break;
    }
    
    var move = Console.ReadLine()!;
    if (move == "")
        break;
    chess.Move(move);
}
static void ChessToAscii(Chess chess)
{
    string verticalBorder = "  +-----------------+";
    Console.ForegroundColor = ConsoleColor.Cyan;

    Console.WriteLine(verticalBorder);
    for (int y = 7; y >= 0; y--)
    {
        Console.Write(y + 1);
        Console.Write(" | ");
        for (int x = 0; x < 8; x++)
        {
            Figure? figure = chess[x, y];
            if (figure == null)
                Console.ForegroundColor = ConsoleColor.Gray;
            else 
                Console.ForegroundColor = ConsoleColor.White;
            Console.Write(figure == null? "." : StringParser.FigureToUnicode(figure));
            Console.Write(' ');
        }
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("|");
    }
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(verticalBorder);
    Console.WriteLine("    a b c d e f g h");
    Console.ResetColor();
    Console.WriteLine();
}