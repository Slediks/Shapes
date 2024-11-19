using SFML.Graphics;
using Shapes.Decorators;

namespace Shapes.Visitors;

public sealed class ShapeChangeFillColorVisitor(Color color) : ShapeVisitor
{
    public override void Visit(TriangleDecorator triangleDecorator)
    {
        triangleDecorator.SetFillColor(color);
    }

    public override void Visit(RectangleDecorator rectangleDecorator)
    {
        rectangleDecorator.SetFillColor(color);
    }

    public override void Visit(CircleDecorator circleDecorator)
    {
        circleDecorator.SetFillColor(color);
    }
}