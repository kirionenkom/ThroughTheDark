using System.Configuration;
using MyGame.Types;

namespace MyGame.Entities;

public class Collectable : IEntity
{
    private readonly CollectableType _type;
    private readonly Point _logicalLocation;

    public Collectable(int x, int y, CollectableType type)
    {
        _logicalLocation = new Point(x, y);
        _type = type;
    }

    public CollectableType GetCollectableType() => _type;

    public Point GetLogicalLocation() => _logicalLocation;

    public Point GetLocation() => new (_logicalLocation.X * Map.CellSize, _logicalLocation.Y * Map.CellSize);

    public Point GetDirection() => new (-1, -1);

    public string GetImageName()
    {
        return _type switch
        {
            CollectableType.Card => "card",
            CollectableType.Heal => "heal",
            _ => ""
        };
    }
}