using Chess.Global;

namespace Chess.Model.Pieces;

public class King : Piece
{
    public King(PlayerColors color, Square location, bool hasMoved = false) : base(color, location, hasMoved)
    {
    }

    public bool IsInCheck
    {
        get
        {
            PlayerColors otherPlayer = Color == PlayerColors.WHITE ? PlayerColors.BLACK : PlayerColors.WHITE;
            return Location.IsAttackedBy(otherPlayer);
        }
    }

    protected override List<Move> GetPseudoLegalMoves(Board board)
    {
        //Adds the regular moves
        List<Move> legalMoves = GetPseudoLegalSingleSpaceSlidingMoves(board);

        // Castling
        //You cannot castle if the king has moved before
        if (HasMoved) return legalMoves;

        //Calculate the other player - we'll need this in a sec.
        PlayerColors otherPlayer = Color == PlayerColors.WHITE ? PlayerColors.BLACK : PlayerColors.WHITE;

        //You cannot castle if the king is in check
        if (IsInCheck) return legalMoves;

        //Find the moving player's rooks that have not yet moved. Then we can iterate through them to check if they can castle.
        List<Rook> unMovedFriendlyRooks = new List<Rook>();
        foreach (Piece p in board.Pieces)
        {
            if (p.GetType().Name != "Rook") continue;
            if (p.HasMoved) continue;
            if (p.Color == otherPlayer) continue;
            unMovedFriendlyRooks.Add((Rook)p);
        }

        //If there are no unmoved friendly rooks, we can return early (skipping this iterator).
        foreach (Rook r in unMovedFriendlyRooks)
            switch (r.Location.File)
            {
                case 0: //Queenside rook is always on File 0 if it hasn't moved.
                    //Check that the intermediate squares are empty
                    if (board.PieceAt(board.SquareAt(Location.Rank, 1)) is not null) break;
                    if (board.PieceAt(board.SquareAt(Location.Rank, 2)) is not null) break;
                    if (board.PieceAt(board.SquareAt(Location.Rank, 3)) is not null) break;

                    //Check that the intermediate square is not attacked - you can't castle through check.
                    //Checking the final square is done automatically by the legal move generator later.
                    if (board.SquareAt(Location.Rank, 3).IsAttackedBy(otherPlayer)) break;

                    //If we've passed all tests, we can add the castling move.
                    legalMoves.Add(new Move(Location, board.SquareAt(Location.Rank, 2), this));
                    break;

                case 7: //Repeat the above steps for the Kingside rook on File 7.
                    if (board.PieceAt(board.SquareAt(Location.Rank, 5)) is not null) break;
                    if (board.PieceAt(board.SquareAt(Location.Rank, 6)) is not null) break;
                    if (board.SquareAt(Location.Rank, 5).IsAttackedBy(otherPlayer)) break;
                    legalMoves.Add(new Move(Location, board.SquareAt(Location.Rank, 6), this));
                    break;
            }

        return legalMoves;
    }

    private List<Move> GetPseudoLegalSingleSpaceSlidingMoves(Board board)
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

    public override List<Square> GetAttackedSquares(Board board)
    {
        List<Square> attackedSquares = new List<Square>();
        int[] xDirections = { 1, -1, 0, 0, 1, 1, -1, -1 };
        int[] yDirections = { 0, 0, 1, -1, 1, -1, 1, -1 };
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