using OpenTK.Windowing.Desktop;

namespace OpenTK_Gaem;

public static class Program
{

    public static void Main()
    {
        GameWindowSettings settings = GameWindowSettings.Default;
        //settings.UpdateFrequency = 60;
        using Game game = new Game(1920, 1920, "Gaem");
        game.VSync = OpenTK.Windowing.Common.VSyncMode.On;
        game.Run();
    }
}