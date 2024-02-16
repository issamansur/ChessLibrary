using ChessMaster.Boards;

namespace ChessMaster.Tests.FiguresTests;

public class KingTests
{
    // 
    private Chess _chess;

    [SetUp]
    public void Setup()
    {
        _chess = new Chess("rn2k1nr/8/8/8/8/8/8/RN2K1NR w KQkq - 0 1");
        /*
         * Initial position
         *
         * 8    r  n  .  .  k  .  n  r
         * 7    .  .  .  .  .  .  .  .
         * 6    .  .  .  .  .  .  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    .  .  .  .  .  .  .  .
         * 1    R  N  .  .  K  .  N  R
         *
         *      a  b  c  d  e  f  g  h
         */
    }
    
    // SimpleMove
    [Test]
    public void SimpleMoveSuccess()
    {
        /*
         * Map of moves
         *
         * 8    r  n  .  .  4  3  n  r
         * 7    .  .  .  .  1  2  .  .
         * 6    .  .  .  .  .  .  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    .  .  .  .  1  2  .  .
         * 1    R  N  .  .  4  3  N  R
         *
         *      a  b  c  d  e  f  g  h
         */
        _chess.Move("Ke1e2");
        _chess.Move("ke8e7");
        _chess.Move("Ke2f2");
        _chess.Move("ke7f7");
        _chess.Move("Kf2f1");
        _chess.Move("kf7f8");
        _chess.Move("Kf1e1");
        _chess.Move("kf8e8");
        
        /*
         * Map of moves
         *
         * 8    r  n  .  .  4  .  n  r
         * 7    .  .  .  1  .  3  .  .
         * 6    .  .  .  .  2  .  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  2  .  .  .
         * 2    .  .  .  1  .  3  .  .
         * 1    R  N  .  .  4  .  N  R
         *
         *     a  b  c  d  e  f  g  h
         */
        
        _chess.Move("Ke1d2");
        _chess.Move("ke8d7");
        _chess.Move("Kd2e3");
        _chess.Move("kd7e6");
        _chess.Move("Ke3f2");
        _chess.Move("ke6f7");
        _chess.Move("Kf2e1");
        _chess.Move("kf7e8");
        
        Assert.Pass();
    }

    [Test]
    [TestCase("Ke4c2")]
    [TestCase("Ke4c3")]
    [TestCase("Ke4c4")]
    [TestCase("Ke4c5")]
    [TestCase("Ke4c6")]
    [TestCase("Ke4d6")]
    [TestCase("Ke4e6")]
    [TestCase("Ke4f6")]
    [TestCase("Ke4g6")]
    [TestCase("Ke4g5")]
    [TestCase("Ke4g4")]
    [TestCase("Ke4g3")]
    [TestCase("Ke4g2")]
    [TestCase("Ke4f2")]
    [TestCase("Ke4e2")]
    [TestCase("Ke4d2")]
    public void SimpleMoveFault_BadDirection(string move)
    {
        _chess.Move("Ke1e2");
        _chess.Move("ke8e7");
        _chess.Move("Ke2e3");
        _chess.Move("ke7d8");
        _chess.Move("Ke3e4");
        _chess.Move("kd8e8");
        /*
         * Position after 4 moves and Map of moves
         *
         * 8    r  n  .  .  k  .  n  r
         * 7    .  .  .  .  .  .  .  .
         * 6    .  .  5  6  7  8  9  .
         * 5    .  .  4  .  .  . 10  .
         * 4    .  .  3  .  K  . 11  .
         * 3    .  .  2  .  .  . 12  .
         * 2    .  .  1 16 15 14 13  .
         * 1    R  N  .  .  .  .  N  R
         *
         *      a  b  c  d  e  f  g  h
         */
        Assert.Throws<ArgumentException>(() => _chess.Move(move));
    }
    
    // Castle
    [Test]
    [TestCase("Ke1c1", "ke8c8")]
    [TestCase("Ke1g1", "ke8g8")]
    public void CastleSuccess(string moveWhite, string moveBlack)
    {
        _chess.Move("Nb1d2");
        _chess.Move("nb8d7");
        _chess.Move("Ng1f3");
        _chess.Move("ng8f6");
        /*
         * Position after 4 moves and Map of moves
         *
         * 8    r  .  1  .  k  .  2  r
         * 7    .  .  .  n  .  .  .  .
         * 6    .  .  .  .  .  n  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  .  N  .  .
         * 2    .  .  .  N  .  .  .  .
         * 1    R  .  1  .  K  .  2  R
         *
         *      a  b  c  d  e  f  g  h
         */
        _chess.Move(moveWhite);
        _chess.Move(moveBlack);
        
        Assert.Pass();
    }
    
    [Test]
    public void CastleSuccess_Half()
    {
        _chess.Move("Nb1d2");
        _chess.Move("nb8d7");
        _chess.Move("Ng1f3");
        _chess.Move("ng8f6");
        
        _chess.Move("Ra1a2");
        _chess.Move("rh8h7");
        /*
         * Position after 6 moves and Map of moves
         *
         * 8    r  .  1  .  k  .  .  .
         * 7    .  .  .  n  .  .  .  r
         * 6    .  .  .  .  .  n  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  .  N  .  .
         * 2    R  .  .  N  .  .  .  .
         * 1    .  .  .  .  K  .  1  R
         *
         *      a  b  c  d  e  f  g  h
         */
        _chess.Move("Ke1g1");
        _chess.Move("ke8c8");
        
        Assert.Pass();
    }

    [Test]
    [TestCase("rn2k1nq/8/8/8/8/8/8/RN2K1NQ w KQkq - 0 1")]
    [TestCase("rn2k1nB/8/8/8/8/8/8/RN2K1Nb w KQkq - 0 1")]
    [TestCase("rn2k1n1/8/8/8/8/8/8/RN2K1N1 w KQkq - 0 1")]
    public void CastleFault_NoRook(string chessFen)
    {
        _chess = new Chess(chessFen);
        /*
         * Position after 4 moves and Map of moves
         *
         * 8    ?  .  1  .  k  .  2  ?
         * 7    .  .  .  n  .  .  .  .
         * 6    .  .  .  .  .  n  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  .  N  .  .
         * 2    .  .  .  N  .  .  .  .
         * 1    ?  .  1  .  K  .  2  ?
         *
         *      a  b  c  d  e  f  g  h
         */
        Assert.Throws<ArgumentException>(() => _chess.Move("Ke1c1"));
        Assert.Throws<ArgumentException>(() => _chess.Move("Ke1g1"));
        
        _chess.Move("Ke1e2");
        
        Assert.Throws<ArgumentException>(() => _chess.Move("ke8c8"));
        Assert.Throws<ArgumentException>(() => _chess.Move("ke8g8"));
    }

    [Test]
    public void CastleFault_KingAlreadyMove()
    {
        _chess.Move("Nb1d2");
        _chess.Move("nb8d7");
        _chess.Move("Ng1f3");
        _chess.Move("ng8f6");

        _chess.Move("Ke1e2");
        _chess.Move("ke8e7");
        _chess.Move("Ke2e1");
        _chess.Move("ke7e8");
        /*
         * Position after 8 moves
         *
         * 8    r  .  .  .  k  .  .  r
         * 7    .  .  .  n  .  .  .  .
         * 6    .  .  .  .  .  n  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  .  N  .  .
         * 2    .  .  .  N  .  .  .  .
         * 1    R  .  .  .  K  .  . R
         *
         *      a  b  c  d  e  f  g  h
         */
        Assert.Throws<ArgumentException>(() => _chess.Move("Ke1c1"));
        Assert.Throws<ArgumentException>(() => _chess.Move("Ke1g1"));

        _chess.Move("Ke1e2");

        Assert.Throws<ArgumentException>(() => _chess.Move("ke8c8"));
        Assert.Throws<ArgumentException>(() => _chess.Move("ke8g8"));
    }

    [Test]
    public void CastleFault_RookAlreadyMove()
    {
        _chess.Move("Nb1d2");
        _chess.Move("nb8d7");
        _chess.Move("Ng1f3");
        _chess.Move("ng8f6");
        
        _chess.Move("Ke1e2");
        _chess.Move("ke8e7");
        _chess.Move("Ke2e1");
        _chess.Move("ke7e8");
        
        _chess.Move("Ra1a2");
        _chess.Move("rh8h7");
        /*
         * Position after 4 moves and Map of moves
         *
         * 8    r  .  1  .  k  .  2  r
         * 7    .  .  .  n  .  .  .  .
         * 6    .  .  .  .  .  n  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  .  N  .  .
         * 2    .  .  .  N  .  .  .  .
         * 1    R  .  1  .  K  .  2  R
         *
         *      a  b  c  d  e  f  g  h
         */
        Assert.Throws<ArgumentException>(() => _chess.Move("Ke1c1"));
        
        _chess.Move("Ke1g1");
        
        Assert.Throws<ArgumentException>(() => _chess.Move("ke8g8"));

        _chess.Move("ke8c8");
        
        Assert.Pass();
    }

    [Test]
    public void CastleFault_FiguresBetween()
    {
        // TODO: Add test
    }

    [Test]
    public void CastleFault_KingInCheck()
    {
        // TODO: Add test
    }
}