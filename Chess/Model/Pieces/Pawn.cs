using Chess.Controller;
using Chess.Global;

namespace Chess.Model.Pieces;

public class Pawn : Piece
{
    public Pawn(PlayerColors color, Square location, bool hasMoved = false) : base(color, location, hasMoved, PieceType.PAWN) { }

    protected override List<Move> GetPseudoLegalMoves()
    {
        List<Move> pseudoLegalMoves = new List<Move>();

        Move? pseudoLegalEnPassant = GetPseudoLegalEnPassant();
        Move? pseudoLegalDoublePush = GetPseudoLegalDoublePush();
        Move? pseudoLegalPush = GetPseudoLegalPush();
        List<Move> pseudoLegalCaptures = GetPseudoLegalCaptures();

        if (pseudoLegalEnPassant is not null) pseudoLegalMoves.Add(pseudoLegalEnPassant);
        if (pseudoLegalDoublePush is not null) pseudoLegalMoves.Add(pseudoLegalDoublePush);
        if (pseudoLegalPush is not null) pseudoLegalMoves.Add(pseudoLegalPush);
        pseudoLegalMoves.AddRange(pseudoLegalCaptures);

        return pseudoLegalMoves;
    }

    private Move? GetPseudoLegalEnPassant()
    {
        //Get the most recently made move, because en passant can only be done if the most recent move was a double push.
        //TODO: Remove this global state.
        Move? mostRecentMove = GameController.Instance.MoveHistory.MostRecentMove;

        //This would only happen if it's the first move of the game, and naturally there would be no en passant available.
        if (mostRecentMove is null) return null;

        //If the most recent move wasn't a double push, then there is no en passant available.'
        if (!mostRecentMove.IsDoublePush) return null;

        //If the pawn hasn't moved to the same rank as this pawn, then there is no en passant available.
        if (mostRecentMove.To.Rank != Location.Rank) return null;

        //For every square that the pawn would normally attack, if it's in file as the just-double-pushed pawn, then we can take by en passant.
        foreach (Square s in GetAttackedSquares())
        {
            if (mostRecentMove.To.File == s.File)
            {
                return new Move(Location, s, this, mostRecentMove.PieceMoved);
            }
        }

        return null;
    }

    private Move? GetPseudoLegalDoublePush()
    {
        Board board = Location.Board;

        //Cannot perform double push if the piece has already moved.
        if (HasMoved) return null;

        int rank = Location.Rank;
        int file = Location.File;
        int direction = GetDirection();

        Square destinationSquare = board.SquareAt(rank + 2 * direction, file);
        Square intermediateSquare = board.SquareAt(rank + direction, file);

        Piece? pieceOnDestinationSquare = board.PieceAt(destinationSquare);
        Piece? pieceOnIntermediateSquare = board.PieceAt(intermediateSquare);

        if (pieceOnDestinationSquare is null && pieceOnIntermediateSquare is null) return new Move(Location, destinationSquare, this);
        return null;
    }

    private Move? GetPseudoLegalPush()
    {
        Board board = Location.Board;

        int rank = Location.Rank;
        int file = Location.File;
        int direction = GetDirection();

        Square destinationSquare = board.SquareAt(rank + direction, file);
        Piece? pieceInWay = board.PieceAt(destinationSquare);
        return pieceInWay is null ? new Move(Location, destinationSquare, this) : null;
    }

    private List<Move> GetPseudoLegalCaptures()
    {
        Board board = Location.Board;

        List<Move> pseudoLegalCaptures = new List<Move>();
        foreach (Square s in GetAttackedSquares())
        {
            Piece? pieceToCapture = board.PieceAt(s);
            if (pieceToCapture is not null && pieceToCapture.Color != Color)
            {
                pseudoLegalCaptures.Add(new Move(Location, s, this, pieceToCapture));
            }
        }

        return pseudoLegalCaptures;
    }

    public override List<Square> GetAttackedSquares()
    {
        Board board = Location.Board;

        List<Square> attackedSquares = new List<Square>();
        int direction = GetDirection();
        int rank = Location.Rank + direction;
        foreach (int fileOffset in new[] { -1, 1 })
        {
            int file = Location.File + fileOffset;
            if (rank is >= 0 and < 8 && file is >= 0 and < 8)
            {
                attackedSquares.Add(board.SquareAt(rank, file));
            }
        }

        return attackedSquares;
    }

    private int GetDirection()
    {
        return Color == PlayerColors.WHITE ? 1 : -1; //Move up for white pawns, or down for black.
    }
}