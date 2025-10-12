using SplashKitSDK;
using Chess.Model;
using Chess.Controller;

namespace Chess.View;

public class BoardView : IView
{
    private Board _board;

    public BoardView(Board board)
    {
        _board = board;
    }

    public void Draw()
    {

        DrawBoard(_board.Squares);
        DrawPieces(_board.Pieces);
    }

    private void DrawBoard(List<Square> squares)
    {
        foreach (Square s in squares)
        {
            (int xPos, int yPos) = CalculatePosition(s);

            Color color = (s.Color == PlayerColors.BLACK) ? Theme.BLACK_SQUARE : Theme.WHITE_SQUARE;

            SplashKit.FillRectangle(color, xPos, yPos, GlobalSizes.BOARD_SQUARE_SIZE, GlobalSizes.BOARD_SQUARE_SIZE);
        }
    }

    private void DrawPieces(List<Piece> pieces)
    {
        foreach (Piece p in pieces)
        {
            (int xPos, int yPos) = CalculatePosition(p.Location);

            if (p.IsPickedUp)
            {
                //If the piece is picked up then it should follow the mouse pointer.
                (xPos, yPos) = ((int)SplashKit.MousePosition().X, (int)(SplashKit.MousePosition().Y));
            }

            Color color = (p.Color == PlayerColors.WHITE) ? Theme.WHITE_PIECE : Theme.BLACK_PIECE;
            Font arial = SplashKit.LoadFont("arial", "Arial");
            SplashKit.DrawText($"{p.GetType().Name}", color, "arial", 12, xPos, yPos);

        }
    }

    private (int, int) CalculatePosition(Square s)
    {
        int xPos = (s.File * GlobalSizes.BOARD_SQUARE_SIZE);
        int yPos = ((7 - s.Rank) * GlobalSizes.BOARD_SQUARE_SIZE);

        xPos += GlobalSizes.BOARD_LEFT_OFFSET;
        yPos += GlobalSizes.BOARD_VERTICAL_OFFSET;
        return (xPos, yPos);
    }

    private bool SquareIsAt(Square s, int x, int y)
    {
        (int xPos, int yPos) = CalculatePosition(s);
        if ((xPos < x) && (yPos < y) && ((xPos + GlobalSizes.BOARD_SQUARE_SIZE) > x) && ((yPos + GlobalSizes.BOARD_SQUARE_SIZE) > y))
        {
            return true;
        }
        return false;
    }

    public void HandleClick(Point2D clickLocation, GameController controller) { throw new NotImplementedException(); }

    public void HandleMouseDown(Point2D mouseDownLocation)
    {
        foreach (Piece p in _board.Pieces)
        {
            if (SquareIsAt(p.Location, (int)mouseDownLocation.X, (int)mouseDownLocation.Y))
            {
                p.IsPickedUp = true;
                return;
            }

        }
    }

    public void HandleMouseUp(Point2D mouseUpLocation, GameController controller)
    {
        foreach (Piece p in _board.Pieces)
        {
            if (p.IsPickedUp)
            {
                Square newLocation;

                foreach (Square s in _board.Squares)
                {
                    (int xPos, int yPos) = CalculatePosition(s);
                    if (SquareIsAt(s, (int)mouseUpLocation.X, (int)mouseUpLocation.Y))
                    {
                        newLocation = s;
                        p.IsPickedUp = false;
                        controller.HandleMove(p.Location, newLocation, p);
                        return;
                    }
                }
            }
        }
    }
}
