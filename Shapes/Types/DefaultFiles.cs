using SFML.Graphics;

namespace Shapes.Types;

public static class DefaultFiles
{
    public static Font? Font { get; } = new("Roboto.ttf");
    public const string Input = "input";
    public const string Output = "output";
    public const string OpenFileName = "export";
    public const string SaveFileName = "export";
}