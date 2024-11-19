using SFML.Graphics;
using SFML.System;
using Shapes.Decorators;
using Shapes.Visitors;

namespace Shapes.Compounds;

public class ShapeDecoratorGroup : ShapeDecorator
{
    public List<ShapeDecorator> Shapes { get; } = [];

    public void Add(ShapeDecorator shape)
    {
        if (Shapes.Contains(shape))
        {
            return;
        }
        
        Shapes.Add(shape);
    }

    public void Remove(ShapeDecorator shape)
    {
        Shapes.Remove(shape);
    }

    public void RemoveAll()
    {
        Shapes.Clear();
    }

    public override void Draw(RenderWindow window)
    {
        Shapes.ForEach(s => s.Draw(window));
    }

    public override void Move(int moveX, int moveY)
    {
        Shapes.ForEach(s => s.Move(moveX, moveY));
    }
    
    public override void SetFillColor(Color color)
    {
        Shapes.ForEach(s => s.SetFillColor(color));
    }

    public override void SetOutlineColor(Color color)
    {
        Shapes.ForEach(s => s.SetOutlineColor(color));
    }

    public override void SetOutlineThickness(int thickness)
    {
        Shapes.ForEach(s => s.SetOutlineThickness(thickness));
    }

    public override int GetArea()
    {
        var area = 0;
        Shapes.ForEach(s => area += s.GetArea());
        return area;
    }

    public override int GetPerimeter()
    {
        var perimeter = 0;
        Shapes.ForEach(s => perimeter += s.GetPerimeter());
        return perimeter;
    }

    public override IntRect GetGlobalBounds()
    {
        var currentBounds = Shapes[0].GetGlobalBounds();
        var leftTopPoint = new Vector2i(currentBounds.Left, currentBounds.Top);
        var rightBottomPoint = new Vector2i(
            currentBounds.Left + currentBounds.Width,
            currentBounds.Top + currentBounds.Height);

        foreach (var currentFrame in Shapes.Select(s => s.GetGlobalBounds()))
        {
            if (currentFrame.Left < leftTopPoint.X)
            {
                leftTopPoint.X = currentFrame.Left;
            }

            if (currentFrame.Top < leftTopPoint.Y)
            {
                leftTopPoint.Y = currentFrame.Top;
            }

            if (currentFrame.Left + currentFrame.Width > rightBottomPoint.X)
            {
                rightBottomPoint.X = currentFrame.Left + currentFrame.Width;
            }

            if (currentFrame.Top + currentFrame.Height > rightBottomPoint.Y)
            {
                rightBottomPoint.Y = currentFrame.Top + currentFrame.Height;
            }
        }
        
        return new IntRect(
            leftTopPoint.X,
            leftTopPoint.Y,
            rightBottomPoint.X - leftTopPoint.X,
            rightBottomPoint.Y - leftTopPoint.Y);
    }

    public override void Accept(ShapeVisitor visitor)
    {
        Shapes.ForEach(s => s.Accept(visitor));
    }

    public override ShapeDecorator Clone()
    {
        var newGroup = new ShapeDecoratorGroup();
        Shapes.ForEach(s => newGroup.Add(s.Clone()));

        return newGroup;
    }
}