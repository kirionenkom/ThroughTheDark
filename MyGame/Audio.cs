using System.Media;

namespace MyGame;

public class Audio
{
    private static readonly DirectoryInfo AudioDirectory = new (@"C:\Users\misha\Desktop\MyGame\MyGame\Audio\");
    private readonly Dictionary<string, SoundPlayer> _audios = InsertAudios();

    private static Dictionary<string, SoundPlayer> InsertAudios()
    {
        var dict = new Dictionary<string, SoundPlayer>();
        foreach (var file in AudioDirectory.GetFiles("*.wav"))
            dict[file.Name[..^4]] = new SoundPlayer(file.FullName);
        return dict;
    }
    
    public SoundPlayer this[string index] => _audios[index];
}