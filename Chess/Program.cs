using SplashKitSDK;
using Chess;
using Chess.Controller;

Window window = new Window("Chess", GlobalSizes.WINDOW_WIDTH, GlobalSizes.WINDOW_HEIGHT);
GameController controller = new GameController();

do
{
    SplashKit.ProcessEvents();
    SplashKit.ClearScreen(Theme.BACKGROUND);

    try
    {
        controller.DrawViews();
    }
    catch (NotImplementedException e)
    {
        // Console.WriteLine(e.Message);
    }

    if (SplashKit.MouseClicked(SplashKitSDK.MouseButton.LeftButton))
    {
        try
        {
            controller.HandleClick(SplashKit.MousePosition());
        }
        catch (NotImplementedException e)
        {
            // Console.WriteLine(e.Message);
        }
    }

    SplashKit.RefreshScreen();
} while (!window.CloseRequested);

SplashKit.CloseAllWindows();
