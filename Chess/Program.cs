using Chess.Controller;
using Chess.Global;
using SplashKitSDK;

Window window = new Window("Chess", GlobalSizes.WINDOW_WIDTH, GlobalSizes.WINDOW_HEIGHT);
GameController.Instance.SetUp();

do
{
    SplashKit.ProcessEvents();

    GameController.Instance.CheckEngineTurn();

    //Here we have the great hack of 2025. I've spend 2 entire days trying to get this to work. I want the engine to be async because otherwise
    //the program stops responding while it's thinking. I've tried everything. Nothing works. Nothing stops the engine from editing the pieces on
    //the main board while it's thinking despite the fact that it is meant to be doing everything on a clone. It's infuriating, because it really
    //should not be editing the same pieces that the views iterate over, but somehow it is. This is me giving up. We don't really lose anything
    // functional, to be honest, except for the feeling that I have an understanding of the engine. Yikes. Well, we also lose the ability to see
    //the "engine is thinking" text - but it's fast enough now that it shouldn't matter. I can't spend more time on this when it's due so soon.
    if (!GameController.Instance.EngineIsThinking)
    {
        if (SplashKit.MouseDown(MouseButton.LeftButton)) GameController.Instance.HandleMouseDown(SplashKit.MousePosition());
        if (SplashKit.MouseUp(MouseButton.LeftButton)) GameController.Instance.HandleMouseUp(SplashKit.MousePosition());

        SplashKit.ClearScreen(Theme.BACKGROUND);
        GameController.Instance.DrawViews();
    }

    SplashKit.RefreshScreen();
} while (!window.CloseRequested);

SplashKit.CloseAllWindows();
