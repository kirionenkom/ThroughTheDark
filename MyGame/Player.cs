namespace MyGame;

public class Player : ICreature
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Player()
    {
        X = 1;
        Y = 13;
    }
    
    public string GetImageFileName() => "player.png";

    public int GetDrawingPriority() => 1;

    public void Move(int dx, int dy)
    {
        if (Map.IsWall(X + dx, Y + dy)) return;
        X += dx;
        Y += dy;
    }
    
    public bool DeadInConflict(ICreature conflictedObject)
    {
        throw new NotImplementedException();
    }
}