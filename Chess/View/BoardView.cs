using Chess.Controller;
using Chess.Global;
using Chess.Model;
using Chess.Model.Pieces;
using SplashKitSDK;

namespace Chess.View;

public class BoardView : IView
{
    private readonly Board _board;

    public BoardView(Board board)
    {
        _board = board;
    }

    public void Draw()
    {
        DrawBoard();
        DrawPieces();
    }

    public void HandleMouseDown(Point2D mouseDownLocation)
    {
        if (GameController.Instance.PiecePickedUp is not null) return;

        foreach (Piece p in _board.Pieces)
        {
            if (SquareIsAt(p.Location, (int)mouseDownLocation.X, (int)mouseDownLocation.Y))
            {
                p.IsPickedUp = true;
            }
        }
    }

    public void HandleMouseUp(Point2D mouseUpLocation)
    {
        Piece? pickedUpPiece = GameController.Instance.PiecePickedUp;
        if (pickedUpPiece is null) return;

        foreach (Square s in _board.Squares)
        {
            if (SquareIsAt(s, (int)mouseUpLocation.X, (int)mouseUpLocation.Y))
            {
                Square newLocation = s;
                pickedUpPiece.IsPickedUp = false;

                //We'll only handle the move if the Engine isn't thinking - cause if it is then it's the engine's turn.
                if (GameController.Instance.EngineIsThinking) return;
                GameController.Instance.HandlePlayerMove(newLocation, pickedUpPiece);
            }
        }

        pickedUpPiece.IsPickedUp = false;
    }

    private void DrawBoard()
    {
        //If there is a piece picked up, then we want to colour those squares differently as an indication.
        List<Move> possibleMoves = new List<Move>();
        if (GameController.Instance.PiecePickedUp is not null)
            //Get every move that the picked-up piece can make.
        {
            possibleMoves = GameController.Instance.PiecePickedUp.GetLegalMoves();
        }

        foreach (Square s in _board.Squares)
        {
            (int xPos, int yPos) = CalculatePosition(s);

            //Set the colour appropriately for the square's colour property.
            Color color = s.Color == PlayerColors.BLACK ? Theme.BLACK_SQUARE : Theme.WHITE_SQUARE;

            //If the square is the "TO" property of any move in the possible moves, then highlight it with a different colour.
            if (possibleMoves.Any(m => m.To == s)) color = s.Color == PlayerColors.BLACK ? Theme.HIGHLIGHTED_BLACK_SQUARE : Theme.HIGHLIGHTED_WHITE_SQUARE;

            //Draw the square.
            SplashKit.FillRectangle(color, xPos, yPos, GlobalSizes.BOARD_SQUARE_SIZE, GlobalSizes.BOARD_SQUARE_SIZE);
        }
    }

    private void DrawPieces()
    {
        foreach (Piece p in _board.Pieces)
        {
            (double xPos, double yPos) = CalculatePosition(p.Location);
            xPos -= GlobalSizes.PIECE_BMP_OFFSET_MULTIPLIER * GlobalSizes.BOARD_SQUARE_SIZE;
            yPos -= GlobalSizes.PIECE_BMP_OFFSET_MULTIPLIER * GlobalSizes.BOARD_SQUARE_SIZE;

            if (p.IsPickedUp)
            {
                //If the piece is picked up, then it should follow the mouse pointer.
                (xPos, yPos) = ((int)SplashKit.MousePosition().X, (int)SplashKit.MousePosition().Y);
                xPos -= GlobalSizes.PIECE_BMP_MOUSE_POINTER_OFFSET_MULTIPLIER * GlobalSizes.BOARD_SQUARE_SIZE;
                yPos -= GlobalSizes.PIECE_BMP_MOUSE_POINTER_OFFSET_MULTIPLIER * GlobalSizes.BOARD_SQUARE_SIZE;
            }

            string color = p.Color == PlayerColors.WHITE ? "White" : "Black";

            DrawingOptions options = SplashKit.OptionDefaults();
            options.ScaleX = GlobalSizes.PIECE_BMP_SCALING_FACTOR;
            options.ScaleY = GlobalSizes.PIECE_BMP_SCALING_FACTOR;
            SplashKit.DrawBitmap(SplashKit.BitmapNamed($"{p.GetType().Name}{color}-Square"), xPos, yPos, options);
        }
    }

    private (int, int) CalculatePosition(Square s)
    {
        int xPos = s.File * GlobalSizes.BOARD_SQUARE_SIZE;
        int yPos = (7 - s.Rank) * GlobalSizes.BOARD_SQUARE_SIZE;

        xPos += GlobalSizes.BOARD_LEFT_OFFSET;
        yPos += GlobalSizes.BOARD_VERTICAL_OFFSET;
        return (xPos, yPos);
    }

    private bool SquareIsAt(Square s, int x, int y)
    {
        (int xPos, int yPos) = CalculatePosition(s);
        return xPos < x && yPos < y && xPos + GlobalSizes.BOARD_SQUARE_SIZE > x && yPos + GlobalSizes.BOARD_SQUARE_SIZE > y;
    }
}
