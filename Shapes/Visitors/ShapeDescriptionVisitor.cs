using Shapes.Decorators;

namespace Shapes.Visitors;

public class ShapeDescriptionVisitor(StreamWriter outputStream) : ShapeVisitor
{
    private const string TriangleShapeType = "TRIANGLE";
    private const string RectangleShapeType = "RECTANGLE";
    private const string CircleShapeType = "CIRCLE";

    public override void Visit(TriangleDecorator triangleDecorator)
    {
        DisplayShapeDescription(TriangleShapeType, triangleDecorator.GetArea(), triangleDecorator.GetPerimeter());
    }

    public override void Visit(RectangleDecorator rectangleDecorator)
    {
        DisplayShapeDescription(RectangleShapeType, rectangleDecorator.GetArea(), rectangleDecorator.GetPerimeter());
    }

    public override void Visit(CircleDecorator circleDecorator)
    {
        DisplayShapeDescription(CircleShapeType, circleDecorator.GetArea(), circleDecorator.GetPerimeter());
    }

    private void DisplayShapeDescription(string type, int area, int perimeter)
    {
        outputStream.WriteLine($"{type}: P={perimeter}; S={area}");
    }
    
    
}