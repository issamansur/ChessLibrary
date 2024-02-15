using System.Text.RegularExpressions;
using ChessMaster.Figures;

namespace ChessMaster.Boards;


public class Field
{
    // Fields and Properties
    private const string Alphabet = "abcdefgh";

    private static Regex FieldPattern => new Regex("^[a-h][1-8]$");

    public readonly int X;
    public readonly int Y;
    
    // Constructors
    public Field(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public static Field FromString(string field)
    {
        if (!FieldPattern.IsMatch(field.ToLower()))
        {
            throw new ArgumentException("Invalid field"); 
        }
        var x = Alphabet.IndexOf(field.ToLower()[0]);
        var y = int.Parse(field[1].ToString()) - 1;
        
        return new Field(x, y);
    }
    
    // Overrides
    
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
    
    // ToString
    public override string ToString()
    {
        return $"{Alphabet[X]}{Y + 1}";
    }
}