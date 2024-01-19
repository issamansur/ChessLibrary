namespace ChessLibrary
{
    public class Board
    {
        public string Fen { get; private set; }
        private Figure[,] Figures { get; set; }
        public Color MoveColor { get; private set; }
        public int MoveNumber { get; private set; }
        public bool CanCastleA1 { get; private set; }
        public bool CanCastleH1 { get; private set; }
        public bool CanCastleA8 { get; private set; }
        public bool CanCastleH8 { get; private set; }

        public Square EnPassant { get; private set; }
        public int HalfMoveClock { get; private set; }

        public Board(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            Fen = fen;

            // Default
            Figures = new Figure[8, 8];
            HalfMoveClock = 0;
            CanCastleA1 = true;
            CanCastleH1 = true;
            CanCastleA8 = true;
            CanCastleH8 = true;
            EnPassant = Square.None;

            // By Fen
            InitByFen(Fen);
        }
        
        private void InitByFen(string fen)
        {
            string[] parts = fen.Split();
            if (parts.Length != 6)
                return;

            // 1. Piece placement data
            string coordinates = parts[0];
            string[] horisontals = coordinates.Split('/');

            for (int horizonal = 0; horizonal < 8; horizonal++)
            {
                int vertical = 0;
                foreach (char figure in horisontals[7 - horizonal])
                {
                    if (char.IsDigit(figure))
                        vertical += figure - '0';
                    else
                    {
                        Figures[vertical, horizonal] = (Figure)figure;
                        vertical++;
                    }
                }
            }

            // 2. Active color
            MoveColor = parts[1] == "b" ? Color.Black : Color.White;

            // 3. Castling availability
            CanCastleA1 = parts[2].Contains('Q');
            CanCastleH1 = parts[2].Contains('K');
            CanCastleA8 = parts[2].Contains('q');
            CanCastleH8 = parts[2].Contains('k');

            // 4. En Passant
            EnPassant = parts[3] == "-" ? Square.None : new Square(parts[3]);

            // 5. HalfMove Clock
            HalfMoveClock = int.Parse(parts[4]);

            // 6. Fullmove number
            MoveNumber = int.Parse(parts[5]);
        }

        private void GenerateFen()
        {
            // 1. Piece placement data
            string coordinates = "";
            
            for (int horizonal = 7; horizonal >= 0; horizonal--)
            {
                int emptyCounter = 0;
                for (int vertical = 0; vertical < 8; vertical++)
                {
                    Figure figure = Figures[vertical, horizonal];
                    // If no fugure in position
                    if (figure == Figure.None)
                    {
                        emptyCounter += 1;
                        if (vertical == 7)
                        {
                            coordinates += emptyCounter;
                        }
                    }
                    // If figure exist
                    else
                    {
                        if (emptyCounter != 0)
                        {
                            coordinates += emptyCounter;
                            emptyCounter = 0;
                        }
                        coordinates += (char)figure;
                    }
                }

                // Add separator
                if (horizonal != 0)
                {
                    coordinates += '/';
                }
            }

            // 2. Active color
            string moveColor = MoveColor == Color.White ? "w": "b";

            // 3. Castling availability
            string castleAv = "";
            if (CanCastleH1)
            {
                castleAv += "K";
            }
            if (CanCastleA1)
            {
                castleAv += "Q";
            }
            if (CanCastleH8)
            {
                castleAv += "k";
            }
            if (CanCastleA8)
            {
                castleAv += "q";
            }

            // 4. En Passant
            string enPassant = EnPassant == Square.None ? "-" : EnPassant.ToString();

            // 5. HalfMove Clock
            string halfMove = HalfMoveClock.ToString();

            // 6. Fullmove number
            string moveNumber = MoveNumber.ToString();
            
            // Create fen
            Fen = coordinates + " " + moveColor + " " + castleAv + " " + enPassant + " " + halfMove + " " + moveNumber;
        }

        public Figure GetFigureAt(Square square)
        {
            return !square.OnBoard() ? Figure.None : Figures[square.X, square.Y];
        }
        
        private void SetFigureAt(Square square, Figure figure)
        {
            if (!square.OnBoard())
                return;
            Figures[square.X, square.Y] = figure;
        }
        
        public Board Move(FigureMoving move)
        {
            // Create Board
            Board nextBoard = new Board(Fen);

            // Main moving
            nextBoard.SetFigureAt(move.From, Figure.None);
            nextBoard.SetFigureAt(move.To, move.Promotion != Figure.None ? move.Promotion : move.Figure);
            
            // Check enPassant and Promotion
            if (move.Figure is Figure.WhitePawn or Figure.BlackPawn)
            {
                if (move.To == EnPassant)
                {
                    nextBoard.SetFigureAt(new Square(move.To.X, move.From.Y), Figure.None);
                }

                if (move.To.X is 0 or 7 && move.Promotion != Figure.None)
                {
                    nextBoard.SetFigureAt(move.To, move.Promotion);
                }
            }
            
            // Set enPassant
            if (move.Figure is Figure.WhitePawn or Figure.BlackPawn && move.AbsDeltaY == 2)
            {
                nextBoard.EnPassant = move.To.Y switch
                {
                    3 => new Square(move.To.X, 2),
                    4 => new Square(move.To.X, 5),
                    _ => throw new Exception("InvalidSituation")
                };
            }
            else
            {
                nextBoard.EnPassant = Square.None;
            }

            // If it pawn increase HalfMoveClock, else set 0
            if (nextBoard.GetFigureAt(move.From) is Figure.BlackPawn or Figure.WhitePawn)
            {
                nextBoard.HalfMoveClock = 0;
            }
            else
            {
                nextBoard.HalfMoveClock++;
            }

            // If now was Black Move, then increase MoveNumber
            if (MoveColor == Color.Black)
            {
                nextBoard.MoveNumber++;
            }

            // Change MoveColor
            nextBoard.MoveColor = MoveColor.FlipColor();
            
            // Set fen via changes
            nextBoard.GenerateFen();

            return nextBoard;
        }

        public IEnumerable<FigureOnSquare> YieldFigures()
        {
            foreach (Square square in Square.YieldSquares())
            {
                Figure current = GetFigureAt(square);
                if (current.GetColor() == MoveColor)
                {
                    yield return new FigureOnSquare(current, square);
                }
            }
        }

        private Square FindEnemyKing()
        {
            Figure badKing = MoveColor == Color.Black? Figure.WhiteKing : Figure.BlackKing;
            return Square.YieldSquares().FirstOrDefault(s => GetFigureAt(s) == badKing);
        }
        
        private bool CanEatEnemyKing()
        {
            Square badKing = FindEnemyKing();
            Moves moves = new Moves(this);
            foreach (FigureOnSquare fs in YieldFigures())
            {
                FigureMoving figMov = new FigureMoving(fs, badKing);
                if (fs.Figure.GetColor() == MoveColor && moves.CanMove(figMov))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsCheckNow()
        {
            Board board = new Board(Fen);
            board.MoveColor = MoveColor.FlipColor();
            return board.CanEatEnemyKing();
        }
        
        public bool IsCheckAfterMove(FigureMoving figureMoving)
        {
            Board after = Move(figureMoving);
            return after.CanEatEnemyKing();
        }
    }
}