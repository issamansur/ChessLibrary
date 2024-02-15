namespace ChessMaster.Utils;

using Boards;

public class BoardBuilder
{
    public Board Build(string fen)
    {
        // Разбиваем FEN на части
        var parts = fen.Split();
        if (parts.Length != 6)
        {
            throw new ArgumentException("Invalid fen: must contain 6 data parts");
        }

        // Создаем новый экземпляр доски
        var board = new Board();

        // Заполняем поля доски на основе FEN

        return board;
    }
}