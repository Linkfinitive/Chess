using Chess.Global;

namespace Chess.Model.Pieces;

public class Rook : Piece
{
    public Rook(PlayerColors color, Square location) : base(color, location) { }

    public override List<Move> GetLegalMoves()
    {
        throw new NotImplementedException();
    }
}
