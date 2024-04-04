using System.Text.RegularExpressions;
using ChessMaster.ChessModels.Figures;

namespace ChessMaster.ChessModels.Boards;


public class Field
{
    // Fields and Properties
    public readonly int X;
    public readonly int Y;
    
    // Constructors
    public Field(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public Field Clone()
    {
        return new Field(X, Y);
    }
    
    // Equals (we no need to compare Figure)
    private bool Equals(Field other)
    {
        return X == other.X && Y == other.Y;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is Field other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
    
    // operators + and -
    public static Field operator +(Field field1, Field field2)
    {
        return new Field(field1.X + field2.X, field1.Y + field2.Y);
    }
    
    public static Field operator -(Field field1, Field field2)
    {
        return new Field(field1.X - field2.X, field1.Y - field2.Y);
    }
    
    // operators == and !=
    public static bool operator ==(Field? a, Field? b)
    {
        return a?.X == b?.X && a?.Y == b?.Y;
    }
    
    public static bool operator !=(Field a, Field b)
    {
        return !(a == b);
    }
}