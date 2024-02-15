namespace ChessMaster.Tests.FiguresTests;

public class PawnTests
{
    // 
    private Chess _chess;

    [SetUp]
    public void Setup()
    {
        _chess = new Chess();
    }
    
    // SimpleMove
    [Test]
    public void SimpleMoveSuccess()
    {
        for (char verMin = 'a', verMax = 'h'; verMin <= 'd'; verMin++, verMax--)
        {
            for (int i = 2; i < 6; i++)
            {
                string moveWhite = $"P{verMin}{i}{verMin}{i + 1}";
                _chess.Move(moveWhite);
                
                string moveBlack = $"p{verMax}{9 - i}{verMax}{9 - i - 1}";
                _chess.Move(moveBlack);
                Console.WriteLine($"{moveWhite}  {moveBlack}");
            }

            Console.WriteLine("------------");
        }
        /*
         * Position after all moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  p  p  .  .  .  .
         * 6    P  P  P  P  .  .  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  p  p  p  p
         * 2    .  .  .  .  P  P  P  P
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */
        
        Assert.Pass();
    }

    [Test]
    public void SimpleMoveFault_BadDirection()
    {
        _chess.Move("Pe2e3");
        _chess.Move("pe7e6");
        
        /*
         * Position after 2 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  p  p  .  p  p  p
         * 6    .  .  .  .  p  .  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  P  .  .  .
         * 2    P  P  P  P  .  P  P  P
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */

        Assert.Throws(
            typeof(ArgumentException), 
            () => _chess.Move("Pe3e2")
        );
        
        _chess.Move("Pe3e4");
        
        Assert.Throws(
            typeof(ArgumentException), 
            () => _chess.Move("Pe6e7")
        );
    }

    [Test]
    public void SimpleMoveFault_NoEmpty()
    {
        _chess.Move("Pe2e3");
        _chess.Move("pe7e6");
        _chess.Move("Pe3e4");
        _chess.Move("pe6e5");
        
        /*
         * Position after 4 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  p  p  .  p  p  p
         * 6    .  .  .  .  .  .  .  .
         * 5    .  .  .  .  p  .  .  .
         * 4    .  .  .  .  P  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    P  P  P  P  .  P  P  P
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */

        Assert.Throws(
            typeof(ArgumentException), 
            () => _chess.Move("Pe4e5")
        );
    }

    // DoubleMove
    [Test]
    public void DoubleMoveSuccess()
    {
        for (char ver = 'a'; ver <= 'h'; ver++)
        {
            string moveWhite = $"P{ver}{2}{ver}{4}"; 
            _chess.Move(moveWhite);
                             
            string moveBlack = $"p{ver}{7}{ver}{5}";
            _chess.Move(moveBlack);
            
            Console.WriteLine($"{moveWhite}  {moveBlack}");
        }
        /*
         * Position after all moves
         * 
         * 8    r  n  b  q  k  b  n  r
         * 7    .  .  .  .  .  .  .  .
         * 6    .  .  .  .  .  .  .  .
         * 5    p  p  p  p  p  p  p  p
         * 4    P  P  P  P  P  P  P  P
         * 3    .  .  .  .  .  .  .  .
         * 2    .  .  .  .  .  .  .  .
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */
        
        Assert.Pass();
    }

    [Test]
    public void DoubleMoveFault_BadDirection()
    {
        _chess.Move("Pe2e4");
        _chess.Move("pe7e5");
        /*
         * Position after 2 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  p  p  .  p  p  p
         * 6    .  .  .  .  .  .  .  .
         * 5    .  .  .  .  p  .  .  .
         * 4    .  .  .  .  P  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    P  P  P  P  .  P  P  P
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */

        Assert.Throws(
            typeof(ArgumentException),
            () => _chess.Move("Pe4e2")
        );
        
        _chess.Move("Pd2d4");
        
        Assert.Throws(
            typeof(ArgumentException),
            () => _chess.Move("pe5e7")
        );
    }

    [Test]
    public void DoubleMoveFault_NoEmpty1()
    {
        _chess.Move("Pe2e4");
        _chess.Move("pd7d5");
        _chess.Move("Pe4e5");
        _chess.Move("pd5d4");
        _chess.Move("Pe5e6");
        /*
         * Position after 5 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  p  .  p  p  p  p
         * 6    .  .  .  .  P  .  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  p  .  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    P  P  P  P  .  P  P  P
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */
        
        Assert.Throws(
            typeof(ArgumentException), 
            () => _chess.Move("pe7e5")
        );
    }

    [Test]
    public void DoubleMoveFault_NoEmpty2()
    {
        _chess.Move("Pe2e4");
        _chess.Move("pd7d5");
        _chess.Move("Pe4e5");
        /*
         * Position after 3 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  p  .  p  p  p  p
         * 6    .  .  .  .  .  .  .  .
         * 5    .  .  .  p  P  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    P  P  P  P  .  P  P  P
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */
        
        Assert.Throws(
            typeof(ArgumentException), 
            () => _chess.Move("pe7e5")
        );
    }

    [Test]
    public void DoubleMoveFault_NotFromStartPos()
    {
        _chess.Move("Pe2e4");
        _chess.Move("pd7d5");
        /*
         * Position after 2 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  p  .  p  p  p  p
         * 6    .  .  .  .  .  .  .  .
         * 5    .  .  .  p  .  .  .  .
         * 4    .  .  .  .  P  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    P  P  P  P  .  P  P  P
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */

        Assert.Throws(
            typeof(ArgumentException),
            () => _chess.Move("Pe4e6")
        );
        
        _chess.Move("Pe4e5");
        _chess.Move("pd5d4");
        /*
         * Position after 4 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  p  .  p  p  p  p
         * 6    .  .  .  .  .  .  .  .
         * 5    .  .  .  .  P  .  .  .
         * 4    .  .  .  p  .  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    P  P  P  P  .  P  P  P
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */
        
        Assert.Throws(
            typeof(ArgumentException),
            () => _chess.Move("Pe5e7")
        );
    }

    // Capture
    [Test]
    public void CaptureSuccess()
    {
        _chess.Move("Pe2e4");
        _chess.Move("pe7e5");
        _chess.Move("Pd2d4");
        _chess.Move("pd7d5");
        /*
         * Position after 4 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  p  .  .  p  p  p
         * 6    .  .  .  .  .  .  .  .
         * 5    .  .  .  p  p  .  .  .
         * 4    .  .  .  P  P  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    P  P  P  .  .  P  P  P
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */
        
        _chess.Move("Pe4d5");
        _chess.Move("pe5d4");
        
        Assert.Pass();
    }

    [Test]
    public void CaptureFault_BadDirection()
    {
        _chess.Move("Pe2e4");
        _chess.Move("pd7d5");
        _chess.Move("Pe4e5");
        _chess.Move("pd5d4");
        /*
         * Position after 4 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  p  .  .  p  p  p
         * 6    .  .  .  .  .  .  .  .
         * 5    .  .  .  .  P  .  .  .
         * 4    .  .  .  p  .  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    P  P  P  P  .  P  P  P
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */
        
        Assert.Throws(
            typeof(ArgumentException),
            () => _chess.Move("Pe5d4")
        );
        
        _chess.Move("Pa2a3");
        
        Assert.Throws(
            typeof(ArgumentException),
            () => _chess.Move("pd4e5")
        );
    }

    [Test]
    public void CaptureFault_Empty()
    {
        /*
         * Position after 0 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  p  p  p  p  p  p
         * 6    .  .  .  .  .  .  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    P  P  P  P  P  P  P  P
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */
        Assert.Throws(
            typeof(ArgumentException),
            () => _chess.Move("Pe2d3")
        );
        Assert.Throws(
            typeof(ArgumentException),
            () => _chess.Move("Pe2f3")
        );
    }

    // EnPassant
    [Test]
    public void EnPassantSuccess()
    {
        _chess.Move("Pe2e4");
        _chess.Move("pc7c5");
        _chess.Move("Pe4e5");
        _chess.Move("pd7d5");
        /*
         * Position after 4 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  .  .  p  p  p  p
         * 6    .  .  .  .  .  .  .  .
         * 5    .  .  p  p  P  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    P  P  P  P  .  P  P  P
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */
        
        _chess.Move("Pe5d6");
        
        _chess.Move("pc5c4");
        _chess.Move("Pd2d3");
        /* 
         * Position after 6 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  .  .  .  p  p  p
         * 6    .  .  .  P  .  .  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  p  P  .  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    P  P  P  .  .  P  P  P
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */
        
        _chess.Move("pc4d3");
        
        Assert.Pass();
    }

    [Test]
    public void EnPassantFault_BadDirection()
    {
        _chess.Move("Pe2e4");
        _chess.Move("pd7d5");
        _chess.Move("Pe4d5");
        _chess.Move("pe7e6");
        _chess.Move("Pd5e6");
        _chess.Move("pa7a6");
        _chess.Move("Pe6e7");
        _chess.Move("pf7f5");
        /*
         * Position after 8 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    .  p  p  .  P  .  p  p
         * 6    p  .  .  .  .  .  .  .
         * 5    .  .  .  .  .  p  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    P  P  P  P  .  P  P  P
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */
        Assert.Throws(
            typeof(ArgumentException),
            () => _chess.Move("Pe7f6")
        );
        Assert.Throws(
            typeof(ArgumentException),
            () => _chess.Move("Pe7d6")
        );
    }

    [Test]
    public void EnPassantFault_NoToEnPassant()
    {
        _chess.Move("Pe2e4");
        _chess.Move("pc7c5");
        _chess.Move("Pe4e5");
        _chess.Move("pd7d5");
        /*
         * Position after 4 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  .  .  p  p  p  p
         * 6    .  .  .  .  .  .  .  .
         * 5    .  .  p  p  P  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    P  P  P  P  .  P  P  P
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */
        
        Assert.Throws(
            typeof(ArgumentException),
            () => _chess.Move("Pe5f6")
        );
    }

    // Promote
    [Test]
    public void PromoteSuccess()
    {
        _chess.Move("Pe2e4");
        _chess.Move("pe7e5");
        _chess.Move("Pf2f4");
        _chess.Move("pf7f5");
        _chess.Move("Pe4f5");
        _chess.Move("pe5f4");
        _chess.Move("Pg2g3");
        _chess.Move("pg7g6");
        _chess.Move("Pf5g6");
        _chess.Move("pf4g3");
        _chess.Move("Pg6h7");
        _chess.Move("pg3h2");
        /*
         * Position after 12 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  p  p  p  .  .  P
         * 6    .  .  .  .  .  .  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    P  P  P  P  .  .  .  p
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */
        
        _chess.Move("Ph7g8Q");
    }
    
    [Test]
    [TestCase("Ph7g8", "ph2g1")]
    [TestCase("Ph7h8Q", "ph2h1q")]
    [TestCase("Ph7g8q", "ph2g1Q")]
    [TestCase("Ph7g8K", "ph2g1k")]
    [TestCase("Ph7g8P", "ph2g1p")]
    public void PromoteFault_BadPromotionFigure(string lastMoveWhite, string lastMoveBlack)
    {
        _chess.Move("Pe2e4");
        _chess.Move("pe7e5");
        _chess.Move("Pf2f4");
        _chess.Move("pf7f5");
        _chess.Move("Pe4f5");
        _chess.Move("pe5f4");
        _chess.Move("Pg2g3");
        _chess.Move("pg7g6");
        _chess.Move("Pf5g6");
        _chess.Move("pf4g3");
        _chess.Move("Pg6h7");
        _chess.Move("pg3h2");
        /*
         * Position after 12 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  p  p  p  .  .  P
         * 6    .  .  .  .  .  .  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    P  P  P  P  .  .  .  p
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */

        Assert.Throws(
            typeof(ArgumentException),
            () => _chess.Move(lastMoveWhite)
        );
        
        _chess.Move("Ph7g8Q");
        Assert.Throws(
            typeof(ArgumentException),
            () => _chess.Move(lastMoveBlack)
        );
    }
    
    [Test]
    public void PromoteFault_BadLine()
    {
        /*
         * Position after 0 moves
         *
         * 8    r  n  b  q  k  b  n  r
         * 7    p  p  p  p  p  p  p  p
         * 6    .  .  .  .  .  .  .  .
         * 5    .  .  .  .  .  .  .  .
         * 4    .  .  .  .  .  .  .  .
         * 3    .  .  .  .  .  .  .  .
         * 2    P  P  P  P  P  P  P  P
         * 1    R  N  B  Q  K  B  N  R
         *
         *      a  b  c  d  e  f  g  h
         */

        Assert.Throws(
            typeof(ArgumentException),
            () => _chess.Move("Pe2e3Q")
        );
    }
}