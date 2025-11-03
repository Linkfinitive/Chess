namespace Chess.Global;

public enum PlayerColors
{
    WHITE,
    BLACK
}

public enum GameStatus
{
    WHITE_TO_MOVE,
    BLACK_TO_MOVE,
    CHECKMATE,
    STALEMATE,
    DRAW
}

public enum PieceType
{
    PAWN,
    KNIGHT,
    BISHOP,
    ROOK,
    QUEEN,
    KING
}