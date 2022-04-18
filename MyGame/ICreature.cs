namespace MyGame;

public interface ICreature
{
    string GetImageName();
    int GetDrawingPriority();
    Command Move(int x, int y);
    bool InConflict(ICreature? otherCreature);
}