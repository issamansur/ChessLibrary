using ChessMaster.Domain;

namespace ChessMaster.Tests.FiguresTests;

public class KnightTests
{
    // 
    private Chess _chess;

    [SetUp]
    public void Setup()
    {
        _chess = new Chess("k7/8/2p1p3/2pppp2/2pNp3/2pppp2/8/7K w KQkq - 0 1");
        /*
         * Initial position
         *
         * 8    k  .  .  .  .  .  .  .
         * 7    .  .  .  .  .  .  .  .
         * 6    .  .  p  .  p  .  .  .
         * 5    .  .  p  p  p  p  .  .
         * 4    .  .  p  N  p  .  .  .
         * 3    .  .  p  p  p  p  .  .
         * 2    .  .  .  .  .  .  .  .
         * 1    .  .  .  .  .  .  .  K
         *
         *      a  b  c  d  e  f  g  h
         */
    }
    
    // Move/Capture
    [Test]
    [TestCase("Nd4c6")]
    [TestCase("Nd4e6")]
    [TestCase("Nd4f5")]
    [TestCase("Nd4f3")]
    [TestCase("Nd4e2")]
    [TestCase("Nd4c2")]
    [TestCase("Nd4b3")]
    [TestCase("Nd4b5")]
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
    
    [TestCase("Bd4b2")]
    [TestCase("Bd4c3")]
    [TestCase("Bd4b6")]
    [TestCase("Bd4c5")]
    [TestCase("Bd4e5")]
    [TestCase("Bd4f6")]
    [TestCase("Bd4e3")]
    [TestCase("Bd4f2")]
    public void MoveOrCaptureFault(string moveWhite)
    {
        Assert.Throws(
            typeof(ArgumentException), 
            () => _chess.Move(moveWhite)
        );
    }
}