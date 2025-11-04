using Chess.Global;

namespace Chess.Model.Pieces;

public class Bishop : SlidingPiece
{
    public Bishop(PlayerColors color, Square location, bool hasMoved = false) : base(color, location, hasMoved, PieceType.BISHOP) { }

    protected override List<Move> GetPseudoLegalMoves()
    {
        return GetSlidingPseudoLegalMoves();
    }

    public override List<Square> GetAttackedSquares()
    {
        int[] xDirections = new[] { 1, 1, -1, -1 };
        int[] yDirections = new[] { 1, -1, 1, -1 };

        return GetSlidingAttackedSquares(xDirections, yDirections);
    }
}
