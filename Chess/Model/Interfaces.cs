namespace Model;

public interface IObserver
{
    public void update();
}

public abstract class Observable
{
    private List<IObserver> _observers;

    public Observable()
    {
        _observers = new List<IObserver>();
    }

    public void addObserver(IObserver observer)
    {
        _observers.Add(observer);
    }
    public void notifyObservers()
    {

    }
}

public interface IView
{
    public void draw();
}

public interface ICommand
{
    public void execute();
    public void undo();
}

public interface IEvaluationStrategy
{
    public int evaluate(Board board);
}
