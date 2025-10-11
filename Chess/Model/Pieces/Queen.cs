namespace Chess.Model;

public class Queen : Piece
{

    public Queen(PlayerColors color, Square location) : base(color, location) { }

    public override List<Move> getLegalMoves()
    {
        return new List<Move>();
    }
}
