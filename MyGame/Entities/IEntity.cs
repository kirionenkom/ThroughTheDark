namespace MyGame.Entities;

public interface IEntity
{
    Point GetLogicalLocation();
    Point GetLocation();
    Point GetDirection();
    string GetImageName();
}