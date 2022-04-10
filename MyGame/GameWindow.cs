using System.Drawing.Drawing2D;

namespace MyGame;

public partial class GameWindow : Form
{
    public readonly ImagesFiles Images = new ();
    private int playerTime = 0;
    public GameWindow()
    {
        ClientSize = new Size(Map.Width * Map.CellSize, (Map.Height + 1) * Map.CellSize);
        StartPosition = FormStartPosition.CenterScreen;
        
        var statusBar = new PictureBox
        {
            Image = Images.StatusBar,
            Size = new Size(ClientSize.Width, Map.CellSize)
        };
        Controls.Add(statusBar);
        
        Paint += (sender, args) => PaintLevel(args);
        Paint -= (sender, args) => PaintLevel(args);
        Paint += (sender, args) => PaintPlayer(args); 
    }
    
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Text = "MyGame";
        DoubleBuffered = true;
        Icon = Images.Icon;
        BackgroundImage = Images.Background;
        MaximizeBox = false;
        FormBorderStyle = FormBorderStyle.Fixed3D;
    }
    
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        var startLocation = new Point(Map.Player.X * Map.CellSize, Map.Player.Y * Map.CellSize);
        switch (e.KeyCode)
        {
            case Keys.W:
                Map.Player.Move(0, -1);
                break;
            case Keys.A:
                Map.Player.Move(-1, 0);
                break;
            case Keys.S:
                Map.Player.Move(0, 1);
                break;
            case Keys.D:
                Map.Player.Move(1, 0);
                break;
        }

        Invalidate();
    }
    
    private void PaintPlayer(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        e.Graphics.DrawImage(Images.Player,  
            new Rectangle(
                new Point(Map.Player.X * Map.CellSize,(Map.Player.Y + 1) * Map.CellSize),
                new Size(Map.CellSize, Map.CellSize)));
    }
    
    private void PaintLevel(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        for (var i = 0; i < Map.Height; i++)
        {
            for (var j = 0; j < Map.Width; j++)
            {
                if (Map.MapState[i, j] == MapCell.Wall)
                    e.Graphics.DrawImage
                    (Images.Walls, new Rectangle
                    (j * Map.CellSize,
                        (i + 1) * Map.CellSize,
                        Map.CellSize,
                        Map.CellSize));
            }
        }
    }
}