namespace MyGame;

public static class Map
{
    public const int CellSize = 50;
    public static int Width => 25;
    public static int Height => 15;
    
    public static MapCell[,] MapState = ParseLevel(Levels.Level1);
    public static Player Player = new();
    
    

    public static bool IsWall(int x, int y) => MapState[y, x] == MapCell.Wall;
    
    private static MapCell[,] ParseLevel(string level)
    {
        var l = level.Split('\n');
        var map = new MapCell[Height, Width];
        for (var i = 0; i < Height; i++)
        for (var j = 0; j < Width; j++)
        {
            if (l[i][j] == 'W')
                map[i, j] = MapCell.Wall;
            else
                map[i, j] = MapCell.Empty;
        }

        return map;
    }
}