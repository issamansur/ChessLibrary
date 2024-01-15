using ChessLibrary;
using System.Text;

Chess chess = new Chess();
while (true)
{
    Console.Clear();
    Console.OutputEncoding = Encoding.UTF8;
    ChessToAscii(chess);
    Console.WriteLine(chess.Fen);
    var move = Console.ReadLine();
    if (move == "")
        break;
    chess = chess.Move(move!);
}

static char ToUnicode(char figure)
{
    switch (figure)
    {
        case 'K':
            return '\u2654';
        case 'Q':
            return '\u2655';
        case 'R':
            return '\u2656';
        case 'B':
            return '\u2657';
        case 'N':
            return '\u2658';
        case 'P':
            return '\u2659';
        case 'k':
            return '\u265A';
        case 'q':
            return '\u265B';
        case 'r':
            return '\u265C';
        case 'b':
            return '\u265D';
        case 'n':
            return '\u265E';
        case 'p':
            return '\u265F';
    }

    return '.';
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
                Console.ForegroundColor = ConsoleColor.Cyan;
            else if (char.IsLower(figure))
                Console.ForegroundColor = ConsoleColor.Black;
            else 
                Console.ForegroundColor = ConsoleColor.White;
            Console.Write(ToUnicode(figure));
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