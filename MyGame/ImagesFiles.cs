namespace MyGame;

public class ImagesFiles
{
    private static readonly string ImagesDirectory = 
        @"C:\Users\misha\Desktop\MyGame\MyGame\Images\";
    
    public readonly Icon Icon = new (ImagesDirectory + "icon.ico");
    public readonly Image Player = Image.FromFile(ImagesDirectory + "player.png");
    public readonly Image StatusBar = Image.FromFile(ImagesDirectory + "statusBar.png");
    public readonly Image Background = Image.FromFile(ImagesDirectory + "BackgroundImage.jpg");
    public readonly Image Walls = Image.FromFile(ImagesDirectory + "bricks.png");
}