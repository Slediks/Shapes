using Shapes.Builders;
using Shapes.Decorators;
using Shapes.Types;

namespace Shapes.Strategies;

public abstract class OpenShapesStrategy
{
    protected const string Pattern = @"(?<=[=,])\d+(?=[;,])";

    protected abstract List<KeyValuePair<ShapeType, List<string>>> GetShapesInfo(string filename);

    public List<ShapeDecorator> Open(string filename)
    {
        List<ShapeDecorator> shapes = [];
        
        var shapesInfo = GetShapesInfo(filename);

        foreach (var builder in shapesInfo.Select(shapeInfo => (ShapeBuilder)(shapeInfo.Key switch
                 {
                     ShapeType.Triangle => new TriangleBuilder(shapeInfo.Value),
                     ShapeType.Rectangle => new RectangleBuilder(shapeInfo.Value),
                     ShapeType.Circle => new CircleBuilder(shapeInfo.Value),
                     _ => throw new ArgumentException($"Unknown shape type: {shapeInfo.Key}")
                 })))
        {
            builder.Build();
            
            shapes.Add(builder.GetResult());
        }
        
        return shapes;
    }
}