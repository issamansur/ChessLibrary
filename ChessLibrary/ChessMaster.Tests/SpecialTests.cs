using ChessMaster.Domain;
using ChessMaster.Domain.Utils;

namespace ChessMaster.Tests;

public class SpecialTests
{
    private Chess _chess;

    [SetUp]
    public void Setup()
    {
    }
    
    // We move in check and give check
    [Test]
    public void InvalidMoveWithCheckToUsAndEnemy()
    {
        _chess = Builders.ChessBuild("k2b4/8/1P6/K7/8/8/8/8 w KQkq - 0 1");
        /*
         * Initial position
         *
         * 8    k  .  .  b  .  .  .  .
         * 7    .  .  .  .  .  .  .  .
         * 6    .  P  .  .  .  .  .  .
         * 5    K  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    .  .  .  .  .  .  .  .
         * 1    .  .  .  .  .  .  .  .
         *
         *      a  b  c  d  e  f  g  h
         */
        
        Assert.Throws(
            typeof(ArgumentException), 
            () => _chess.Move("Pb6b7")
        );
    }
}