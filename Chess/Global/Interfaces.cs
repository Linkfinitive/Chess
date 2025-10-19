using Chess.Controller;
using Chess.Model;
using SplashKitSDK;

namespace Chess.Global;

public interface IView
{
    public void Draw();
    public void HandleClick(Point2D clickLocation, GameController controller);
    public void HandleMouseDown(Point2D mouseDownLocation, GameController controller);
    public void HandleMouseUp(Point2D mouseUpLocation, GameController controller);
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