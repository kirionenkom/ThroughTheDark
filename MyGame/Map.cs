namespace MyGame;

public static class Map
{
    public const int CellSize = 50;
    public static int Width => 25;
    public static int Height => 15;
    public static ICreature?[,] MapState = ParseLevel(Levels.Level1);
    
    public static Keys KeyPressed;
    public static HashSet<Keys> PressedKeys = new();

    public static bool IsWall(int x, int y)
    {
        if (MapState[y, x] == null) return false;
        return MapState[y, x]!.GetImageName() == "wall";
    }

    public static bool IsWall(Point point)
    {
        return IsWall(point.X, point.Y);
    }

    public static bool InBounds(int x, int y)
    {
        return x > 0 && x < Width && y > 0 && y < Height;
    }

    public static bool InBounds(Point point)
    {
        return InBounds(point.X, point.Y);
    }

    private static ICreature?[,] ParseLevel(string level)
    {
        var l = level.Split('\n');
        var map = new ICreature?[Height, Width];
        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
        {
            switch (l[y][x])
            {
                case 'W':
                    map[y, x] = new Wall();
                    break;
                case 'P':
                    map[y, x] = new Player(x, y);
                    break;
                case 'E':
                    map[y, x] = new Enemy(x, y);
                    break;
                default:
                    map[y, x] = null;
                    break;
            }
        }

        return map;
    }
}