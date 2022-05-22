namespace MyGame;

public class Levels
{
    private static readonly DirectoryInfo LevelsDirectory = new (@"C:\Users\misha\Desktop\MyGame\MyGame\Levels\");
    public readonly Dictionary<int, string[]> LevelsFiles = ImportLevels();

    private static Dictionary<int, string[]> ImportLevels()
    {
        var files = new Dictionary<int, string[]>();
        foreach (var file in LevelsDirectory.GetFiles("*.txt"))
            files[int.Parse(file.Name[..^4])] = File.ReadAllLines(file.FullName);
        return files;
    }
}