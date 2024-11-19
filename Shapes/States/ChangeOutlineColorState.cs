using SFML.System;
using Shapes.Applications;
using Shapes.Handlers;
using Shapes.Visitors;

namespace Shapes.States;

public sealed class ChangeOutlineColorState(Application application) : IState
{
    private ToolbarHandler ToolbarHandler => application.ToolbarHandler;
    private CanvasHandler CanvasHandler => application.CanvasHandler;
    
    public void Click(Vector2i point)
    {
        CanvasHandler.SaveToHistory();
        
        var activeShape = CanvasHandler.GetActiveShape(point);
        var visitor = new ShapeChangeOutlineColorVisitor(ToolbarHandler.CurrentOutlineColor);
        activeShape.Accept(visitor);
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