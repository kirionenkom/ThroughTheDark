namespace MyGame;

public class Projectile : ICreature
{
    public readonly Point Direction;

    public Projectile(Point direction)
    {
        Direction = direction;
    }

    public string GetImageName() => "projectile";

    public int GetDrawingPriority() => 2;

    public Command Move(int x, int y)
    {
        var nextPoint = new Point(x + Direction.X, y + Direction.Y);
        if (!Map.InBounds(nextPoint) || Map.IsWall(nextPoint)) { return new Command(); }
        return new Command(Direction.X, Direction.Y);
    }

    public bool InConflict(ICreature? otherCreature)
    {
        return otherCreature.GetImageName() == "enemy" || otherCreature.GetImageName() == "wall";
    }

}