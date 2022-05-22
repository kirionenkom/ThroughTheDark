using MyGame.Types;
using Timer = System.Threading.Timer;
namespace MyGame.Entities;

public class Player : IEntity
{
    private int _health;
    public WeaponType WeaponType = WeaponType.Horizontal; 
    private Point _location;
    private Point _logicalLocation;
    private Point _direction;
    private Point _gazeDirection;
    private const int MovementPeriod = 30;
    public Timer Timer;
    private int _timerCount;

    public Player(int x, int y, int health)
    {
        _health = health;
        _logicalLocation = new Point(x, y);
        _location = new Point(x * Map.CellSize, y * Map.CellSize);
        _direction = new Point(0, 0);
        _gazeDirection = new Point(1, 0);
        
        var autoResetEvent = new AutoResetEvent(false); 
        Timer = new Timer(_ => TimerTick(),autoResetEvent, 300, MovementPeriod);
    }

    private void TimerTick()
    {
        if (_timerCount % 10 == 0) Move(_logicalLocation.X, _logicalLocation.Y);
        _location = new Point(_location.X + _direction.X * 8, _location.Y + _direction.Y * 8);
        _timerCount++;
        if (_timerCount != 30) return;
        Shoot();
        _timerCount = 0;
    }

    public int GetHealth() => _health;

    public Point GetLogicalLocation() => _logicalLocation;

    public Point GetLocation() => _location;

    public Point GetDirection() => _gazeDirection;
    
    public string GetImageName() => "player";

    private void Move(int x, int y)
    {
        var dx = 0;
        var dy = 0;
        switch (Map.KeyPressed)
        {
            case Keys.W:
                dy--;
                break;
            case Keys.A:
                dx--;
                break;
            case Keys.S:
                dy++;
                break;
            case Keys.D:
                dx++;
                break;
        }

        var newLocation = new Point(x + dx, y + dy);
        var nextEntity = Map.GetEntity(newLocation);
        if (nextEntity != null)
        {
            InConflict(newLocation, nextEntity);
            return;
        }

        if (!Map.InBounds(newLocation))
        {
            _direction = Point.Empty;
            return;
        }

        if (dx == 0 && dy == 0) return;
        Map.SetEntity(this, newLocation);
        _logicalLocation = new Point(x + dx, y + dy);
        _direction = new Point(dx, dy);
        _gazeDirection = _direction;
        GameWindow.Audio["move"].Play();
    }

    private void InConflict(Point newLocation, IEntity entity)
    {
        if (entity.GetType() == typeof(Collectable))
        {
            var collectable = (Collectable) entity;
            Map.KillEntity(newLocation);
            if (collectable.GetCollectableType() == CollectableType.Card)
            {
                Map.TakeCard();
                GameWindow.Audio["card"].Play();
            }

            if (collectable.GetCollectableType() == CollectableType.Heal)
            {
                _health += 25;
                GameWindow.Audio["heal"].Play();
            }
            if (_health > 100) _health = 100;
        }
        if (entity.GetType() == typeof(Wall) &&
            ((Wall)entity).GetWallType() == WallType.Door && Map.CanChangeLevel())
        {
            Timer.Dispose();
            Map.ChangeLevel();
            return;
        }
        _direction = Point.Empty;
    }

    private void Shoot()
    {
        if (Map.MouseButtonPressed == MouseButtons.Right)
            WeaponType = WeaponType == WeaponType.Horizontal ? WeaponType.Vertical : WeaponType.Horizontal;
        
        var projectilePoint = new Point(_logicalLocation.X + _gazeDirection.X, _logicalLocation.Y + _gazeDirection.Y);
        if (Map.IsEntity(projectilePoint) || Map.MouseButtonPressed != MouseButtons.Left) return;
        Map.SetEntity(new Projectile(WeaponType, projectilePoint ,_gazeDirection), projectilePoint);
        GameWindow.Audio["shoot"].Play();
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health > 0) return;
        Map.KillEntity(_logicalLocation);
        Timer.Dispose();
    }
}