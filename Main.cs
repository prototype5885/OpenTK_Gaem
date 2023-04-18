using System;


public class Start
{
    static void Main(string[] args)
    {
        using (Game game = new Game(1600, 1200, "Gaem"))
        {
            game.Run();
        }
    }
}
