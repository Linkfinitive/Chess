using Chess.Global;

namespace Chess.Model.Pieces;

public class Knight : Piece
{
    public Knight(PlayerColors color, Square location) : base(color, location)
    {
    }

    public override List<Move> GetLegalMoves(Board board)
    {
        List<Move> legalMoves = new List<Move>();

        int[] xDirections = new[] { 2, 2, 1, -1, -2, -2, -1, 1 };
        int[] yDirections = new[] { 1, -1, 2, 2, 1, -1, -2, -2 };

        int rank = Location.Rank;
        int file = Location.File;

        for (int direction = 0; direction < xDirections.Length; direction++)
        {
            rank += xDirections[direction];
            file += yDirections[direction];

            if (rank < 0 || rank > 7 || file < 0 || file > 7) continue;

            Square targetSquare = board.SquareAt(rank, file);
            Piece? pieceOnTarget = board.PieceAt(targetSquare);

            if (pieceOnTarget is null)
                legalMoves.Add(new Move(Location, targetSquare, this));
            else if (pieceOnTarget.Color != Color) legalMoves.Add(new Move(Location, targetSquare, this, pieceOnTarget));
        }

        return legalMoves;
    }
}
