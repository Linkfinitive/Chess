namespace Model;

public interface IObserver
{
    public void update();
}

public interface IObservable
{
    public void addObserver(IObserver observer);
    public void notifyObservers();
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
