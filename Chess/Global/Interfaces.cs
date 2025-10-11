using Chess.Model;

namespace Chess;

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
