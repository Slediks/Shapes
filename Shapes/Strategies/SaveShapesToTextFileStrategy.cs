using Shapes.Decorators;

namespace Shapes.Strategies;

public sealed class SaveShapesToTextFileStrategy : SaveShapesStrategy
{
    public override void Save(string filename, List<ShapeDecorator> shapes)
    {
        using StreamWriter outputFile = new($"{filename}.txt");
        var shapesInfo = GetShapesInfo(shapes);
        shapesInfo.ForEach(s => outputFile.WriteLine(s));
    }
}