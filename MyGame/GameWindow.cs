using System.Drawing.Drawing2D;
using Timer = System.Windows.Forms.Timer;

namespace MyGame;

public partial class GameWindow : Form
{
    private readonly ImagesFiles Images = new();
    private static List<Animation> Animations = new();
    private static int timerCount = 0;

    public GameWindow()
    {
        ClientSize = new Size(Map.Width * Map.CellSize, (Map.Height + 1) * Map.CellSize);
        StartPosition = FormStartPosition.CenterScreen;

        var statusBar = new PictureBox
        {
            Image = Images.Bitmaps["statusBar"],
            Size = new Size(ClientSize.Width, Map.CellSize)
        };
        Controls.Add(statusBar);

        var timer = new Timer();
        timer.Interval = 20;
        timer.Tick += TimerTick;
        timer.Start();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Text = "MyGame";
        DoubleBuffered = true;
        Icon = ImagesFiles.Icon;
        BackgroundImage = Images.Bitmaps["BackgroundImage"];
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
        Map.KeyPressed = Map.PressedKeys.Any() ? Map.PressedKeys.First() : Keys.None;
    }
}