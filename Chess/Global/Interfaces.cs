using Chess.Model;
using SplashKitSDK;

namespace Chess.Global;

public interface IView
{
    public void Draw();
    public void HandleClick(Point2D clickLocation);
    public void HandleMouseDown(Point2D mouseDownLocation);
    public void HandleMouseUp(Point2D mouseUpLocation);
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