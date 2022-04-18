namespace MyGame;

public class Wall : ICreature
{
    public Point GetPosition()
    {
        return Point.Empty;
    }

    public string GetImageName() => "wall";

    public int GetDrawingPriority() => 2;

    public Command Move(int x, int y) => new ();

    public bool InConflict(ICreature? otherCreature) => false;
}