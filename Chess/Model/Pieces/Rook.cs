using Chess.Global;

namespace Chess.Model.Pieces;

public class Rook : SlidingPiece
{
    public Rook(PlayerColors color, Square location) : base(color, location)
    {
    }

    public override List<Move> GetLegalMoves()
    {
        int[] xDirections = new[] { 1, -1, 0, 0 }; // Right, Left, 0, 0
        int[] yDirections = new[] { 0, 0, 1, -1 }; // 0, 0, Up, Down

        return GetLegalSlidingMoves(xDirections, yDirections);
    }
}
