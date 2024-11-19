using Shapes.Decorators;

namespace Shapes.Strategies;

public sealed class SaveShapesToBinaryFileStrategy : SaveShapesStrategy
{
    public override void Save(string filename, List<ShapeDecorator> shapes)
    {
        using FileStream fileStream = new($"{filename}.dat", FileMode.Create);
        using BinaryWriter outputFile = new(fileStream);
        
        var shapesInfo = GetShapesInfo(shapes);
        shapesInfo.ForEach(s => outputFile.Write(s));
    }
}