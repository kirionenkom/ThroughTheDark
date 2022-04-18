using Timer = System.Windows.Forms.Timer;

namespace MyGame;

public class Enemy : ICreature
{
    private List<Point> track = new();
    private int X;
    private int Y;

    public Enemy(int x, int y)
    {
        X = x;
        Y = y;
        var findPlayerTimer = new Timer();
        findPlayerTimer.Interval = 200;
        findPlayerTimer.Tick += (sender, args) => MakeWayInThread(GetPosition());
        findPlayerTimer.Start();
    }

    public Point GetPosition() => new (X, Y);

    public string GetImageName() => "enemy";

    public int GetDrawingPriority() => 1;

    public Command Move(int x, int y)
    {
        var nextPoint = track.FirstOrDefault();
        if (track.Any())
            track.RemoveAt(0);
        if (nextPoint == default) return new Command();
        X = nextPoint.X;
        Y = nextPoint.Y; 
        return new Command(nextPoint.X - x, nextPoint.Y - y);
    }

    public bool InConflict(ICreature? otherCreature)
    {
        return otherCreature.GetImageName() == "projectile";
    }

    public async void MakeWayInThread(Point point)
    {
        var task = new Task<List<Point>>(() => FindPathToPlayer(point));
        task.Start();
        var way = await task; 
        track = way;
    }

    List<Point> FindPathToPlayer(Point startLocation)
    {
        var player = FindPlayer();
        if (player == Point.Empty)
            return new List<Point>();
        var track = new Dictionary<Point, Point>();
        track[startLocation] = default;

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
                if (Map.IsWall(newPoint) || track.ContainsKey(newPoint)) continue;
                queue.Enqueue(newPoint);
                track[newPoint] = lastPoint;
            }

            if (track.ContainsKey(player)) break;
        }

        if (!track.ContainsKey(player)) return new List<Point>();

        var pathPoint = player;
        var result = new List<Point>();
        while (track[pathPoint] != default)
        {
            result.Add(pathPoint);
            pathPoint = track[pathPoint];
        }

        result.Reverse();
        return result;
    }

    private Point FindPlayer()
    {
        for (var y = 0; y < Map.Height; y++)
        for (var x = 0; x < Map.Width; x++)
            if (Map.MapState[y, x] != null && Map.MapState[y, x].GetImageName() == "player")
                return new Point(x, y);
        return Point.Empty;
    }
}