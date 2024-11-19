using SFML.System;
using Shapes.Applications;
using Shapes.Decorators;
using Shapes.Factories;
using Shapes.Handlers;
using Shapes.Types;

namespace Shapes.States;

public sealed class AddShapeState(Application application) : IState
{
    private ToolbarHandler ToolbarHandler => application.ToolbarHandler;
    private CanvasHandler CanvasHandler => application.CanvasHandler;
    
    public void Click(Vector2i point)
    {
        CanvasHandler.SaveToHistory();
        
        var shapeType = ToolbarHandler.CurrentAddShapeType;
        ShapeDecorator shape = shapeType switch
        {
            ShapeType.Triangle => ShapeDecoratorFactory.GetTriangleDecorator(),
            ShapeType.Rectangle => ShapeDecoratorFactory.GetRectangleDecorator(),
            ShapeType.Circle => ShapeDecoratorFactory.GetCircleDecorator(),
            _ => throw new ArgumentOutOfRangeException($"Unknown shape type: {shapeType}")
        };
        
        // ReSharper disable PossibleLossOfFraction
        shape.Move((int)(point.X - shape.Position.X - shape.GetGlobalBounds().Width / 2),
            (int)(point.Y - shape.Position.Y - shape.GetGlobalBounds().Height / 2));
        // ReSharper restore PossibleLossOfFraction
        shape.SetFillColor(ToolbarHandler.CurrentFillColor);
        shape.SetOutlineColor(ToolbarHandler.CurrentOutlineColor);
        shape.SetOutlineThickness(ToolbarHandler.CurrentOutlineThickness);
        
        CanvasHandler.AddShape(shape);
    }

    public void PressedKeyG()
    {
    }

    public void PressedKeyU()
    {
    }

    public void MouseMoved(Vector2i point)
    {
    }

    public void MouseReleased()
    {
    }
}