using Chess.Global;

namespace Chess.Model.Pieces;

public class King : Piece
{
    public King(PlayerColors color, Square location) : base(color, location)
    {
    }

    public override List<Move> GetLegalMoves(Board board)
    {
        int[] xDirections = new[] { 1, -1, 0, 0, 1, 1, -1, -1 };
        int[] yDirections = new[] { 0, 0, 1, -1, 1, -1, 1, -1 };

        List<Move> legalMoves = GetLegalSingleSpaceSlidingMoves(xDirections, yDirections, board);

        //TODO: Add castling

        return legalMoves;
    }

    private List<Move> GetLegalSingleSpaceSlidingMoves(int[] xDirections, int[] yDirections, Board board)
    {
        if (xDirections.Length != yDirections.Length) throw new ArgumentException("Number of x directions must be equal to y directions.");
        if (xDirections.Length is not 8) throw new ArgumentOutOfRangeException(nameof(xDirections), "Number of directions must 8.");

        List<Move> legalMoves = new List<Move>();

        for (int direction = 0; direction < xDirections.Length; direction++)
        {
            int rank = Location.Rank;
            int file = Location.File;

            while (true)
            {
                rank += xDirections[direction];
                file += yDirections[direction];

                if (rank < 0 || rank > 7 || file < 0 || file > 7) break;

                Piece? pieceInWay = board.PieceAt(board.SquareAt(rank, file));
                if (pieceInWay is not null && pieceInWay.Color == Color) break;

                if (pieceInWay is not null && pieceInWay.Color != Color)
                {
                    legalMoves.Add(new Move(Location, pieceInWay.Location, this, pieceInWay));
                    break;
                }

                legalMoves.Add(new Move(Location, board.SquareAt(rank, file), this));
                break;
            }
        }

        return legalMoves;
    }
}