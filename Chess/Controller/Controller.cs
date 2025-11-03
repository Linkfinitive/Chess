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
    private readonly Engine _engine;

    private GameController()
    {
        _engine = new Engine(PlayerColors.BLACK); //TODO: Add a way to change or randomize who will be playing white.

        _board = new Board();
        MoveHistory = new MoveHistory();

        GameStatus = GameStatus.WHITE_TO_MOVE;

        _boardView = new BoardView(_board);
        _moveListView = new MoveListView(MoveHistory);

        _views = new List<IView> { _boardView, _moveListView };
    }

    public GameStatus GameStatus { get; private set; }

    private PlayerColors? PlayerToMove
    {
        get
        {
            return GameStatus switch
            {
                GameStatus.WHITE_TO_MOVE => PlayerColors.WHITE,
                GameStatus.BLACK_TO_MOVE => PlayerColors.BLACK,
                _ => null
            };
        }
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
        //Mouse clicks are only passed on if the game is still in progress.
        if (GameStatus is GameStatus.WHITE_TO_MOVE or GameStatus.BLACK_TO_MOVE) _boardView.HandleMouseDown(mouseDownLocation);
    }

    public void HandleMouseUp(Point2D mouseUpLocation)
    {
        if (GameStatus is GameStatus.WHITE_TO_MOVE or GameStatus.BLACK_TO_MOVE) _boardView.HandleMouseUp(mouseUpLocation);
    }

    public void HandleClick(Point2D clickLocation)
    {
        _moveListView.HandleClick(clickLocation);
    }

    public async Task HandleMove(Square to, Piece pieceMoved)
    {
        //Check we are moving a piece of the correct colour for this turn.
        if (pieceMoved.Color != PlayerToMove) return;

        //Get the legal moves for this piece
        List<Move> legalMoves = pieceMoved.GetLegalMoves(_board);

        //Find the specific move we are trying to make based on the mouse movement
        Move? newMove = legalMoves.Find(m => m.To == to);
        if (newMove is null) return;

        //Execute the move and add it to the history
        newMove.Execute();
        MoveHistory.AddMove(newMove);

        //Set the next player to move, or set the status to an end of game state.
        GameStatus = UpdateGameStatus(newMove);

        //If it's the engine's turn, then the engine can make its move.
        if (PlayerToMove == _engine.PlayingAs)
        {
            _moveListView.EngineIsThinking = true;
            _boardView.Locked = true;
            Move bestMove = await _engine.FindBestMove(_board, 3) ?? throw new Exception("Engine failed to find a move");
            _moveListView.EngineIsThinking = false;
            _boardView.Locked = false;
            bestMove.Execute();
            MoveHistory.AddMove(bestMove);
            GameStatus = UpdateGameStatus(bestMove);
        }
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

    private GameStatus UpdateGameStatus(Move lastMove)
    {
        if (lastMove.IsCheckmate) return GameStatus.CHECKMATE;
        if (lastMove.IsStalemate) return GameStatus.STALEMATE;

        //TODO: Add checks for draw by 50 move rule, 3fold repetition, dead position, and insufficient material. Agreement doesn't work cause it's a computer.

        //If nothing special has happened, we can just swap who's turn it is.
        return PlayerToMove == PlayerColors.WHITE ? GameStatus.BLACK_TO_MOVE : GameStatus.WHITE_TO_MOVE;
    }
}
