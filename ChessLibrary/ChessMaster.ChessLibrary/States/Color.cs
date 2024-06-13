namespace ChessMaster.ChessLibrary.States;

public enum Color
{
    // Is this a good idea?
    White = 'w',
    Black = 'b',
}

public static class Colors {
    // Change color from white to black and vice versa
    public static Color ChangeColor(this Color color)
    {
        return color switch
        {
            Color.White => Color.Black,
            Color.Black => Color.White,
            _ => throw new InvalidOperationException("Invalid color"),
        };
    }
}
