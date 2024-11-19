using SFML.Graphics;
using Shapes.Decorators;

namespace Shapes.Visitors;

public sealed class ShapeChangeOutlineColorVisitor(Color color) : ShapeVisitor
{
    public override void Visit(TriangleDecorator triangleDecorator)
    {
        triangleDecorator.SetOutlineColor(color);
    }

    public override void Visit(RectangleDecorator rectangleDecorator)
    {
        rectangleDecorator.SetOutlineColor(color);
    }

    public override void Visit(CircleDecorator circleDecorator)
    {
        circleDecorator.SetOutlineColor(color);
    }
}