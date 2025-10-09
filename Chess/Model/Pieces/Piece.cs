namespace Model;

public abstract class Piece
{
    private PlayerColors _color;
    private Square _location;

    public abstract List<Move> getLegalMoves();
}
