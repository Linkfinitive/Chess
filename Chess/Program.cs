using Chess.Controller;
using Chess.Global;
using SplashKitSDK;

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
        if (SplashKit.MouseDown(MouseButton.LeftButton)) controller.HandleMouseDown(SplashKit.MousePosition());

        if (SplashKit.MouseUp(MouseButton.LeftButton)) controller.HandleMouseUp(SplashKit.MousePosition());

        if (SplashKit.MouseClicked(MouseButton.LeftButton)) controller.HandleClick(SplashKit.MousePosition());
    }
    catch (NotImplementedException)
    {
    }

    SplashKit.RefreshScreen();
} while (!window.CloseRequested);

SplashKit.CloseAllWindows();