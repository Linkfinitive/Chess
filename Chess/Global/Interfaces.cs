using Chess.Model;

namespace Chess;

public interface IView
{
    public void Draw();
}

public interface ICommand
{
    public void Execute();
    public void Undo();
}

public interface IEvaluationStrategy
{
    public int Evaluate(Board board);
}
