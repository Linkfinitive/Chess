namespace Model;

public class MoveHistory : IObservable
{
    private Stack<Move> _moves;
    private List<IObserver> _observers;

    public void addObserver(IObserver observer)
    {

    }

    public void notifyObservers()
    {

    }

    public void addMove(Move move)
    {

    }

    public void undoLastMove()
    {

    }
}
