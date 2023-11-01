using ChessLibrary;
using System.Text;

Chess chess = new Chess();
while (true)
{
    Console.WriteLine(chess.Fen);
    Console.WriteLine(ChessToAscii(chess));
    string move = Console.ReadLine();
    if (move == "")
        break;
    chess = chess.Move(move);
}

static string ChessToAscii(Chess chess)
{
    string verticalBorder = "  +-----------------+";

    StringBuilder sb = new StringBuilder();
    sb.AppendLine(verticalBorder);
    for (int y = 7; y >= 0; y--)
    {
        sb.Append(y + 1);
        sb.Append(" | ");
        for (int x = 0; x < 8; x++)
        {
            sb.Append(chess.GetFigureAt(x, y));
            sb.Append(' ');
        }
        sb.AppendLine("|");
    }
    sb.AppendLine(verticalBorder);
    sb.AppendLine("    a b c d e f g h");
    return sb.ToString();
}