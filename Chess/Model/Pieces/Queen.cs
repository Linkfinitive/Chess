using Chess.Global;

namespace Chess.Model.Pieces;

public class Queen : SlidingPiece
{
    public Queen(PlayerColors color, Square location) : base(color, location)
    {
    }

    public override List<Move> GetLegalMoves()
    {
        int[] xDirections = new[] { 1, -1, 0, 0 , 1, 1, -1, -1};
        int[] yDirections = new[] { 0, 0, 1, -1 , 1, -1, 1, -1};

        return GetLegalSlidingMoves(xDirections, yDirections);
    }
}
