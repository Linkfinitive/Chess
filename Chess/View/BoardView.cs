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

    public void draw()
    {

        drawBoard(_board.getSquares());
        drawPieces(_board.getPieces());
    }

    private void drawBoard(List<Square> squares)
    {
        foreach (Square s in squares)
        {
            (int xPos, int yPos) = calculatePosition(s);

            Color color = (s.getColor() == PlayerColors.BLACK) ? CustomColors.BLACK_SQUARE : CustomColors.WHITE_SQUARE;

            SplashKit.FillRectangle(color, xPos, yPos, GlobalSizes.BOARD_SQUARE_SIZE, GlobalSizes.BOARD_SQUARE_SIZE);
        }
    }

    private void drawPieces(List<Piece> pieces)
    {
        foreach (Piece p in pieces)
        {
            (int xPos, int yPos) = calculatePosition(p.getLocation());
            Color color = (p.getColor() == PlayerColors.WHITE) ? CustomColors.WHITE_PIECE : CustomColors.BLACK_PIECE;
            Font arial = SplashKit.LoadFont("arial", "Arial");
            SplashKit.DrawText($"{p.GetType().Name}", color, "arial", 12, xPos, yPos);

        }
    }

    private (int, int) calculatePosition(Square s)
    {
        (int rank, int file) = s.getRankAndFile();

        int xPos = (file * GlobalSizes.BOARD_SQUARE_SIZE);
        int yPos = ((7 - rank) * GlobalSizes.BOARD_SQUARE_SIZE);

        xPos += GlobalSizes.BOARD_LEFT_OFFSET;
        yPos += GlobalSizes.BOARD_VERTICAL_OFFSET;
        return (xPos, yPos);
    }
}
