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
        DrawBoard(_board.Squares);
        DrawPieces(_board.Pieces);
    }

    public void HandleClick(Point2D clickLocation, GameController controller)
    {
        throw new NotImplementedException();
    }

    public void HandleMouseDown(Point2D mouseDownLocation, GameController controller)
    {
        if (controller.PiecePickedUp) return;

        foreach (Piece p in _board.Pieces)
            if (SquareIsAt(p.Location, (int)mouseDownLocation.X, (int)mouseDownLocation.Y))
                p.IsPickedUp = true;
    }

    public void HandleMouseUp(Point2D mouseUpLocation, GameController controller)
    {
        foreach (Piece p in _board.Pieces)
            if (p.IsPickedUp)
            {
                foreach (Square s in _board.Squares)
                    if (SquareIsAt(s, (int)mouseUpLocation.X, (int)mouseUpLocation.Y))
                    {
                        Square newLocation = s;
                        p.IsPickedUp = false;
                        controller.HandleMove(newLocation, p);
                    }

                p.IsPickedUp = false;
            }
    }

    private void DrawBoard(List<Square> squares)
    {
        foreach (Square s in squares)
        {
            (int xPos, int yPos) = CalculatePosition(s);

            Color color = s.Color == PlayerColors.BLACK ? Theme.BLACK_SQUARE : Theme.WHITE_SQUARE;

            SplashKit.FillRectangle(color, xPos, yPos, GlobalSizes.BOARD_SQUARE_SIZE, GlobalSizes.BOARD_SQUARE_SIZE);
        }
    }

    private void DrawPieces(List<Piece> pieces)
    {
        foreach (Piece p in pieces)
        {
            (double xPos, double yPos) = CalculatePosition(p.Location);
            xPos -= 2.7 * GlobalSizes.BOARD_SQUARE_SIZE;
            yPos -= 2.7 * GlobalSizes.BOARD_SQUARE_SIZE;

            if (p.IsPickedUp)
            {
                //If the piece is picked up then it should follow the mouse pointer.
                (xPos, yPos) = ((int)SplashKit.MousePosition().X, (int)SplashKit.MousePosition().Y);
                xPos -= 3.2 * GlobalSizes.BOARD_SQUARE_SIZE;
                yPos -= 3.2 * GlobalSizes.BOARD_SQUARE_SIZE;
            }

            string color = p.Color == PlayerColors.WHITE ? "White" : "Black";

            DrawingOptions options = SplashKit.OptionDefaults();
            options.ScaleX = 0.1f;
            options.ScaleY = 0.1f;
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
        if (xPos < x && yPos < y && xPos + GlobalSizes.BOARD_SQUARE_SIZE > x && yPos + GlobalSizes.BOARD_SQUARE_SIZE > y) return true;

        return false;
    }
}