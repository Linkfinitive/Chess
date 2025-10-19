namespace Chess.Model;

public class MoveHistory
{
    private readonly Stack<Move> _moves;

    public MoveHistory()
    {
        _moves = new Stack<Move>();
    }

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