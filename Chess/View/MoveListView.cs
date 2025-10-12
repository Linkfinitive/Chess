using Chess.Model;

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
}
