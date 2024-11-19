using Shapes.Decorators;

namespace Shapes.Visitors;

public abstract class ShapeVisitor
{
    public abstract void Visit(TriangleDecorator triangleDecorator);
    public abstract void Visit(RectangleDecorator rectangleDecorator);
    public abstract void Visit(CircleDecorator circleDecorator);
}