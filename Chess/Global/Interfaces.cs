using Chess.Model;
using SplashKitSDK;

namespace Chess;

public interface IView
{
    public void Draw();
    public void HandleClick(Point2D clickLocation);
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
