namespace Model;

public class Rook : Piece
{

    public Rook(PlayerColors color, Square location) : base(color, location) { }

    public override List<Move> getLegalMoves()
    {
        return new List<Move>();
    }
}
