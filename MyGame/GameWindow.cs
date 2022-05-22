using MyGame.Entities;
using Timer = System.Windows.Forms.Timer;

namespace MyGame;

public class GameWindow : Form
{
    private static readonly Timer Timer = new ();
    public static readonly Audio Audio = new Audio();

    public GameWindow()
    {
        ClientSize = new Size(Map.Width * Map.CellSize, (Map.Height + 1) * Map.CellSize);
        StartPosition = FormStartPosition.CenterScreen;
        Timer.Interval = 10;
        Timer.Tick += (_, _) => Invalidate();
        Timer.Start();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Text = "Through the Dark";
        DoubleBuffered = true;
        Icon = Images.Icon;
        BackgroundImage = Images.Bitmaps["backGround"];
        MaximizeBox = false;
        FormBorderStyle = FormBorderStyle.Fixed3D;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        Map.PressedKeys.Add(e.KeyCode);
        Map.KeyPressed = e.KeyCode;
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        Map.PressedKeys.Remove(e.KeyCode);
        Map.KeyPressed = Map.PressedKeys.Any() ? Map.PressedKeys.Min() : Keys.None;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Map.PressedMouseButtons.Add(e.Button);
        Map.MouseButtonPressed = e.Button;
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        Map.PressedMouseButtons.Remove(e.Button);
        Map.MouseButtonPressed = Map.PressedMouseButtons.Any() ? Map.PressedMouseButtons.Min() : MouseButtons.None;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (Map.CurrentLevel == 0)
            e.Graphics.DrawString(
                File.ReadAllText(@"C:\Users\misha\Desktop\MyGame\MyGame\Hint.txt"),
                new Font("Arial", 15), Brushes.Azure, new Point(300, 400));
        if (Map.CurrentLevel == 6)
            e.Graphics.DrawImage(Images.Bitmaps["victory"], 
                new Rectangle(new Point(675, 300), new Size(250, 360)));
        e.Graphics.DrawImage(Images.Bitmaps["statusBar"], 
            new Rectangle(new Point(0, 0), new Size(1600, 80)));
        e.Graphics.DrawString("Health", new Font("Arial", 11), Brushes.Maroon, new Point(30, 10));
        e.Graphics.DrawRectangle(Pens.Black, new Rectangle(new Point(29, 29), new Size(302, 32)));
        e.Graphics.TranslateTransform(0, Map.CellSize);
        for (var y = 0; y < Map.Height; y++)
        for (var x = 0; x < Map.Width; x++)
        {
            var creature = Map.GetEntity(new Point(x, y));
            if (creature == null) continue;
            if (creature.GetType() == typeof(Player))
            {
                e.Graphics.DrawImage(Images.Bitmaps["health"], 
                    new Rectangle(new Point(30, -50), new Size(((Player)creature).GetHealth() * 3, 30)));
                e.Graphics.DrawString("Weapon: " + ((Player)creature).WeaponType, 
                    new Font("Arial", 11), Brushes.Blue, new Point(100, -70));
            }
            var location = creature.GetLocation();
            var direction = creature.GetDirection();
            e.Graphics.DrawImage(Images.Bitmaps[creature.GetImageName()],
                new Rectangle(location, new Size(Map.CellSize, Map.CellSize)),
                80 + direction.X * Map.CellSize, 80 + direction.Y * Map.CellSize,
                Map.CellSize, Map.CellSize, GraphicsUnit.Pixel);
        }
    }
}