using SFML.System;
using SFML.Window;
using Shapes.Applications;
using Shapes.Handlers;

namespace Shapes.States;

public sealed class DragAndDropState(Application application) : IState
{
    private CanvasHandler CanvasHandler => application.CanvasHandler;

    public void Click(Vector2i point)
    {
        var activeShape = CanvasHandler.GetActiveShape(point);
        
        if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
        {
            CanvasHandler.SelectShape(activeShape);
        }
        else
        {
            if (!CanvasHandler.SelectedShapes.Contains(activeShape))
            {
                CanvasHandler.ForceSelectShape(activeShape);
            }
        }
    }
    
    public void PressedKeyG()
    {
        CanvasHandler.SaveToHistory();
        
        CanvasHandler.GroupShapes();
    }

    public void PressedKeyU()
    {
        CanvasHandler.SaveToHistory();

        CanvasHandler.UngroupShapes();
    }

    public void MouseMoved(Vector2i point)
    {
        if(!Mouse.IsButtonPressed(Mouse.Button.Left)) return;
        
        var activeShape = CanvasHandler.GetActiveShape(point);

        if (!CanvasHandler.SelectedShapes.Contains(activeShape)) return;
        
        if (!application.IsSomethingMoving)
        {
            CanvasHandler.SaveToHistory();
        }

        var moveX = point.X - application.ClickMousePosition.X;
        var moveY = point.Y - application.ClickMousePosition.Y;
        CanvasHandler.MoveShapes(moveX, moveY);
        application.SetClickMousePosition(new Vector2i(
            application.ClickMousePosition.X + moveX,
            application.ClickMousePosition.Y + moveY));
            
        application.SetIsSomethingMoving(true);
    }

    public void MouseReleased()
    {
        if (application.IsSomethingMoving)
        {
            application.SetIsSomethingMoving(false);
        }
    }
}