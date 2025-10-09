namespace Model;

public class Board : IObservable
{
    private List<Piece> _pieces;
    private List<IObserver> _observers;

    public void addObserver(IObserver observer)
    {

    }

    public void notifyObservers()
    {

    }
}
