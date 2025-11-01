namespace Chess.Model;

public class MoveHistory
{
    private readonly Stack<Move> _moves;

    public MoveHistory()
    {
        _moves = new Stack<Move>();
    }

    public Move? MostRecentMove => _moves.Count == 0 ? null : _moves.Peek();

    public void AddMove(Move move)
    {
        _moves.Push(move);
    }

    public void UndoLastMove()
    {
        Move move = _moves.Pop();
        move.Undo();
    }
}