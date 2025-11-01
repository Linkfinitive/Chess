using Chess.Global;
using Chess.Model;
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
        SplashKit.DrawText("Move List View Coming Soon", Color.BlueViolet, "arial", 12, GlobalSizes.WINDOW_WIDTH - 4 * GlobalSizes.BOARD_SQUARE_SIZE, GlobalSizes.WINDOW_HEIGHT / (double)2);
    }

    public void HandleClick(Point2D clickLocation)
    {
        throw new NotImplementedException();
    }
}
