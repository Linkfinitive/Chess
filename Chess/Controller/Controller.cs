using Chess.Global;
using Chess.Model;
using Chess.Model.Engine;
using Chess.Model.Pieces;
using Chess.View;
using SplashKitSDK;

namespace Chess.Controller;

public class GameController
{
    private readonly MoveHistory _moveHistory;
    private readonly List<IView> _views;
    private Engine _engine;
    private PlayerColors _playerToMove;

    private GameController()
    {
        _engine = new Engine();

        Board = new Board();
        _moveHistory = new MoveHistory();

        _playerToMove = PlayerColors.WHITE;

        BoardView boardView = new BoardView();
        MoveListView moveListView = new MoveListView(_moveHistory);
        _views = new List<IView> { boardView, moveListView };
    }

    public Board Board { get; }

    public static GameController Instance { get; } = new GameController();


    public bool PiecePickedUp
    {
        get
        {
            foreach (Piece p in Board.Pieces)
                if (p.IsPickedUp)
                    return true;

            return false;
        }
    }

    public void DrawViews()
    {
        foreach (IView v in _views) v.Draw();
    }

    public void HandleClick(Point2D clickLocation)
    {
        foreach (IView v in _views) v.HandleClick(clickLocation);
    }

    public void HandleMouseDown(Point2D mouseDownLocation)
    {
        foreach (IView v in _views) v.HandleMouseDown(mouseDownLocation);
    }

    public void HandleMouseUp(Point2D mouseUpLocation)
    {
        foreach (IView v in _views) v.HandleMouseUp(mouseUpLocation);
    }

    public void HandleMove(Square to, Piece pieceMoved)
    {
        List<Move> legalMoves = pieceMoved.GetLegalMoves(Board);
        Move? newMove = legalMoves.Find(m => m.To == to);
        if (newMove is null) return;
        _moveHistory.AddMove(newMove);
        newMove.Execute();
    }

    public void SetUp()
    {
        SplashKit.LoadFont("arial", "Arial");

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

    public void HandleCapture(Piece pieceCaptured)
    {
        Board.Pieces.Remove(pieceCaptured);
    }
}
