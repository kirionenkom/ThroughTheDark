using MyGame.Types;

namespace MyGame.Entities;

public class Wall : IEntity
{
    private readonly WallType _type;
    private readonly Point _logicalLocation;

    public Wall(int x, int y, WallType type)
    {
        _type = type;
        _logicalLocation = new Point(x, y);
    }

    public WallType GetWallType() => _type;

    public Point GetLogicalLocation() => _logicalLocation;

    public Point GetLocation() => new (_logicalLocation.X * Map.CellSize, _logicalLocation.Y * Map.CellSize);

    public Point GetDirection() => new (-1, -1);

    public string GetImageName()
    {
        return _type switch
        {
            WallType.Door => "door",
            WallType.Wall => "wall",
            WallType.Glass => "window",
            _ => ""
        };
    }
}