using Chess.Global;

namespace Chess.Model.Pieces;

public class Queen : SlidingPiece
{
    public Queen(PlayerColors color, Square location, bool hasMoved = false) : base(color, location, hasMoved, PieceType.QUEEN) { }

    protected override List<Move> GetPseudoLegalMoves()
    {
        return GetSlidingPseudoLegalMoves();
    }

    public override List<Square> GetAttackedSquares()
    {
        int[] xDirections = new[] { 1, -1, 0, 0, 1, 1, -1, -1 };
        int[] yDirections = new[] { 0, 0, 1, -1, 1, -1, 1, -1 };

        return GetSlidingAttackedSquares(xDirections, yDirections);
    }
}