namespace Chess.Model;

public class Pawn : Piece
{

    public Pawn(PlayerColors color, Square location) : base(color, location) { }

    public override List<Move> GetLegalMoves()
    {
        return new List<Move>();
    }
}
