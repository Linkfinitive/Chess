namespace Chess.Model;

public class Queen : Piece
{

    public Queen(PlayerColors color, Square location) : base(color, location) { }

    public override List<Move> GetLegalMoves()
    {
        return new List<Move>();
    }
}
