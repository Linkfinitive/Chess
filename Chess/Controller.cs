using Model;
using Model.Engine;
using View;

namespace Controller;

public class GameController
{
    private Board _board;
    private Engine _engine;
    private MoveHistory _moveHistory;
    private BoardView _boardView;
    private MoveListView _moveListView;
    private PlayerColors _playerToMove;

    public GameController()
    {
        _board = new Board();
        _engine = new Engine();
        _moveHistory = new MoveHistory();
        _boardView = new BoardView();
        _moveListView = new MoveListView();
        _playerToMove = PlayerColors.WHITE;
    }

    public void handleMove()
    {

    }

}
