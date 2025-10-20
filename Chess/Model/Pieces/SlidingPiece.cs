using Chess.Controller;
using Chess.Global;

namespace Chess.Model.Pieces;

public abstract class SlidingPiece : Piece
{
    protected SlidingPiece(PlayerColors color, Square location) : base(color, location)
    {
    }

    protected List<Move> GetLegalSlidingMoves(int[] xDirections, int[] yDirections)
    {
        if (xDirections.Length != yDirections.Length) throw new ArgumentException("Number of x directions must be equal to y directions.");
        if (xDirections.Length is not (4 or 8)) throw new ArgumentOutOfRangeException(nameof(xDirections), "Number of directions must be 4 or 8.");

        List<Move> legalMoves = new List<Move>();

        for (int direction = 0; direction < xDirections.Length; direction++)
        {
            int rank = Location.Rank;
            int file = Location.File;

            while (rank < 8 && file < 8 && rank >= 0 && file >= 0)
            {
                rank += xDirections[direction];
                file += yDirections[direction];

                Piece? pieceInWay = GameController.Instance.Board.Pieces.Find(p => p.Location.File == file && p.Location.Rank == rank);
                if (pieceInWay is not null && pieceInWay.Color == Color) break;

                if (pieceInWay is not null && pieceInWay.Color != Color)
                {
                    legalMoves.Add(new Move(Location, pieceInWay.Location, this, pieceInWay));
                    break;
                }

                Square? newLocation = GameController.Instance.Board.Squares.Find(s => s.File == file && s.Rank == rank);
                if (newLocation is not null) legalMoves.Add(new Move(Location, newLocation, this, null));
            }
        }

        return legalMoves;
    }
}
