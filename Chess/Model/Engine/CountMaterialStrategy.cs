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
            int pieceScore = p.GetType().Name switch
            {
                "Pawn" => 1,
                "Knight" => 3,
                "Bishop" => 3,
                "Rook" => 5,
                "Queen" => 9,
                "King" => 0,
                _ => throw new Exception("Invalid piece type.")
            };

            if (p.Color == PlayerColors.BLACK) pieceScore *= -1;
            evaluation += pieceScore;
        }

        return evaluation;
    }
}
