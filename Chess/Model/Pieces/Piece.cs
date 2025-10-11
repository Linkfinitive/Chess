namespace Chess.Model;

public abstract class Piece
{
    public PlayerColors Color { get; }
    public Square Location { get; }

    public Piece(PlayerColors color, Square location)
    {
        Color = color;
        Location = location;
    }

    public abstract List<Move> GetLegalMoves();
}
