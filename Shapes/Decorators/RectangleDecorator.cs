using SFML.Graphics;
using SFML.System;
using Shapes.Visitors;

namespace Shapes.Decorators;

public class RectangleDecorator(RectangleShape shape) : ShapeDecorator(shape)
{
    private RectangleShape Rectangle => (RectangleShape)Shape;
    
    public Vector2f Size => Rectangle.Size;

    public void SetSize(Vector2f size) => Rectangle.Size = size;
    
    public override int GetArea() => (int)(Size.X * Size.Y);
    
    public override int GetPerimeter() => 2 * (int)(Size.X + Size.Y);

    public override void Accept(ShapeVisitor visitor) => visitor.Visit(this);

    public override RectangleDecorator Clone()
    {
        var newRectangle = new RectangleDecorator(new RectangleShape());
        newRectangle.SetSize(Size);
        newRectangle.SetPosition(Rectangle.Position);
        newRectangle.SetFillColor(Rectangle.FillColor);
        newRectangle.SetOutlineColor(Rectangle.OutlineColor);
        newRectangle.SetOutlineThickness((int)Rectangle.OutlineThickness);
        
        return newRectangle;
    }
}