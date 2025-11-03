using Chess.Global;

namespace Chess.Model.Pieces;

public class Knight : Piece
{
    public Knight(PlayerColors color, Square location, bool hasMoved = false) : base(color, location, hasMoved, PieceType.KNIGHT) { }

    protected override List<Move> GetPseudoLegalMoves()
    {
        Board board = Location.Board;
        List<Move> pseudoLegalMoves = new List<Move>();
        foreach (Square s in GetAttackedSquares())
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

    public override List<Square> GetAttackedSquares()
    {
        Board board = Location.Board;
        List<Square> attackedSquares = new List<Square>();
        int[] xDirections = new[] { 2, 2, 1, -1, -2, -2, -1, 1 };
        int[] yDirections = new[] { 1, -1, 2, 2, 1, -1, -2, -2 };
        for (int i = 0; i < 8; i++)
        {
            int rank = Location.Rank + xDirections[i];
            int file = Location.File + yDirections[i];
            if (rank < 0 || rank > 7 || file < 0 || file > 7) continue;
            attackedSquares.Add(board.SquareAt(rank, file));
        }

        return attackedSquares;
    }
}