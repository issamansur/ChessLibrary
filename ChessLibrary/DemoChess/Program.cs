using ChessLibrary;

Chess chess = new Chess();
while (true)
{
    Console.WriteLine(chess.Fen);
    string move = Console.ReadLine();
    if (move == "")
        break;
    chess = chess.Move(move);
}