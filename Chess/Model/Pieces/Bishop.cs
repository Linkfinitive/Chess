using Chess.Global;

namespace Chess.Model.Pieces;

public class Bishop : SlidingPiece
{
    public Bishop(PlayerColors color, Square location) : base(color, location)
    {
    }

    public override List<Move> GetLegalMoves(Board board)
    {
        int[] xDirections = new[] { 1, -1, 1, -1 };
        int[] yDirections = new[] { 1, 1, -1, -1 };

        return GetLegalSlidingMoves(xDirections, yDirections, board);
    }
}