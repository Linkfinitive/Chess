using Chess.Controller;
using Chess.Global;

namespace Chess.Model.Pieces;

public class Pawn : Piece
{
    public Pawn(PlayerColors color, Square location) : base(color, location)
    {
    }

    public override List<Move> GetLegalMoves()
    {
        List<Move> legalMoves = new List<Move>();

        int rank = Location.Rank;
        int file = Location.File;

        if (Color == PlayerColors.WHITE && rank == 1)
        {
            Move? possibleDoublePush = FindPossiblePush(rank + 2, file);
            if (possibleDoublePush is not null) legalMoves.Add(possibleDoublePush);
        }

        if (Color == PlayerColors.BLACK && rank == 6)
        {
            Move? possibleDoublePush = FindPossiblePush(rank - 2, file);
            if (possibleDoublePush is not null) legalMoves.Add(possibleDoublePush);
        }

        rank = Color == PlayerColors.WHITE ? rank + 1 : rank - 1; //Move up for white pawns, or down for black.

        Move? possiblePush = FindPossiblePush(rank, file);
        if (possiblePush is not null) legalMoves.Add(possiblePush);

        if (file < 7)
        {
            Move? possibleCapture = FindPossibleCaptures(rank, file + 1);
            if (possibleCapture is not null) legalMoves.Add(possibleCapture);
        }

        if (file > 0)
        {
            Move? possibleCapture = FindPossibleCaptures(rank, file - 1);
            if (possibleCapture is not null) legalMoves.Add(possibleCapture);
        }

        return legalMoves;
    }

    private Move? FindPossiblePush(int rank, int file)
    {
        Piece? pieceInWay = GameController.Instance.Board.Pieces.Find(p => p.Location.File == file && p.Location.Rank == rank);
        if (pieceInWay is null)
        {
            Square? newLocation = GameController.Instance.Board.Squares.Find(s => s.File == file && s.Rank == rank);
            if (newLocation is not null) return new Move(Location, newLocation, this);
        }

        return null;
    }


    private Move? FindPossibleCaptures(int rank, int file)
    {
        Piece? pieceToCapture = GameController.Instance.Board.Pieces.Find(p => p.Location.File == file && p.Location.Rank == rank);
        if (pieceToCapture is not null && pieceToCapture.Color != Color)
        {
            Square? newLocation = GameController.Instance.Board.Squares.Find(s => s.File == file && s.Rank == rank);
            if (newLocation is not null) return new Move(Location, newLocation, this, pieceToCapture);
        }

        return null;
    }
}