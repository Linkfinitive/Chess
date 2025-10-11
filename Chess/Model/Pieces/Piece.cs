namespace Chess.Model;

public abstract class Piece
{
    private PlayerColors _color;
    private Square _location;

    public Piece(PlayerColors color, Square location)
    {
        _color = color;
        _location = location;
    }

    public abstract List<Move> getLegalMoves();

    public Square getLocation()
    {
        return _location;
    }

    public PlayerColors getColor() { return _color; }
}
