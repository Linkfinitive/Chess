namespace Model;

public class Pawn : Piece
{

    public Pawn(PlayerColors color, Square location) : base(color, location) { }

    public override List<Move> getLegalMoves()
    {
        return new List<Move>();
    }
}
