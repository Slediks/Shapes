using System.Text.RegularExpressions;
using Shapes.Types;

namespace Shapes.Strategies;

public sealed class OpenShapesFromTextFileStrategy : OpenShapesStrategy
{
    protected override List<KeyValuePair<ShapeType, List<string>>> GetShapesInfo(string filename)
    {
        List<KeyValuePair<ShapeType, List<string>>> shapesInfo = [];
        
        using StreamReader inputFile = new($"{filename}.txt");
        while (!inputFile.EndOfStream)
        {
            var shapeParams = inputFile.ReadLine()!
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