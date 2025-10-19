using Chess.Global;

namespace Chess.Model.Pieces;

public class King : Piece
{
    public King(PlayerColors color, Square location) : base(color, location)
    {
    }

    public override List<Move> GetLegalMoves(Board board)
    {
        throw new NotImplementedException();
    }
}