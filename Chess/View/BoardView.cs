using SplashKitSDK;
using Chess.Model;

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

    public void HandleClick(Point2D clickLocation)
    {
        foreach (Piece p in _board.Pieces)
        {
            (int xPos, int yPos) = CalculatePosition(p.Location);

            if ((xPos < clickLocation.X) && (yPos < clickLocation.Y) && ((xPos + GlobalSizes.BOARD_SQUARE_SIZE) > clickLocation.X) && ((yPos + GlobalSizes.BOARD_SQUARE_SIZE) > clickLocation.Y))
            {
                p.IsPickedUp = true;
            }

        }
    }
}
