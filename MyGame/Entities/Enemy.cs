using MyGame.Types;
using Timer = System.Threading.Timer;
namespace MyGame.Entities;

public class Enemy : IEntity
{
    private EnemyStatus _status;
    private int _health = 100;
    private int _damage = 25;
    private List<Point> _track = new();
    private Point _location;
    private Point _logicalLocation;
    private Point _direction;
    private Point _gazeDirection;
    private int _timerCount;
    private readonly Timer _timer;

    public Enemy(int x, int y)
    {
        _status = EnemyStatus.Whole;
        _logicalLocation = new Point(x, y);
        _location = new Point(x * Map.CellSize, y * Map.CellSize);
        _direction = new Point(0, 0);
        _gazeDirection = new Point(1, 0);
        var movementPeriod = 40;

        var autoResetEvent = new AutoResetEvent(false);
        _timer = new Timer(_ => TimerTick(), autoResetEvent, 0, movementPeriod);
    }

    private void TimerTick()
    {
        if (_timerCount == 0)
        {
            FindPathToPlayer(_logicalLocation);
            Move(_logicalLocation.X, _logicalLocation.Y);
            Attack();
        }
        _location = new Point(_location.X + 4 * _direction.X, _location.Y + 4 * _direction.Y);
        _timerCount++;
        if (_timerCount != 20) return;
        _timerCount = 0;
    }

    public Point GetDirection() => _gazeDirection;

    public Point GetLogicalLocation() => _logicalLocation;

    public Point GetLocation() => _location;

    public string GetImageName()
    {
        return _status switch
        {
            EnemyStatus.Whole => "wholeEnemy",
            EnemyStatus.Armless => "armlessEnemy",
            EnemyStatus.Legless => "leglessEnemy",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void Move(int x, int y)
    {
        if (!_track.Any())
        {
            _direction = new Point(0, 0);
            return;
        }

        var nextPoint = _track.First();

        if (!Map.SetEntityOrFalse(this, nextPoint, out _))
        {
            _direction = new Point(0, 0);
            return;
        }

        _track.RemoveAt(0);
        _logicalLocation = nextPoint;
        _direction = new Point(nextPoint.X - x, nextPoint.Y - y);
        _gazeDirection = _direction;
    }

    private void Attack()
    {
        var nextCell =
            Map.GetEntity(new Point(_logicalLocation.X + _gazeDirection.X, _logicalLocation.Y + _gazeDirection.Y));
        if (nextCell == null || nextCell.GetType() != typeof(Player) ) return;
        ((Player) nextCell).TakeDamage(_damage);
        GameWindow.Audio["attack"].Play();
    }

    public void TakeDamage(WeaponType weaponType, int damage)
    {
        var random = new Random();
        if (random.Next(0, 10) == 9)
        {
            _health = 0;
        }

        if (_status == EnemyStatus.Whole)
        {
            switch (weaponType)
            {
                case WeaponType.Horizontal when random.Next(0, 10) > 7:
                    _status = EnemyStatus.Legless;
                    _timer.Change(0, 55);
                    break;
                case WeaponType.Vertical when random.Next(0, 10) > 7:
                    _status = EnemyStatus.Armless;
                    _damage = 12;
                    break;
            }
        }

        _health -= damage;
        if (_health > 0) return;
        Map.KillEntity(_logicalLocation);
        _timer.Dispose();
        Map.AliveEnemies--;
        if (random.Next(0, 10) == 9)
            Map.SetEntity(new Collectable(_logicalLocation.X, _logicalLocation.Y, CollectableType.Heal), _logicalLocation);
    }

    private void FindPathToPlayer(Point startLocation)
    {
        var player = Map.FindPlayer();
        if (player == Point.Empty)
            _track = new List<Point>();
        var newTrack = new Dictionary<Point, Point> { [startLocation] = default };

        var queue = new Queue<Point>();
        queue.Enqueue(startLocation);

        while (queue.Any())
        {
            var lastPoint = queue.Dequeue();

            for (var dx = -1; dx <= 1; dx++)
            for (var dy = -1; dy <= 1; dy++)
            {
                if (Math.Abs(dx + dy) != 1) continue;
                var newPoint = new Point(lastPoint.X + dx, lastPoint.Y + dy);
                if (!Map.InBounds(newPoint)) continue;
                var entity = Map.GetEntity(newPoint);
                if ((entity != null && entity.GetType() != typeof(Player)) || newTrack.ContainsKey(newPoint)) continue;
                queue.Enqueue(newPoint);
                newTrack[newPoint] = lastPoint;
            }

            if (newTrack.ContainsKey(player)) break;
        }

        if (!newTrack.ContainsKey(player))
        {
            _track = new List<Point>();
            return;
        }

        var pathPoint = player;
        var result = new List<Point>();
        while (newTrack[pathPoint] != default)
        {
            result.Add(pathPoint);
            pathPoint = newTrack[pathPoint];
        }

        result.Reverse();
        _track = result;
    }
}