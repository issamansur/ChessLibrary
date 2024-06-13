using ChessMaster.ChessLibrary;
using ChessMaster.ChessLibrary.Utils;

namespace ChessMaster.Tests.FiguresTests;

public class BishopTests
{
    // 
    private Chess _chess;

    [SetUp]
    public void Setup()
    {
        _chess = Builders.ChessBuild("k7/8/1r3r2/8/3B4/8/1r3r2/7K w KQkq - 0 1");
        /*
         * Initial position
         *
         * 8    k  .  .  .  .  .  .  .
         * 7    .  .  .  .  .  .  .  .
         * 6    .  r  .  .  .  r  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  B  .  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    .  r  .  .  .  r  .  .
         * 1    .  .  .  .  .  .  .  K
         *
         *      a  b  c  d  e  f  g  h
         */
    }
    
    // Move/Capture
    [Test]
    [TestCase("Bd4c3")]
    [TestCase("Bd4b2")]
    [TestCase("Bd4e3")]
    [TestCase("Bd4f2")]
    [TestCase("Bd4c5")]
    [TestCase("Bd4b6")]
    [TestCase("Bd4e5")]
    [TestCase("Bd4f6")]
    public void MoveOrCaptureSuccess(string moveWhite)
    {
        _chess.Move(moveWhite);
        
        Assert.Pass();
    }

    [Test]
    [TestCase("Bd4d3")]
    [TestCase("Bd4d2")]
    [TestCase("Bd4c4")]
    [TestCase("Bd4b4")]
    [TestCase("Bd4e4")]
    [TestCase("Bd4f4")]
    [TestCase("Bd4d5")]
    [TestCase("Bd4d6")]
    
    [TestCase("Bd4a1")]
    [TestCase("Bd4a7")]
    [TestCase("Bd4g1")]
    [TestCase("Bd4g7")]
    public void MoveOrCaptureFault(string moveWhite)
    {
        Assert.Throws(
            typeof(ArgumentException), 
            () => _chess.Move(moveWhite)
        );
    }
}