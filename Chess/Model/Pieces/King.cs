namespace Model;

public class King : Piece
{

    public King(PlayerColors color, Square location) : base(color, location) { }

    public override List<Move> getLegalMoves()
    {
        return new List<Move>();
    }
}
