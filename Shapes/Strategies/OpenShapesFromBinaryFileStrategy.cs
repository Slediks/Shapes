using System.Text.RegularExpressions;
using Shapes.Types;

namespace Shapes.Strategies;

public sealed class OpenShapesFromBinaryFileStrategy : OpenShapesStrategy
{
    protected override List<KeyValuePair<ShapeType, List<string>>> GetShapesInfo(string filename)
    {
        List<KeyValuePair<ShapeType, List<string>>> shapesInfo = [];
        
        using FileStream fileStream = new($"{filename}.dat", FileMode.Open);
        using BinaryReader inputFile = new(fileStream);

        while (inputFile.BaseStream.Position != inputFile.BaseStream.Length)
        {
            var shapeParams = inputFile.ReadString()
                .Split(": ")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
            if (shapeParams.Count == 0 || !Enum.TryParse(shapeParams[0], true, out ShapeType shapeType)) continue;
            
            shapeParams.RemoveAt(0);
            shapesInfo.Add(new KeyValuePair<ShapeType, List<string>>(shapeType, shapeParams
                .SelectMany(s => 
                    Regex.Matches(s, Pattern)
                        .Select(m => m.Value))
                .ToList()));
        }
        
        return shapesInfo;
    }
}