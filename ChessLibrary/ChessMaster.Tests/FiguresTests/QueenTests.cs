using ChessMaster.Domain;
using ChessMaster.Domain.Utils;

namespace ChessMaster.Tests.FiguresTests;

public class QueenTests
{
    // 
    private Chess _chess;

    [SetUp]
    public void Setup()
    {
        _chess = Builders.ChessBuild("k7/8/2r2r2/8/8/2Q2rr1/8/7K w KQkq - 0 1");
        /*
         * Initial position
         *
         * 8    k  .  .  .  .  .  .  .
         * 7    .  .  .  .  .  .  .  .
         * 6    .  .  r  .  .  r  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  Q  .  .  r  r  .
         * 2    .  .  .  .  .  .  .  .
         * 1    .  .  .  .  .  .  .  K
         *
         *      a  b  c  d  e  f  g  h
         */
    }
    
    // Move/Capture
    [Test]
    [TestCase("Qc3c4")]
    [TestCase("Qc3c5")]
    [TestCase("Qc3c6")]
    [TestCase("Qc3d4")]
    [TestCase("Qc3e5")]
    [TestCase("Qc3f6")]
    [TestCase("Qc3d3")]
    [TestCase("Qc3e3")]
    [TestCase("Qc3f3")]
    [TestCase("Qc3d2")]
    [TestCase("Qc3e1")]
    [TestCase("Qc3c2")]
    [TestCase("Qc3c1")]
    [TestCase("Qc3b2")]
    [TestCase("Qc3a1")]
    [TestCase("Qc3b3")]
    [TestCase("Qc3a3")]
    [TestCase("Qc3b4")]
    [TestCase("Qc3a5")]
    public void MoveOrCaptureSuccess(string moveWhite)
    {
        _chess.Move(moveWhite);
        
        Assert.Pass();
    }

    [Test]
    [TestCase("Qc3c7")]
    [TestCase("Qc3g7")]
    [TestCase("Qc3g3")]
    [TestCase("Qc3h3")]
    [TestCase("Qc3b1")]
    [TestCase("Qc3a2")]
    public void MoveOrCaptureFault(string moveWhite)
    {
        Assert.Throws(
            typeof(ArgumentException), 
            () => _chess.Move(moveWhite)
        );
    }
}