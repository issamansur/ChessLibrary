namespace ChessMaster.Figures;

public abstract class Figure
{
    public Color Color { get; private init; }
    public Field Field { get; private set; }
    
    public Figure(Color color, Field field)
    {
        Color = color;
        Field = field;
    }
    
    public abstract bool CanMove(Field field);
    
    public abstract void Move(Field field); 
}