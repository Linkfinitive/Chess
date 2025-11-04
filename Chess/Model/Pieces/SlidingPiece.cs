using Chess.Global;

namespace Chess.Model.Pieces;

public abstract class SlidingPiece : Piece
{
    protected SlidingPiece(PlayerColors color, Square location, bool hasMoved, PieceType type) : base(color, location, hasMoved, type) { }

    protected List<Move> GetSlidingPseudoLegalMoves(Board board)
    {
        List<Move> pseudoLegalMoves = new List<Move>();
        foreach (Square s in GetAttackedSquares(board))
        {
            Piece? pieceInWay = board.PieceAt(s);
            if (pieceInWay is not null && pieceInWay.Color == Color) continue;

            if (pieceInWay is not null && pieceInWay.Color != Color)
            {
                pseudoLegalMoves.Add(new Move(Location, pieceInWay.Location, this, pieceInWay));
                continue;
            }

            pseudoLegalMoves.Add(new Move(Location, s, this));
        }

        return pseudoLegalMoves;
    }

    protected List<Square> GetSlidingAttackedSquares(int[] xDirections, int[] yDirections, Board board)
    {
        List<Square> attackedSquares = new List<Square>();

        for (int i = 0; i < xDirections.Length; i++)
        {
            int rank = Location.Rank;
            int file = Location.File;

            while (true)
            {
                rank += xDirections[i];
                file += yDirections[i];

                if (rank < 0 || rank > 7 || file < 0 || file > 7) break;

                Square target = board.SquareAt(rank, file);
                attackedSquares.Add(target);

                // If we just hit a piece, then we aren't attacking the squares behind it.
                if (board.PieceAt(target) is not null) break;
            }
        }

        return attackedSquares;
    }
}
