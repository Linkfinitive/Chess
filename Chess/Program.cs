using Chess.Controller;
using Chess.Global;
using SplashKitSDK;

Window window = new Window("Chess", GlobalSizes.WINDOW_WIDTH, GlobalSizes.WINDOW_HEIGHT);
GameController.Instance.SetUp();

do
{
    SplashKit.ProcessEvents();
    SplashKit.ClearScreen(Theme.BACKGROUND);

    GameController.Instance.DrawViews();

    if (SplashKit.MouseDown(MouseButton.LeftButton)) GameController.Instance.HandleMouseDown(SplashKit.MousePosition());

    if (SplashKit.MouseUp(MouseButton.LeftButton)) GameController.Instance.HandleMouseUp(SplashKit.MousePosition());

    //TODO: Turn this on.
    // if (SplashKit.MouseClicked(MouseButton.LeftButton)) GameController.Instance.HandleClick(SplashKit.MousePosition());

    SplashKit.RefreshScreen();
} while (!window.CloseRequested);

SplashKit.CloseAllWindows();
