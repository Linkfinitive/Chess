using System.Reflection;
using SplashKitSDK;

namespace Chess;

public static class Theme
{
    //These should be used in place of directly using the SplashKit offerings, so that bulk (theme) changes can be made in future.
    public static Color WHITE_SQUARE = SplashKit.ColorMintCream();
    public static Color BLACK_SQUARE = SplashKit.ColorDarkSeaGreen();
    public static Color BACKGROUND = SplashKit.ColorBisque();
}

public static class GlobalSizes
{
    //Keeps the window in the same aspect ratio (16:9), but the size can be adjusted depending on display resolution.
    public static int WINDOW_WIDTH = 1280;
    public static int WINDOW_HEIGHT = (int)(WINDOW_WIDTH * ((double)9 / (double)16));

    public static int BOARD_SIZE = (int)(WINDOW_WIDTH / (double)2);
    public static int BOARD_SQUARE_SIZE = (int)(BOARD_SIZE / (double)8);

    public static int BOARD_VERTICAL_OFFSET = (int)((WINDOW_HEIGHT - BOARD_SIZE) / (double)2);
    public static int BOARD_LEFT_OFFSET = BOARD_VERTICAL_OFFSET;


    //Log each field and its value to the console for debugging.
    public static void PrintFields()
    {
        FieldInfo[] fields = typeof(GlobalSizes).GetFields();

        foreach (FieldInfo field in fields)
        {
            Console.WriteLine($"{field.Name} = {field.GetValue(null)}");
        }
    }
}
