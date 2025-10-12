namespace Chess.Model;

public class Bishop : Piece
{
    public Bishop(PlayerColors color, Square location) : base(color, location) { }

    public override List<Move> GetLegalMoves()
    {
        throw new NotImplementedException();
    }
}
