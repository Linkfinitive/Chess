using Chess.Global;

namespace Chess.Model.Pieces;

public class Queen : SlidingPiece
{
    public Queen(PlayerColors color, Square location, bool hasMoved = false) : base(color, location, hasMoved)
    {
    }

    protected override List<Move> GetPseudoLegalMoves(Board board)
    {
        return GetSlidingPseudoLegalMoves(board);
    }

    public override List<Square> GetAttackedSquares(Board board)
    {
        int[] xDirections = new[] { 1, -1, 0, 0, 1, 1, -1, -1 };
        int[] yDirections = new[] { 0, 0, 1, -1, 1, -1, 1, -1 };

        return GetSlidingAttackedSquares(xDirections, yDirections, board);
    }
}