using Chess.Global;
using Chess.Model;
using Chess.Model.Engine;
using Chess.Model.Pieces;
using Chess.View;
using SplashKitSDK;

namespace Chess.Controller;

public class GameController
{
    private readonly Board _board;
    private readonly BoardView _boardView;
    private readonly MoveListView _moveListView;
    private readonly List<IView> _views;
    private Engine _engine;
    private PlayerColors _playerToMove;

    private GameController()
    {
        _engine = new Engine();

        _board = new Board();
        MoveHistory = new MoveHistory();

        _playerToMove = PlayerColors.WHITE;

        _boardView = new BoardView(_board);
        _moveListView = new MoveListView(MoveHistory);

        _views = new List<IView> { _boardView, _moveListView };
    }

    public MoveHistory MoveHistory { get; }

    public static GameController Instance { get; } = new GameController();


    public Piece? PiecePickedUp
    {
        get
        {
            foreach (Piece p in _board.Pieces)
            {
                if (p.IsPickedUp)
                {
                    return p;
                }
            }

            return null;
        }
    }

    public void DrawViews()
    {
        foreach (IView v in _views) v.Draw();
    }

    public void HandleMouseDown(Point2D mouseDownLocation)
    {
        _boardView.HandleMouseDown(mouseDownLocation);
    }

    public void HandleMouseUp(Point2D mouseUpLocation)
    {
        _boardView.HandleMouseUp(mouseUpLocation);
    }

    public void HandleClick(Point2D clickLocation)
    {
        _moveListView.HandleClick(clickLocation);
    }

    public void HandleMove(Square to, Piece pieceMoved)
    {
        //Check we are moving a piece of the correct colour for this turn.
        if (pieceMoved.Color != _playerToMove) return;

        //Get the pseudolegal moves for this piece (we haven't checked for check)
        List<Move> legalMoves = pieceMoved.GetLegalMoves(_board);

        //Find the specific move we are trying to make based on the mouse movement
        Move? newMove = legalMoves.Find(m => m.To == to);
        if (newMove is null) return;

        //Execute the move and add it to the history
        newMove.Execute();
        MoveHistory.AddMove(newMove);

        //Set the next player to move.
        _playerToMove = _playerToMove == PlayerColors.WHITE ? PlayerColors.BLACK : PlayerColors.WHITE;
    }

    public void SetUp()
    {
        //Load in the fonts required for the views.
        SplashKit.LoadFont("arial", "Arial");

        //Load in the bitmaps for the pieces and convert them to square so that they display correctly.
        MakeBitmapSquare(SplashKit.LoadBitmap("BishopBlack", "Bishop-Black.bmp"));
        MakeBitmapSquare(SplashKit.LoadBitmap("KingBlack", "King-Black.bmp"));
        MakeBitmapSquare(SplashKit.LoadBitmap("KnightBlack", "Knight-Black.bmp"));
        MakeBitmapSquare(SplashKit.LoadBitmap("PawnBlack", "Pawn-Black.bmp"));
        MakeBitmapSquare(SplashKit.LoadBitmap("QueenBlack", "Queen-Black.bmp"));
        MakeBitmapSquare(SplashKit.LoadBitmap("RookBlack", "Rook-Black.bmp"));

        MakeBitmapSquare(SplashKit.LoadBitmap("BishopWhite", "Bishop-White.bmp"));
        MakeBitmapSquare(SplashKit.LoadBitmap("KingWhite", "King-White.bmp"));
        MakeBitmapSquare(SplashKit.LoadBitmap("KnightWhite", "Knight-White.bmp"));
        MakeBitmapSquare(SplashKit.LoadBitmap("PawnWhite", "Pawn-White.bmp"));
        MakeBitmapSquare(SplashKit.LoadBitmap("QueenWhite", "Queen-White.bmp"));
        MakeBitmapSquare(SplashKit.LoadBitmap("RookWhite", "Rook-White.bmp"));
    }

    private static void MakeBitmapSquare(Bitmap original)
    {
        int size = Math.Max(original.Width, original.Height);
        Bitmap square = SplashKit.CreateBitmap($"{SplashKit.BitmapName(original)}-Square", size, size);

        int x = (size - original.Width) / 2;
        int y = (size - original.Height) / 2;
        SplashKit.DrawBitmapOnBitmap(square, original, x, y);
    }
}
