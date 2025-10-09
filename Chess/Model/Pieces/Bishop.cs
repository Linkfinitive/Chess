namespace Model;

public class Bishop : Piece
{
    private PlayerColors _color;
    private Square _location;

    public override List<Move> getLegalMoves()
    {
        return new List<Move>();
    }
}
