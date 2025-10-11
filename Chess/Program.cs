using static SplashKitSDK.SplashKit;
using Chess;
using Chess.Controller;

OpenWindow("Chess", GlobalSizes.WINDOW_WIDTH, GlobalSizes.WINDOW_HEIGHT);
GameController controller = new GameController();
GlobalSizes.PrintFields();

do
{
    ProcessEvents();
    ClearScreen(Theme.BACKGROUND);

    controller.DrawViews();

    RefreshScreen();
} while (!QuitRequested());

CloseAllWindows();
