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
        throw new NotImplementedException();
    }

    public void HandleClick(Point2D clickLocation)
    {
        throw new NotImplementedException();
    }
}
