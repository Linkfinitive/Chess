using Chess.Global;
using Chess.Model.Pieces;

namespace Chess.Model.Engine;

public class CountMaterialStrategy : IEvaluationStrategy
{
    public int Evaluate(Board board)
    {
        int evaluation = 0;

        foreach (Piece p in board.Pieces)
        {
            int pieceScore = p.Type switch
            {
                PieceType.PAWN => 1,
                PieceType.KNIGHT => 3,
                PieceType.BISHOP => 3,
                PieceType.ROOK => 5,
                PieceType.QUEEN => 9,
                PieceType.KING => 0,
                _ => throw new Exception("Invalid piece type.")
            };

            if (p.Color == PlayerColors.BLACK) pieceScore *= -1;
            evaluation += pieceScore;
        }

        return evaluation;
    }
}
