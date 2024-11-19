using SFML.Graphics;
using SFML.System;
using Shapes.Compounds;
using Shapes.Decorators;
using Shapes.Handlers;
using Shapes.Types;

namespace Shapes.Factories;

public static class ShapeDecoratorFactory
{
    public static TriangleDecorator GetTriangleDecorator(List<string> parameters)
    {
        if (parameters.Count != 6) // pos1X, pos1Y, pos2X, pos2Y, pos3X, pos3Y
        {
            throw new ArgumentException("The number of parameters for triangle must be 6");
        }

        return GetTriangle(GetCoordinates(parameters));
    }

    public static TriangleDecorator GetTriangleDecorator()
    {
        return GetTriangle([
            Tuple.Create(50, 0),
            Tuple.Create(0, 100),
            Tuple.Create(100, 100)
        ]);
    }

    public static RectangleDecorator GetRectangleDecorator(List<string> parameters)
    {
        if (parameters.Count != 4) // pos1X, pos1Y, pos2X, pos2Y
        {
            throw new ArgumentException("The number of parameters for rectangle must be 4");
        }
        
        return GetRectangle(GetCoordinates(parameters));
    }

    public static RectangleDecorator GetRectangleDecorator()
    {
        return GetRectangle([
            Tuple.Create(0, 0),
            Tuple.Create(100, 100)
        ]);
    }

    public static CircleDecorator GetCircleDecorator(List<string> parameters)
    {
        if (parameters.Count != 3) // pos1X, pos2Y, radius
        {
            throw new ArgumentException("The number of parameters for circle must be 3");
        }
        
        var radius = int.Parse(parameters[2]);
        return GetCircle(GetCoordinates(parameters).First(), radius);
    }

    public static CircleDecorator GetCircleDecorator()
    {
        return GetCircle(Tuple.Create(50, 50), 50);
    }

    public static ShapeDecoratorGroup GetShapeDecoratorGroup()
    {
        return new ShapeDecoratorGroup();
    }

    public static SelectedShapeDecoratorGroup GetSelectedShapeDecoratorGroup()
    {
        return new SelectedShapeDecoratorGroup();
    }

    private static TriangleDecorator GetTriangle(List<Tuple<int, int>> coordinates)
    {
        var triangle = new TriangleDecorator(new ConvexShape());
        
        triangle.SetPosition(new Vector2f(50, 50));
        foreach (var tuple in coordinates)
        {
            var pointCount = triangle.GetPointCount();
            triangle.SetPointCount(pointCount + 1);
            triangle.SetPoint(pointCount, new Vector2f(tuple.Item1, tuple.Item2));
        }

        return triangle;
    }

    private static RectangleDecorator GetRectangle(List<Tuple<int, int>> coordinates)
    {
        var rectangle = new RectangleDecorator(new RectangleShape());
        
        // ReSharper disable PossibleLossOfFraction
        rectangle.SetPosition(new Vector2f(
            (coordinates[0].Item1 + coordinates[1].Item1) / 2,
            (coordinates[0].Item2 + coordinates[1].Item2) / 2));
        // ReSharper restore PossibleLossOfFraction
        rectangle.SetSize(new Vector2f(
            coordinates[1].Item1 - coordinates[0].Item1,
            coordinates[1].Item2 - coordinates[0].Item2));

        return rectangle;
    }

    private static CircleDecorator GetCircle(Tuple<int, int> coordinates, int radius)
    {
        var circle = new CircleDecorator(new CircleShape());
        
        circle.SetRadius(radius);
        circle.SetPosition(new Vector2f(coordinates.Item1, coordinates.Item2));
        
        return circle;
    }

    private static List<Tuple<int, int>> GetCoordinates(List<string> parameters)
    {
        if (parameters.Count % 2 != 0)
        {
            parameters.Remove(parameters.Last());
        }
        
        return parameters
            .Where((_, i) => i % 2 == 0)
            .Zip(parameters.Where((_, i) => i % 2 != 0),
                (a, b) => Tuple.Create(int.Parse(a), int.Parse(b)))
            .ToList();
    }
}