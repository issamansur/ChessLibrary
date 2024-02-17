namespace ChessMaster.Domain.States;

public enum GameState
{
    None,
    Playing,
    Check,
    Checkmate,
    Stalemate,
}