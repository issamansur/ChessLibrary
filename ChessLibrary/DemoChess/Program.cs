using ChessLibrary;
using ChessMaster;
using System.Text;
using Chess = ChessMaster.Chess;
using Figure = ChessMaster.Figures.Figure;

Console.OutputEncoding = Encoding.UTF8;

Chess chess = new Chess();

while (true)
{
    Console.Clear();
    ChessToAscii(chess);
    
    //Console.WriteLine(chess.Fen);
    /*
    foreach (string avMove in chess.GetAllMoves())
    {
        Console.Write($"{avMove} ");
    }
    Console.WriteLine();
    */
    
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
            else if (figure.Color == ChessMaster.States.Color.White)
                Console.ForegroundColor = ConsoleColor.White;
            else 
                Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(figure?.ToUnicode() ?? '.');
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
/*
Chess chess = new ChessLibrary.Chess();
while (true)
{
    Console.Clear();
    Console.OutputEncoding = Encoding.UTF8;
    ChessToAscii(chess);
    if (chess.IsStalemate())
    {
        Console.WriteLine("Stalemate");
    }
    else if (chess.IsMate())
    {
        Console.WriteLine("Mate");
    }
    else if (chess.IsCheck())
    {
        Console.WriteLine("Check");
    }

    Console.WriteLine(chess.Fen);
    foreach (string avMove in chess.GetAllMoves())
    {
        Console.Write($"{avMove} ");
    }
    Console.WriteLine();
    
    var move = Console.ReadLine();
    if (move == "")
        break;
    chess = chess.Move(move!);
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
            char figure = chess.GetFigureAt(x, y);
            if (figure == '.')
                Console.ForegroundColor = ConsoleColor.Gray;
            else if (char.IsLower(figure))
                Console.ForegroundColor = ConsoleColor.Black;
            else 
                Console.ForegroundColor = ConsoleColor.White;
            Console.Write(ConsoleMethods.ToUnicode(figure));
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
*/