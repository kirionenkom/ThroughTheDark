namespace MyGame;

public interface ICreature
{
    string GetImageFileName();
    int GetDrawingPriority();
    void Move(int dx, int dy);
    bool DeadInConflict(ICreature conflictedObject);
}