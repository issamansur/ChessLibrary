namespace ChessMaster.Domain.States;

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
    
    public static Color FromChar(char c)
    {
        return c switch
        {
            'w' => Color.White,
            'b' => Color.Black,
            _ => throw new ArgumentException("Invalid color"),
        };
    }
    
    public static string ToStr(this Color color)
    {
        return color switch
        {
            Color.White => "w",
            Color.Black => "b",
            _ => throw new InvalidOperationException("Invalid color"),
        };
    }
}
