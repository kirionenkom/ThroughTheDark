using MyGame.Types;
using Timer = System.Threading.Timer;
namespace MyGame.Entities;

public class Projectile : IEntity
{
    private readonly WeaponType _type;
    private int _health = 1;
    private readonly int _damage = 25;
    private readonly Point _direction;
    private Point _logicalLocation;
    private Point _location;
    private const int MovementPeriod = 15;
    private readonly Timer _timer;
    private int _timerCount;
    

    public Projectile(WeaponType type, Point stLocation, Point stDirection)
    {
        _type = type; 
        _direction = stDirection;
        _logicalLocation = stLocation;
        _location = new Point(stLocation.X * Map.CellSize, stLocation.Y * Map.CellSize);

        var autoResetEvent = new AutoResetEvent(false); 
        _timer = new Timer(_ => TimerTick(), autoResetEvent, 0, MovementPeriod);
    }

    private void TimerTick()
    {
        if (_timerCount == 0)
        {
            if (_health == 0)
            {
                _timer.Dispose();
                Map.KillEntity(_logicalLocation);
                return;
            }
            Move(_logicalLocation.X, _logicalLocation.Y);
        }
        _location = new Point(_location.X + _direction.X * 8, _location.Y + _direction.Y * 8);
        _timerCount++;
        if (_timerCount != 10) return;
        _timerCount = 0;
        
    }

    public Point GetDirection() => _direction;

    public Point GetLogicalLocation() => _logicalLocation;

    public Point GetLocation() => _location;

    public string GetImageName()
    {
        return _type == WeaponType.Horizontal ? "horizontalProjectile" : "verticalProjectile";
    }

    private void Move(int x, int y)
    {
        var nextPoint = new Point(x + _direction.X, y + _direction.Y);
        if (!Map.SetEntityOrFalse(this, nextPoint, out var conflictedEntity))
        {
            InConflict(conflictedEntity);
            return;
        }
        _logicalLocation = nextPoint;
    }

    private void InConflict(IEntity conflictedEntity)
    {
        _health--;
        if (conflictedEntity.GetType() == typeof(Enemy))
            ((Enemy)conflictedEntity).TakeDamage(_type, _damage);
    }
}