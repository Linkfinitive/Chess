using Chess.Global;

namespace Chess.Model.Pieces;

public class Rook : Piece
{
    public Rook(PlayerColors color, Square location) : base(color, location)
    {
    }

    public override List<Move> GetLegalMoves(Board board)
    {
        List<Move> legalMoves = new List<Move>();

        int[] dx = new[] { 1, -1, 0, 0 }; // Right, Left, 0, 0
        int[] dy = new[] { 0, 0, 1, -1 }; // 0, 0, Up, Down

        for (int direction = 0; direction < 4; direction++)
        {
            int rank = Location.Rank;
            int file = Location.File;
            while (rank < 8 && file < 8 && rank >= 0 && file >= 0)
            {
                rank += dx[direction];
                file += dy[direction];

                Piece? pieceInWay = board.Pieces.Find(p => p.Location.File == file && p.Location.Rank == rank);
                if (pieceInWay is not null && pieceInWay.Color == Color) break;

                if (pieceInWay is not null && pieceInWay.Color != Color)
                {
                    legalMoves.Add(new Move(Location, pieceInWay.Location, this, pieceInWay));
                    break;
                }

                Square? newLocation = board.Squares.Find(s => s.File == file && s.Rank == rank);
                if (newLocation is not null) legalMoves.Add(new Move(Location, newLocation, this, null));
            }
        }
        return legalMoves;
    }
}
