using SFML.Graphics;
using Shapes.Visitors;

namespace Shapes.Decorators;

public sealed class CircleDecorator(CircleShape shape) : ShapeDecorator(shape)
{
    private CircleShape Circle => (CircleShape)Shape;
    
    public int Radius => (int)Circle.Radius;
    
    public void SetRadius(int radius) => Circle.Radius = radius;
    
    public override int GetArea() => (int)(Radius * Radius * Math.PI);

    public override int GetPerimeter() => 2 * (int)(Radius * Math.PI);

    public override void Accept(ShapeVisitor visitor) => visitor.Visit(this);

    public override CircleDecorator Clone()
    {
        var newCircle = new CircleDecorator(new CircleShape());
        newCircle.SetRadius(Radius);
        newCircle.SetPosition(Circle.Position);
        newCircle.SetFillColor(Circle.FillColor);
        newCircle.SetOutlineColor(Circle.OutlineColor);
        newCircle.SetOutlineThickness((int)Circle.OutlineThickness);
        
        return newCircle;
    }
}