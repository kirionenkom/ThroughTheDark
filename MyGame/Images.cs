namespace MyGame;

public static class Images
{
    private static readonly DirectoryInfo ImagesDirectory = new (@"C:\Users\misha\Desktop\MyGame\MyGame\Images\");
    public static readonly Icon Icon = new (ImagesDirectory.FullName + "icon.ico");
    public static readonly Dictionary<string, Bitmap> Bitmaps = CreateBitmaps();

    private static Dictionary<string, Bitmap> CreateBitmaps()
    {
        var bitmaps = new Dictionary<string, Bitmap>();
        foreach (var file in ImagesDirectory.GetFiles("*.png"))
            bitmaps[file.Name[..^4]] = (Bitmap) Image.FromFile(file.FullName);
        return bitmaps;
    }
        
}