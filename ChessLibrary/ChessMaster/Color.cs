namespace ChessMaster;

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
    
    // Is this a good idea?
    public static string ToString(this Color color)
    {
        return color switch
        {
            Color.White => "w",
            Color.Black => "b",
            _ => throw new InvalidOperationException("Invalid color"),
        };
    }
}
