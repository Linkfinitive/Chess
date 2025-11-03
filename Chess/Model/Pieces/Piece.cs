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
            HasMoved = true;
            _location = value;
        }
    }

    public bool HasMoved { get; set; }
    public bool IsPickedUp { get; set; }

    protected abstract List<Move> GetPseudoLegalMoves();

    public List<Move> GetLegalMoves(bool useClonedBoard = true)
    {
        //Using the cloned board is the default behaviour because it doesn't make the UI spazz out. If the board has'
        //already been cloned, however, then not cloning the board is much cheaper.
        if (useClonedBoard) return GetLegalMovesUsingClonedBoard();

        Board board = Location.Board;

        //Find the king of the moving player.
        King? friendlyKing = board.Pieces.Find(p => p is King && p.Color == Color) as King;
        if (friendlyKing is null) throw new NullReferenceException("King not found - something has gone seriously wrong.");

        List<Move> pseudoLegalMoves = GetPseudoLegalMoves();
        List<Move> legalMoves = new List<Move>();

        foreach (Move m in pseudoLegalMoves)
        {
            m.Execute(true);

            //Check that the king of the moving player is not in check.
            bool moveIsLegal = !friendlyKing.IsInCheck;

            //Have to do this before cloning the move, since we can't clone executed moves.
            m.Undo();

            //If the move is fully legal, we can add it to the list to return.
            if (moveIsLegal) legalMoves.Add(m);
        }

        return legalMoves;
    }

    private List<Move> GetLegalMovesUsingClonedBoard()
    {
        Board board = Location.Board;

        //Perform tests on a cloned board to ensure that the UI doesn't get messed up.
        Board clonedBoard = board.Clone();

        //The cloned analogue of this piece is the piece on the same square on the cloned board.
        Piece? clonedPiece = clonedBoard.PieceAt(clonedBoard.SquareCalled(Location.GetAlgebraicPosition()));
        if (clonedPiece is null) throw new NullReferenceException("Couldn't find analogue of this piece on the cloned board - something went wrong in the cloning process.");

        //Find the king of the moving player.
        King? friendlyKing = clonedBoard.Pieces.Find(p => p is King && p.Color == Color) as King;
        if (friendlyKing is null) throw new NullReferenceException("King not found - something has gone seriously wrong.");

        List<Move> pseudoLegalMoves = clonedPiece.GetPseudoLegalMoves();
        List<Move> legalMoves = new List<Move>();

        foreach (Move m in pseudoLegalMoves)
        {
            m.Execute(true);

            //Check that the king of the moving player is not in check.
            bool moveIsLegal = !friendlyKing.IsInCheck;

            //Have to do this before cloning the move, since we can't clone executed moves.
            m.Undo();

            //If the move is fully legal, we can add it to the list to return.
            //Before adding it though, we need to convert it back to the original board.
            //Avoiding Move.Clone() for this since it searches every piece and we already know which piece we are moving.
            Square to = board.SquareCalled(m.To.GetAlgebraicPosition());
            Piece? pieceCaptured = board.PieceAt(to);
            Move clonedMove = new Move(Location, to, this, pieceCaptured);
            if (moveIsLegal) legalMoves.Add(clonedMove);
        }

        return legalMoves;
    }

    public Piece Clone(Board newBoard)
    {
        Square newLocation = newBoard.SquareCalled(Location.GetAlgebraicPosition());
        return PieceFactory.CreatePiece(Type, Color, newLocation, HasMoved);
    }

    public abstract List<Square> GetAttackedSquares();
}
