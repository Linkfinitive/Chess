namespace Chess.Model;

public class MoveHistory
{
    public MoveHistory()
    {
        Moves = new Stack<Move>();
    }

    public Stack<Move> Moves { get; }

    public Move? MostRecentMove => Moves.Count == 0 ? null : Moves.Peek();

    public void AddMove(Move move)
    {
        Moves.Push(move);
    }

    public void UndoLastMove()
    {
        Move move = Moves.Pop();
        move.Undo();
    }
}
