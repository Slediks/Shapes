using SFML.Graphics;
using SFML.System;
using Shapes.Visitors;

namespace Shapes.Decorators;

public sealed class TriangleDecorator(ConvexShape shape) : ShapeDecorator(shape)
{
    private ConvexShape Triangle => (ConvexShape)Shape;
    
    public void SetPointCount(uint pointCount) => Triangle.SetPointCount(pointCount);
    
    public void SetPoint(uint index, Vector2f position) => Triangle.SetPoint(index, position);

    public override int GetArea()
    {
        var firstPointVector = Triangle.GetPoint(0);
        var secondPointVector = Triangle.GetPoint(1);
        var thirdPointVector = Triangle.GetPoint(2);
        
        return (int)(Math.Abs(
            (secondPointVector.X - firstPointVector.X) * (thirdPointVector.Y - firstPointVector.Y)
            - (thirdPointVector.X - firstPointVector.X) * (secondPointVector.Y - firstPointVector.Y))
            / 2);
    }

    public override int GetPerimeter()
    {
        var firstPointVector = Triangle.GetPoint(0);
        var secondPointVector = Triangle.GetPoint(1);
        var thirdPointVector = Triangle.GetPoint(2);
        
        return (int)(
            GetSideLength(firstPointVector, secondPointVector)
            + GetSideLength(secondPointVector, thirdPointVector)
            + GetSideLength(firstPointVector, thirdPointVector));
    }

    private static double GetSideLength(Vector2f firstPoint, Vector2f secondPoint)
    {
        return Math.Sqrt(
            Math.Pow(firstPoint.X - secondPoint.X, 2)
            + Math.Pow(firstPoint.Y - secondPoint.Y, 2));
    }

    public override void Accept(ShapeVisitor visitor) => visitor.Visit(this);

    public override TriangleDecorator Clone()
    {
        var newTriangle = new TriangleDecorator(new ConvexShape());
        newTriangle.SetPointCount(Triangle.GetPointCount());
        newTriangle.SetPoint(0, Triangle.GetPoint(0));
        newTriangle.SetPoint(1, Triangle.GetPoint(1));
        newTriangle.SetPoint(2, Triangle.GetPoint(2));
        newTriangle.SetPosition(Triangle.Position);
        newTriangle.SetFillColor(Triangle.FillColor);
        newTriangle.SetOutlineColor(Triangle.OutlineColor);
        newTriangle.SetOutlineThickness((int)Triangle.OutlineThickness);
        
        return newTriangle;
    }
}