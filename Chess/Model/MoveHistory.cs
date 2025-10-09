namespace Model;

public class MoveHistory : Observable
{
    private Stack<Move> _moves;

    public MoveHistory() : base()
    {
        _moves = new Stack<Move>();
    }

    public void addMove(Move move)
    {

    }

    public void undoLastMove()
    {

    }
}
