namespace Chess.Model;

public class King : Piece
{

    public King(PlayerColors color, Square location) : base(color, location) { }

    public override List<Move> GetLegalMoves()
    {
        return new List<Move>();
    }
}
