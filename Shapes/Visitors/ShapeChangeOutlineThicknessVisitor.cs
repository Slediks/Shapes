using Shapes.Decorators;

namespace Shapes.Visitors;

public sealed class ShapeChangeOutlineThicknessVisitor(int thickness) : ShapeVisitor
{
    public override void Visit(TriangleDecorator triangleDecorator)
    {
        triangleDecorator.SetOutlineThickness(thickness);
    }

    public override void Visit(RectangleDecorator rectangleDecorator)
    {
        rectangleDecorator.SetOutlineThickness(thickness);
    }

    public override void Visit(CircleDecorator circleDecorator)
    {
        circleDecorator.SetOutlineThickness(thickness);
    }
}