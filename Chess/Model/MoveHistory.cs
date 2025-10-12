namespace Chess.Model;

public class MoveHistory
{
    private Stack<Move> _moves;

    public MoveHistory()
    {
        _moves = new Stack<Move>();
    }

    public void AddMove(Move move)
    {
        throw new NotImplementedException();
    }

    public void UndoLastMove()
    {
        throw new NotImplementedException();
    }
}
