using MyGame.Entities;
using MyGame.Types;

namespace MyGame;

public static class Map
{
    private static readonly Levels Levels = new ();
    public const int CellSize = 80;
    public static int Width => 20;
    public static int Height => 11;

    private static int _lastPlayerHealth = 100;
    public static int CurrentLevel { get; private set; } = 6;
    public static int AliveEnemies;
    private static bool _cardIsTaken;
    private static IEntity[,] _mapState = ParseLevel(Levels.LevelsFiles[CurrentLevel]);

    public static Keys KeyPressed;
    public static HashSet<Keys> PressedKeys = new();
    public static MouseButtons MouseButtonPressed;
    public static HashSet<MouseButtons> PressedMouseButtons = new();

    public static bool IsEntity(Point point)
    {
        lock (_mapState) return _mapState[point.Y, point.X] != null;
    }

    public static bool InBounds(Point point)
    {
        lock (_mapState) return point.X > 0 && point.X < Width && point.Y > 0 && point.Y < Height;
    }

    public static void KillEntity(Point point)
    {
        lock (_mapState) _mapState[point.Y, point.X] = null;
    }

    public static void SetEntity(IEntity entity, Point point)
    {
        var entityLocation = entity.GetLogicalLocation();
        lock (_mapState)
        {
            KillEntity(entityLocation);
            _mapState[point.Y, point.X] = entity;
        }
    }

    public static IEntity GetEntity(Point point)
    {
        lock (_mapState) return _mapState[point.Y, point.X];
    }

    public static bool SetEntityOrFalse(IEntity entity, Point point, out IEntity conflictedEntity)
    {
        lock (_mapState)
        {
            conflictedEntity = GetEntity(point);  
            if (conflictedEntity != null) return false;
            SetEntity(entity, point);
            return true;
        }
    }

    public static bool CanChangeLevel()
    {
        return AliveEnemies == 0 && _cardIsTaken;
    }

    public static void ChangeLevel()
    {
        CurrentLevel++;
        if (CurrentLevel == 1)
            _cardIsTaken = false;
        var playerPoint = FindPlayer();
        _lastPlayerHealth = ((Player) _mapState[playerPoint.Y, playerPoint.X]).GetHealth();
        _mapState = ParseLevel(Levels.LevelsFiles[CurrentLevel]);
    }

    public static void TakeCard()
    {
        _cardIsTaken = true;
    }

    private static IEntity[,] ParseLevel(string[] level)
    {
        var map = new IEntity[Height, Width];
        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
        {
            switch (level[y][x])
            {
                case 'W':
                    map[y, x] = new Wall(x, y, WallType.Wall);
                    break;
                case 'D':
                    map[y, x] = new Wall(x, y, WallType.Door);
                    break;
                case 'G':
                    map[y, x] = new Wall(x, y, WallType.Glass);
                    break;
                case 'C':
                    map[y, x] = new Collectable(x, y, CollectableType.Card);
                    break;
                case 'H':
                    map[y, x] = new Collectable(x, y, CollectableType.Heal);
                    break;
                case 'P':
                    map[y, x] = new Player(x, y, _lastPlayerHealth);
                    break;
                case 'E':
                    map[y, x] = new Enemy(x, y);
                    AliveEnemies++;
                    break;
                default:
                    map[y, x] = null;
                    break;
            }
        }
        return map;
    }
    
    public static Point FindPlayer()
    {
        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
            if (_mapState[y, x]?.GetType() == typeof(Player))
                return new Point(x, y);
        return Point.Empty;
    }
}