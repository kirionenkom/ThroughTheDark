using Microsoft.VisualBasic.Devices;
using Timer = System.Windows.Forms.Timer;

namespace MyGame;

public class Player : ICreature
{
    private  int X;
    private int Y;
    public Point GazeDirection { get; private set; }

    public Player(int x, int y)
    {
        X = x;
        Y = y;
        GazeDirection = new Point(1, 0);
        var playerTimer = new Timer();
        playerTimer.Interval = 10;
        playerTimer.Tick += (sender, args) => Shoot(X, Y);
        playerTimer.Start();
    }

    public Point GetPosition() => new(X, Y);
    
    public string GetImageName() => "player";

    public int GetDrawingPriority() => 1;

    public Command Move(int x, int y)
    {
        var dx = 0;
        var dy = 0;
        switch (Map.KeyPressed)
        {
            case Keys.W:
                dy--;
                GazeDirection = new Point(0, -1);
                Y--;
                break;
            case Keys.A:
                dx--;
                X--;
                GazeDirection = new Point(-1, 0);
                break;
            case Keys.S:
                dy++;
                Y++;
                GazeDirection = new Point(0, 1);
                break;
            case Keys.D:
                dx++;
                X++;
                GazeDirection = new Point(1, 0);
                break;
        }
        
        return Map.IsWall(x + dx, y + dy) ? new Command() 
            : new Command(dx, dy);
    }

    public bool InConflict(ICreature? otherCreature) => false;

    public void Shoot(int x, int y)
    {
        if (Map.KeyPressed == Keys.Space)
            Map.MapState[y + GazeDirection.Y, x + GazeDirection.X] = new Projectile(GazeDirection);
    }
}