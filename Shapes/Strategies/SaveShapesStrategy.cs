using Shapes.Compounds;
using Shapes.Decorators;
using Shapes.Types;

namespace Shapes.Strategies;

public abstract class SaveShapesStrategy
{
    public abstract void Save(string filename, List<ShapeDecorator> shapes);

    protected static List<string> GetShapesInfo(List<ShapeDecorator> shapes)
    {
        List<string> shapesInfo = [];

        foreach (var shape in shapes)
        {
            var shapeType = shape.GetType();

            if (shapeType == typeof(TriangleDecorator))
            {
                shapesInfo.Add(GetTriangleInfo((TriangleDecorator)shape));
            }
            else if (shapeType == typeof(RectangleDecorator))
            {
                shapesInfo.Add(GetRectangleInfo((RectangleDecorator)shape));
            }
            else if (shapeType == typeof(CircleDecorator))
            {
                shapesInfo.Add(GetCircleInfo((CircleDecorator)shape));
            }
            else if (shapeType == typeof(ShapeDecoratorGroup))
            {
                shapesInfo = shapesInfo
                    .Concat(GetShapesInfo(((ShapeDecoratorGroup)shape).Shapes))
                    .ToList();
            }
        }
        
        return shapesInfo;
    }
    
    private static string GetTriangleInfo(TriangleDecorator triangle) 
    {
        var typeString = $"{ShapeType.Triangle.ToString().ToUpper()}:";

        var position = triangle.Position;
        
        var firstPoint = triangle.GetPoint(0);
        var firstPointString = $"P1={firstPoint.X + position.X},{firstPoint.Y + position.Y};";
        
        var secondPoint = triangle.GetPoint(1);
        var secondPointString = $"P2={secondPoint.X + position.X},{secondPoint.Y + position.Y};";
        
        var thirdPoint = triangle.GetPoint(2);
        var thirdPointString = $"P3={thirdPoint.X + position.X},{thirdPoint.Y + position.Y};";

        var fillColorString = $"FC={triangle.FillColor.ToInteger()};";
        
        var outlineColorString = $"OC={triangle.OutlineColor.ToInteger()};";
        
        var outlineThicknessString = $"OT={triangle.OutlineThickness};";
        
        return $"{typeString} {firstPointString} {secondPointString} {thirdPointString} {fillColorString} {outlineColorString} {outlineThicknessString}";
    }

    private static string GetRectangleInfo(RectangleDecorator rectangle)
    {
        var typeString = $"{ShapeType.Rectangle.ToString().ToUpper()}:";

        var firstPoint = rectangle.Position;
        var firstPointString = $"P1={firstPoint.X},{firstPoint.Y};";
        
        var size = rectangle.Size;
        var secondPointString = $"P2={firstPoint.X + size.X},{firstPoint.Y + size.Y};";
        
        var fillColorString = $"FC={rectangle.FillColor.ToInteger()};";
        
        var outlineColorString = $"OC={rectangle.OutlineColor.ToInteger()};";
        
        var outlineThicknessString = $"OT={rectangle.OutlineThickness};";

        return $"{typeString} {firstPointString} {secondPointString} {fillColorString} {outlineColorString} {outlineThicknessString}";
    }

    private static string GetCircleInfo(CircleDecorator circle)
    {
        var typeString = $"{ShapeType.Circle.ToString().ToUpper()}:";
        
        var radius = circle.Radius;
        var radiusString = $"R={radius};";
        
        var position = circle.Position;
        var centerString = $"C={position.X + radius},{position.Y + radius};";
        
        var fillColorString = $"FC={circle.FillColor.ToInteger()};";
        
        var outlineColorString = $"OC={circle.OutlineColor.ToInteger()};";
        
        var outlineThicknessString = $"OT={circle.OutlineThickness};";

        return $"{typeString} {centerString} {radiusString} {fillColorString} {outlineColorString} {outlineThicknessString}";
    }
}