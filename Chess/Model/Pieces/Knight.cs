namespace Chess.Model;

public class Knight : Piece
{
    public Knight(PlayerColors color, Square location) : base(color, location) { }

    public override List<Move> GetLegalMoves()
    {
        throw new NotImplementedException();
    }
}
