using System.Diagnostics.CodeAnalysis;
using SplashKitSDK;

namespace Chess.Global;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Theme
{
    //These should be used in place of directly using the SplashKit offerings, so that bulk (theme) changes can be made in the future.
    public static readonly Color WHITE_SQUARE = SplashKit.ColorMintCream();
    public static readonly Color BLACK_SQUARE = SplashKit.ColorDarkSeaGreen();
    public static readonly Color HIGHLIGHTED_WHITE_SQUARE = SplashKit.ColorMistyRose();
    public static readonly Color HIGHLIGHTED_BLACK_SQUARE = SplashKit.ColorPink();
    public static readonly Color BACKGROUND = SplashKit.ColorBisque();
    public static readonly Color GAME_STATUS_TEXT = SplashKit.ColorSeaGreen();
    public static readonly Color MOVE_LIST_TEXT = SplashKit.ColorDimGray();
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class GlobalSizes
{
    //Keeps the window in the same aspect ratio (16:9), but the size can be adjusted depending on display resolution.
    public const int WINDOW_WIDTH = 1280;
    public const int WINDOW_HEIGHT = (int)(WINDOW_WIDTH * (9 / (double)16));

    //The size of the board and the squares in pixels.
    public const int BOARD_SIZE = (int)(WINDOW_WIDTH / (double)2);
    public const int BOARD_SQUARE_SIZE = (int)(BOARD_SIZE / (double)8);

    //Distance from the top and left of the window to where the board begins.
    public const int BOARD_VERTICAL_OFFSET = (int)((WINDOW_HEIGHT - BOARD_SIZE) / (double)2);
    public const int BOARD_LEFT_OFFSET = BOARD_VERTICAL_OFFSET;

    //Multipliers to offset the piece bitmaps when drawn on the board. These are magic numbers obtained through trial and error
    //to make the pieces appear centred on the squares.
    public const double PIECE_BMP_OFFSET_MULTIPLIER = 2.7; //Should be 1.64 when WINDOW_WIDTH = 1920
    public const double PIECE_BMP_MOUSE_POINTER_OFFSET_MULTIPLIER = 3.4; //Should be 2 when WINDOW_WIDTH = 1920

    public const float PIECE_BMP_SCALING_FACTOR = BOARD_SQUARE_SIZE / (float)800;

    public const int MOVE_LIST_LEFT_OFFSET = 2 * BOARD_LEFT_OFFSET + BOARD_SIZE;
    public const int MOVE_LIST_VERTICAL_OFFSET = BOARD_VERTICAL_OFFSET;
    public const int MOVE_LIST_INTERNAL_HORIZONTAL_OFFSET = BOARD_LEFT_OFFSET;
    public const int MOVE_LIST_INTERNAL_VERTICAL_OFFSET = MOVE_LIST_INTERNAL_HORIZONTAL_OFFSET / 2;
}
