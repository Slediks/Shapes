using SFML.Graphics;
using SFML.System;
using Shapes.Visitors;

namespace Shapes.Decorators;

public abstract class ShapeDecorator
{
    protected readonly Shape Shape = null!;
    
    public Vector2f Position => Shape.Position;
    public Color FillColor => Shape.FillColor;
    public Color OutlineColor => Shape.OutlineColor;
    public int OutlineThickness => (int)Shape.OutlineThickness;

    protected ShapeDecorator(Shape shape)
    {
        Shape = shape;
    }

    protected ShapeDecorator()
    {
    }

    public virtual void Draw(RenderWindow window)
    {
        window.Draw(Shape);
    }
    
    public virtual void Move(int moveX, int moveY)
    {
        var currentPosition = Position;
        Shape.Position = new Vector2f(currentPosition.X + moveX, currentPosition.Y + moveY);
    }
    
    public virtual void SetFillColor(Color color)
    {
        Shape.FillColor = color;
    }

    public virtual void SetOutlineColor(Color color)
    {
        Shape.OutlineColor = color;
    }

    public virtual void SetOutlineThickness(int thickness)
    {
        Shape.OutlineThickness = thickness;
    }

    public void SetPosition(Vector2f position)
    {
        Shape.Position = position;
    }
    
    public abstract int GetArea();
    public abstract int GetPerimeter();

    public uint GetPointCount()
    {
        return Shape.GetPointCount();
    }

    public Vector2f GetPoint(uint pointIndex)
    {
        return Shape.GetPoint(pointIndex);
    }

    public virtual IntRect GetGlobalBounds()
    {
        return (IntRect)Shape.GetGlobalBounds();
    }
    
    public abstract void Accept(ShapeVisitor visitor);
    public abstract ShapeDecorator Clone();
}