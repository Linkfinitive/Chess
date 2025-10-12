namespace Chess.Model;

public class Move : ICommand
{
    private Square _from;
    private Square _to;
    private Piece _pieceMoved;
    private Piece? _pieceCaptured;
    private Boolean _promotion;

    public Move(Square from, Square to, Piece pieceMoved, Piece? pieceCaptured, Boolean promotion)
    {
        _from = from;
        _to = to;
        _pieceMoved = pieceMoved;
        _pieceCaptured = pieceCaptured;
        _promotion = promotion;
    }

    public Move(Square from, Square to, Piece pieceMoved, Piece? pieceCaptured) : this(from, to, pieceMoved, pieceCaptured, false) { }
    public Move(Square from, Square to, Piece pieceMoved, Boolean promotion) : this(from, to, pieceMoved, null, promotion) { }
    public Move(Square from, Square to, Piece pieceMoved) : this(from, to, pieceMoved, null, false) { }

    public string GetAlgebraicMove()
    {
        throw new NotImplementedException();
    }

    public void Execute()
    {
        throw new NotImplementedException();
    }

    public void Undo()
    {
        throw new NotImplementedException();
    }
}
