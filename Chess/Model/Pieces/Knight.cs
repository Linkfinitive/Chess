namespace Chess.Model;

public class Knight : Piece
{
    public Knight(PlayerColors color, Square location) : base(color, location) { }


    public override List<Move> getLegalMoves()
    {
        return new List<Move>();
    }
}
