using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    public enum Figure
    {
        None,
        
        WhiteKing = 'K',
        WhiteQueen = 'Q',
        WhiteRook = 'R',
        WhiteBishop = 'B',
        WhiteKnight = 'N',
        WhitePawn = 'P',
        
        BlackKing = 'k',
        BlackQueen = 'q',
        BlackRook = 'r',
        BlackBishop = 'b',
        BlackKnight = 'n',
        BlackPawn = 'p'
    }

    public static class FigureMethods
    {
        public static Color GetColor(this Figure figure)
        {
            if (figure == Figure.None)
                return Color.None;
            return char.IsUpper((char)figure) ? Color.White : Color.Black;
        }
    }

    public static class ConsoleMethods
    {
        public static char ToUnicode(char figure)
        {
            return figure switch
            {
                'K' => '\u2654',
                'Q' => '\u2655',
                'R' => '\u2656',
                'B' => '\u2657',
                'N' => '\u2658',
                'P' => '\u2659',
                'k' => '\u265A',
                'q' => '\u265B',
                'r' => '\u265C',
                'b' => '\u265D',
                'n' => '\u265E',
                'p' => '\u265F',
                _ => '\u25A3'
            };
        } 
    }
}
