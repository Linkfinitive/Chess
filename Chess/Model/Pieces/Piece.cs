using Chess.Global;

namespace Chess.Model.Pieces;

public abstract class Piece
{
    public PlayerColors Color { get; }
    public Square Location { get; set; }
    public bool IsPickedUp { get; set; }

    public Piece(PlayerColors color, Square location)
    {
        Color = color;
        Location = location;
    }

    public abstract List<Move> GetLegalMoves();
}
