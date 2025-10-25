using Chess.Global;

namespace Chess.Model.Pieces;

public class Pawn : Piece
{
    public Pawn(PlayerColors color, Square location) : base(color, location)
    {
    }

    public override List<Move> GetLegalMoves(Board board)
    {
        List<Move> legalMoves = new List<Move>();

        int rank = Location.Rank;
        int file = Location.File;

        if (Color == PlayerColors.WHITE && rank == 1)
        {
            Move? possibleDoublePush = FindPossiblePush(rank + 2, file, board);
            Move? moveToIntermediateSquare = FindPossiblePush(rank + 1, file, board);
            if (possibleDoublePush is not null && moveToIntermediateSquare is not null) legalMoves.Add(possibleDoublePush);
        }

        if (Color == PlayerColors.BLACK && rank == 6)
        {
            Move? possibleDoublePush = FindPossiblePush(rank - 2, file, board);
            if (possibleDoublePush is not null) legalMoves.Add(possibleDoublePush);
        }

        rank = Color == PlayerColors.WHITE ? rank + 1 : rank - 1; //Move up for white pawns, or down for black.

        Move? possiblePush = FindPossiblePush(rank, file, board);
        if (possiblePush is not null) legalMoves.Add(possiblePush);

        if (file < 7)
        {
            Move? possibleCapture = FindPossibleCaptures(rank, file + 1, board);
            if (possibleCapture is not null) legalMoves.Add(possibleCapture);
        }

        if (file > 0)
        {
            Move? possibleCapture = FindPossibleCaptures(rank, file - 1, board);
            if (possibleCapture is not null) legalMoves.Add(possibleCapture);
        }

        return legalMoves;
    }

    private Move? FindPossiblePush(int rank, int file, Board board)
    {
        Piece? pieceInWay = board.PieceAt(board.SquareAt(rank, file));
        if (pieceInWay is null) return new Move(Location, board.SquareAt(rank, file), this);

        return null;
    }


    private Move? FindPossibleCaptures(int rank, int file, Board board)
    {
        Piece? pieceToCapture = board.PieceAt(board.SquareAt(rank, file));
        if (pieceToCapture is not null && pieceToCapture.Color != Color) return new Move(Location, board.SquareAt(rank, file), this, pieceToCapture);

        return null;
    }
}