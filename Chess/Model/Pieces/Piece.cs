using Chess.Global;

namespace Chess.Model.Pieces;

public abstract class Piece
{
    private Square _location;

    protected Piece(PlayerColors color, Square location, bool hasMoved, PieceType type)
    {
        //The constructor gives the option to set if the piece has moved because it matters for promoted pieces, and cloned boards.
        //In general, though, HasMoved is automatically set to true when the Location is updated.
        Color = color;
        _location = location;
        HasMoved = hasMoved;
        Type = type;
    }

    public PieceType Type { get; }

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
        King friendlyKing = Color == PlayerColors.WHITE ? board.WhiteKing : board.BlackKing;
        foreach (Move m in pseudoLegalMoves)
        {
            m.Execute(true);

            //Check that the king of the moving player is not in check.
            //If the move is fully legal, we can add it to the list to return.
            if (!friendlyKing.IsInCheck) legalMoves.Add(m);

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