using SplashKitSDK;
using Chess;
using Chess.Controller;

Window window = new Window("Chess", GlobalSizes.WINDOW_WIDTH, GlobalSizes.WINDOW_HEIGHT);
GameController controller = new GameController();
controller.SetUp();

do
{
    SplashKit.ProcessEvents();
    SplashKit.ClearScreen(Theme.BACKGROUND);

    controller.DrawViews();
    try
    {
        if (SplashKit.MouseDown(SplashKitSDK.MouseButton.LeftButton))
        {
            controller.HandleMouseDown(SplashKit.MousePosition());
        }

        if (SplashKit.MouseUp(SplashKitSDK.MouseButton.LeftButton))
        {
            controller.HandleMouseUp(SplashKit.MousePosition());
        }

        if (SplashKit.MouseClicked(SplashKitSDK.MouseButton.LeftButton))
        {
            controller.HandleClick(SplashKit.MousePosition());
        }
    }
    catch (NotImplementedException) { }
    SplashKit.RefreshScreen();
} while (!window.CloseRequested);

SplashKit.CloseAllWindows();
