namespace MyGame;

public partial class GameWindow : Form
{
    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.TranslateTransform(0, Map.CellSize);
        foreach (var anim in Animations)
            e.Graphics.DrawImage(Images.Bitmaps[anim.Creature.GetImageName()],
                new Rectangle(anim.Location, new Size(Map.CellSize, Map.CellSize)),
                50 + anim.Command.DeltaX * Map.CellSize, 50 + anim.Command.DeltaY * Map.CellSize,
                Map.CellSize, Map.CellSize, GraphicsUnit.Pixel);
    }


    private async void TimerTick(object? sender, EventArgs args)
    {
        if (timerCount == 0) BeginAct();
        foreach (var e in Animations)
            e.Location = new Point(e.Location.X + 5 * e.Command.DeltaX, e.Location.Y + 5 * e.Command.DeltaY);
        timerCount++;
        if (timerCount == 10)
        {
            timerCount = 0;
            EndAct();
        }
        await PaintInThread();
    }

    Task PaintInThread()
    {
        var task = new Task(Invalidate);
        task.Start();
        return task;
    }

    private void BeginAct()
    {
        Animations.Clear();
        for (var y = 0; y < Map.Height; y++)
        for (var x = 0; x < Map.Width; x++)
        {
            var creature = Map.MapState[y, x];
            if (creature == null) continue;
            var command = creature.Move(x, y);
            Animations.Add(new Animation
            {
                Command = command,
                Creature = creature,
                Location = new Point(x * Map.CellSize, y * Map.CellSize),
                TargetLogicalLocation = new Point(x + command.DeltaX, y + command.DeltaY)
            });
        }

        Animations = Animations.OrderByDescending(a => a.Creature.GetDrawingPriority()).ToList();
    }

    private void EndAct()
    {
        var creaturesPerLocation = GetCandidatesPerLocation();
        for (var y = 0; y < Map.Height; y++)
        for (var x = 0; x < Map.Width; x++)
            Map.MapState[y, x] = SelectWinnerCandidatePerLocation(creaturesPerLocation, x, y);
        // foreach (var animation in Animations)
        // {
        //     var x = animation.TargetLogicalLocation.X;
        //     var y = animation.TargetLogicalLocation.Y;
        //     Map.MapState[y, x] = animation.Creature;
        // }
    }
    
    private static ICreature SelectWinnerCandidatePerLocation(List<ICreature>[,] creatures, int x, int y)
    {
        var candidates = creatures[y, x];
        var aliveCandidates = candidates.ToList();
        foreach (var candidate in candidates)
        foreach (var rival in candidates)
            if (rival != candidate && candidate.InConflict(rival))
                aliveCandidates.Remove(candidate);

        return aliveCandidates.FirstOrDefault();
    }

    private List<ICreature>[,] GetCandidatesPerLocation()
    {
        var creatures = new List<ICreature>[Map.Height, Map.Width];
        for (var y = 0; y < Map.Height; y++)
        for (var x = 0; x < Map.Width; x++)
            creatures[y, x] = new List<ICreature>();
        foreach (var e in Animations)
        {
            var x = e.TargetLogicalLocation.X;
            var y = e.TargetLogicalLocation.Y;
            creatures[y, x].Add(e.Creature);
        }

        return creatures;
    }
}