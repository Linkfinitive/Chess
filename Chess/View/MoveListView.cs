using Chess.Model;
using Chess.Controller;
using SplashKitSDK;

namespace Chess.View;

public class MoveListView : IView
{
    private MoveHistory _moveHistory;

    public MoveListView(MoveHistory moveHistory)
    {
        _moveHistory = moveHistory;
    }

    public void Draw()
    {
        SplashKit.DrawText("Move List View Coming Soon", Color.BlueViolet, "arial", 12, GlobalSizes.WINDOW_WIDTH - (4 * GlobalSizes.BOARD_SQUARE_SIZE), GlobalSizes.WINDOW_HEIGHT / 2);
    }

    public void HandleClick(Point2D mouseDownLocation, GameController controller) { throw new NotImplementedException(); }

    public void HandleMouseDown(Point2D clickLocation) { } //Empty function body is correct, nothing should happen until mouse up.

    public void HandleMouseUp(Point2D mouseUpLocation, GameController controller) { HandleClick(mouseUpLocation, controller); }
}
