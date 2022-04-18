namespace MyGame;

public class ImagesFiles
{
    private static readonly string ImagesDirectory = 
        @"C:\Users\misha\Desktop\MyGame\MyGame\Images\";

    public static readonly Icon Icon = new (ImagesDirectory + "icon.ico");

    public readonly Dictionary<string, Bitmap> Bitmaps = CreateBitmaps();

    private static Dictionary<string, Bitmap> CreateBitmaps()
    {
        var dict = new Dictionary<string, Bitmap>();
        dict.Add("player", (Bitmap)Image.FromFile(ImagesDirectory + "player.png"));
        dict.Add("statusBar", (Bitmap)Image.FromFile(ImagesDirectory + "statusBar.png"));
        dict.Add("BackgroundImage", (Bitmap)Image.FromFile(ImagesDirectory + "BackgroundImage.jpg"));
        dict.Add("wall", (Bitmap)Image.FromFile(ImagesDirectory + "wall.png"));
        dict.Add("enemy", (Bitmap)Image.FromFile(ImagesDirectory + "enemy.png"));
        dict.Add("projectile", (Bitmap)Image.FromFile(ImagesDirectory + "projectile.png"));
        return dict;
    }
}