namespace MyGame;

static class Program
{
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new GameWindow());
    }
}