namespace OpenTK_Gaem;

public static class Program
{
    public static void Main()
    {
        using Game game = new Game(1024, 1024, "Gaem");
        game.Run();
    }
}