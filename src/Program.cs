namespace OpenTK_Gaem;

public static class Program
{
    public static void Main()
    {
        using Game game = new Game(1024, 768, "Gaem");
        game.Run();

    }
}