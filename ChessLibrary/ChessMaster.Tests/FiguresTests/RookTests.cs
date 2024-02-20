using ChessMaster.Domain;
using ChessMaster.Domain.Utils;

namespace ChessMaster.Tests.FiguresTests;

public class RookTests
{
    // 
    private Chess _chess;

    [SetUp]
    public void Setup()
    {
        _chess = Builders.ChessBuild("k7/8/2r5/8/8/2R2rr1/8/7K w KQkq - 0 1");
        /*
         * Initial position
         *
         * 8    k  .  .  .  .  .  .  .
         * 7    .  .  .  .  .  .  .  .
         * 6    .  .  r  .  .  .  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  R  .  .  r  r  .
         * 2    .  .  .  .  .  .  .  .
         * 1    .  .  .  .  .  .  .  K
         *
         *      a  b  c  d  e  f  g  h
         */
    }
    
    // Move/Capture
    [Test]
    [TestCase("Rc3c4")]
    [TestCase("Rc3c5")]
    [TestCase("Rc3c6")]
    [TestCase("Rc3d3")]
    [TestCase("Rc3e3")]
    [TestCase("Rc3f3")]
    [TestCase("Rc3c2")]
    [TestCase("Rc3c1")]
    [TestCase("Rc3b3")]
    [TestCase("Rc3a3")]
    public void MoveOrCaptureSuccess(string moveWhite)
    {
        _chess.Move(moveWhite);
        
        Assert.Pass();
    }

    [Test]
    [TestCase("Rc3c7")]
    [TestCase("Rc3g7")]
    [TestCase("Rc3g3")]
    [TestCase("Rc3h3")]
    [TestCase("Rc3b1")]
    [TestCase("Rc3a2")]
    public void MoveOrCaptureFault(string moveWhite)
    {
        Assert.Throws(
            typeof(ArgumentException), 
            () => _chess.Move(moveWhite)
        );
    }
}