using Chess.Controller;
using Chess.Global;

namespace Chess.Model.Pieces;

public class King : Piece
{
    public King(PlayerColors color, Square location) : base(color, location)
    {
    }

    public override List<Move> GetLegalMoves()
    {
        int[] xDirections = new[] { 1, -1, 0, 0, 1, 1, -1, -1 };
        int[] yDirections = new[] { 0, 0, 1, -1, 1, -1, 1, -1 };

        List<Move> legalMoves = GetLegalSingleSpaceSlidingMoves(xDirections, yDirections);

        //TODO: Add castling

        return legalMoves;
    }

    private List<Move> GetLegalSingleSpaceSlidingMoves(int[] xDirections, int[] yDirections)
    {
        if (xDirections.Length != yDirections.Length) throw new ArgumentException("Number of x directions must be equal to y directions.");
        if (xDirections.Length is not 8) throw new ArgumentOutOfRangeException(nameof(xDirections), "Number of directions must 8.");

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
                if (newLocation is not null) legalMoves.Add(new Move(Location, newLocation, this));
                break;
            }
        }

        return legalMoves;
    }
}
