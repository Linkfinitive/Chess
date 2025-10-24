using Chess.Controller;
using Chess.Global;
using Chess.Model;
using Chess.Model.Pieces;
using SplashKitSDK;

namespace Chess.View;

public class BoardView : IView
{
    public void Draw()
    {
        DrawBoard();
        DrawPieces();
    }

    public void HandleClick(Point2D clickLocation)
    {
        throw new NotImplementedException();
    }

    public void HandleMouseDown(Point2D mouseDownLocation)
    {
        if (GameController.Instance.PiecePickedUp) return;

        foreach (Piece p in GameController.Instance.Board.Pieces)
            if (SquareIsAt(p.Location, (int)mouseDownLocation.X, (int)mouseDownLocation.Y))
                p.IsPickedUp = true;
    }

    public void HandleMouseUp(Point2D mouseUpLocation)
    {
        foreach (Piece p in GameController.Instance.Board.Pieces)
            if (p.IsPickedUp)
            {
                foreach (Square s in GameController.Instance.Board.Squares)
                    if (SquareIsAt(s, (int)mouseUpLocation.X, (int)mouseUpLocation.Y))
                    {
                        Square newLocation = s;
                        p.IsPickedUp = false;
                        GameController.Instance.HandleMove(newLocation, p);
                    }
                p.IsPickedUp = false;
                break;
            }
    }

    private void DrawBoard()
    {
        foreach (Square s in GameController.Instance.Board.Squares)
        {
            (int xPos, int yPos) = CalculatePosition(s);

            Color color = s.Color == PlayerColors.BLACK ? Theme.BLACK_SQUARE : Theme.WHITE_SQUARE;

            SplashKit.FillRectangle(color, xPos, yPos, GlobalSizes.BOARD_SQUARE_SIZE, GlobalSizes.BOARD_SQUARE_SIZE);
        }
    }

    private void DrawPieces()
    {
        foreach (Piece p in GameController.Instance.Board.Pieces)
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
        return xPos < x && yPos < y && xPos + GlobalSizes.BOARD_SQUARE_SIZE > x && yPos + GlobalSizes.BOARD_SQUARE_SIZE > y;
    }
}
