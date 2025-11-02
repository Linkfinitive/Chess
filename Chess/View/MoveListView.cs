using Chess.Controller;
using Chess.Global;
using Chess.Model;
using SplashKitSDK;

namespace Chess.View;

public class MoveListView : IView
{
    private readonly MoveHistory _moveHistory;

    public MoveListView(MoveHistory moveHistory)
    {
        _moveHistory = moveHistory;
    }

    public void Draw()
    {
        string gameStatusText = GameController.Instance.GameStatus switch
        {
            GameStatus.WHITE_TO_MOVE => "White to Move",
            GameStatus.BLACK_TO_MOVE => "Black to Move",
            GameStatus.CHECKMATE => "Checkmate",
            GameStatus.STALEMATE => "Stalemate",
            GameStatus.DRAW => "Draw",
            _ => throw new Exception("Invalid game status")
        };

        //Title showing game status
        SplashKit.DrawText(gameStatusText, Theme.GAME_STATUS_TEXT, "arial", 24, GlobalSizes.MOVE_LIST_LEFT_OFFSET, GlobalSizes.MOVE_LIST_VERTICAL_OFFSET);

        List<Move> moves = _moveHistory.Moves.ToList();
        moves.Reverse();
        int numberOfWhiteMoves = moves.Count % 2 == 0 ? moves.Count / 2 : moves.Count / 2 + 1;

        for (int i = 0; i < numberOfWhiteMoves; i++)
        {
            int xLocation = GlobalSizes.MOVE_LIST_LEFT_OFFSET;
            int yLocation = GlobalSizes.MOVE_LIST_VERTICAL_OFFSET + (i + 2) * GlobalSizes.MOVE_LIST_INTERNAL_VERTICAL_OFFSET;

            //Start the second column of moves after 30 moves
            if (i >= 30)
            {
                xLocation += 4 * GlobalSizes.MOVE_LIST_INTERNAL_HORIZONTAL_OFFSET;
                yLocation = GlobalSizes.MOVE_LIST_VERTICAL_OFFSET + (i - 28) * GlobalSizes.MOVE_LIST_INTERNAL_VERTICAL_OFFSET;
            }

            //Or third column if over 60 moves
            if (i >= 60)
            {
                xLocation += 4 * GlobalSizes.MOVE_LIST_INTERNAL_HORIZONTAL_OFFSET;
                yLocation = GlobalSizes.MOVE_LIST_VERTICAL_OFFSET + (i - 58) * GlobalSizes.MOVE_LIST_INTERNAL_VERTICAL_OFFSET;
            }

            //Move Number
            SplashKit.DrawText($"{i + 1}. ", Theme.MOVE_LIST_TEXT, "arial", 12, xLocation, yLocation);

            xLocation += GlobalSizes.MOVE_LIST_INTERNAL_HORIZONTAL_OFFSET;

            //White Move
            SplashKit.DrawText($"{moves.ElementAt(2 * i).GetAlgebraicMove()}", Theme.MOVE_LIST_TEXT, "arial", 12, xLocation, yLocation);

            xLocation += GlobalSizes.MOVE_LIST_INTERNAL_HORIZONTAL_OFFSET;

            //Black Move - Check that it exists first
            Move? blackMove = moves.ElementAtOrDefault(2 * i + 1);
            if (blackMove is null) continue;
            SplashKit.DrawText($"{blackMove.GetAlgebraicMove()}", Theme.MOVE_LIST_TEXT, "arial", 12, xLocation, yLocation);
        }
    }

    public void HandleClick(Point2D clickLocation)
    {
        throw new NotImplementedException();
    }
}
