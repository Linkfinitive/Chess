using Chess.Global;

namespace Chess.Model.Pieces;

public abstract class Piece
{
    private Square _location;

    public PieceType Type { get; }

    protected Piece(PlayerColors color, Square location, bool hasMoved, PieceType type)
    {
        //The constructor gives the option to set if the piece has moved because it matters for promoted pieces, and cloned boards.
        //In general, though, HasMoved is automatically set to true when the Location is updated.
        Color = color;
        _location = location;
        HasMoved = hasMoved;
        Type = type;
    }

    public PlayerColors Color { get; }

    public Square Location
    {
        get => _location;
        set
        {
            _location = value;
            HasMoved = true;
        }
    }

    public bool HasMoved { get; set; }
    public bool IsPickedUp { get; set; }

    protected abstract List<Move> GetPseudoLegalMoves(Board board);

    public List<Move> GetLegalMoves()
    {
        Board board = Location.Board;

        List<Move> pseudoLegalMoves = GetPseudoLegalMoves(board);
        List<Move> legalMoves = new List<Move>();

        //Precompute the friendly king so we don't need to do it in every iteration of the loop
        King? friendlyKing = board.Pieces.Find(p => p is King && p.Color == Color) as King;
        if (friendlyKing is null) throw new NullReferenceException("King not found - something has gone seriously wrong.");

        foreach (Move m in pseudoLegalMoves)
        {
            m.Execute();

            //Check that the king of the moving player is not in check.
            if (friendlyKing.IsInCheck) continue;

            //If the move is fully legal, we can add it to the list to return.
            legalMoves.Add(m);
            m.Undo();
        }

        return legalMoves;
    }

    public Piece Clone(Board newBoard)
    {
        Square newLocation = newBoard.SquareCalled(Location.GetAlgebraicPosition());
        return PieceFactory.CreatePiece(Type, Color, newLocation, HasMoved);
    }

    public abstract List<Square> GetAttackedSquares(Board board);
}
