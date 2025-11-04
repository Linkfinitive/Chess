using Chess.Controller;
using Chess.Global;
using SplashKitSDK;

Window window = new Window("Chess", GlobalSizes.WINDOW_WIDTH, GlobalSizes.WINDOW_HEIGHT);
GameController.Instance.SetUp();

do
{
    SplashKit.ProcessEvents();
    SplashKit.ClearScreen(Theme.BACKGROUND);

    GameController.Instance.CheckEngineTurn();

    if (SplashKit.MouseDown(MouseButton.LeftButton)) GameController.Instance.HandleMouseDown(SplashKit.MousePosition());
    if (SplashKit.MouseUp(MouseButton.LeftButton)) GameController.Instance.HandleMouseUp(SplashKit.MousePosition());

    GameController.Instance.DrawViews();

    SplashKit.RefreshScreen();
} while (!window.CloseRequested);

SplashKit.CloseAllWindows();
