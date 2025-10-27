using Chess.Global;

namespace Chess.Model.Pieces;

public abstract class Piece
{
    protected Piece(PlayerColors color, Square location, bool hasMoved)
    {
        //The constructor gives the option to set if the piece has moved because it matters for promoted pieces, and cloned boards.
        Color = color;
        Location = location;
        HasMoved = hasMoved;
    }

    public PlayerColors Color { get; }
    public Square Location { get; set; }

    public bool HasMoved { get; set; }
    public bool IsPickedUp { get; set; }

    public abstract List<Move> GetPseudoLegalMoves(Board board);

    public List<Move> GetLegalMoves(Board board)
    {
        List<Move> pseudoLegalMoves = GetPseudoLegalMoves(board);
        List<Move> legalMoves = new List<Move>();

        foreach (Move m in pseudoLegalMoves)
        {
            //Take a copy of the board and move and execute the move on the cloned board.
            Board clonedBoard = board.Clone();
            m.Clone(clonedBoard).Execute();

            //Check that the king of the moving player is not in check.
            King? friendlyKing = clonedBoard.Pieces.Find(p => p.GetType().Name == "King" && p.Color == Color) as King;
            if (friendlyKing is null) throw new NullReferenceException("King not found - something has gone seriously wrong.");
            if (friendlyKing.IsInCheck) continue;

            //If the move is fully legal, we can add it to the list to return.
            legalMoves.Add(m);
        }

        return legalMoves;
    }

    public Piece Clone(Board newBoard)
    {
        Square newLocation = newBoard.SquareCalled(Location.GetAlgebraicPosition());
        return PieceFactory.CreatePiece(GetType().Name, Color, newLocation, HasMoved);
    }

    public abstract List<Square> GetAttackedSquares(Board board);
}
