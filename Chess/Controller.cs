using Chess.Model;
using Chess.Model.Engine;
using Chess.View;
using SplashKitSDK;

namespace Chess.Controller;

public class GameController
{
    private Engine _engine;

    private Board _board;
    private MoveHistory _moveHistory;

    private List<IView> _views;
    private PlayerColors _playerToMove;

    public GameController()
    {
        _engine = new Engine();

        _board = new Board();
        _moveHistory = new MoveHistory();

        _playerToMove = PlayerColors.WHITE;

        BoardView boardView = new BoardView(_board);
        MoveListView moveListView = new MoveListView(_moveHistory);
        _views = new List<IView> { boardView, moveListView };
    }

    public void DrawViews() { foreach (IView v in _views) { v.Draw(); } }

    public void HandleClick(Point2D clickLocation) { foreach (IView v in _views) { v.HandleClick(clickLocation, this); } }
    public void HandleMouseDown(Point2D mouseDownLocation) { foreach (IView v in _views) { v.HandleMouseDown(mouseDownLocation); } }
    public void HandleMouseUp(Point2D mouseUpLocation) { foreach (IView v in _views) { v.HandleMouseUp(mouseUpLocation, this); } }

    public void HandleMove(Square from, Square to, Piece pieceMoved)
    {
        pieceMoved.Location = to;
    }

}
