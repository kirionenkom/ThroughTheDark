namespace MyGame;

public class Command
{
    public int DeltaX;
    public int DeltaY;

    public Command()
    {
        DeltaX = 0;
        DeltaY = 0;
    }

    public Command(int deltaX, int deltaY)
    {
        DeltaX = deltaX;
        DeltaY = deltaY;
    }
}