using SFML.Graphics;
using SFML.System;
using Shapes.Compounds;
using Shapes.Decorators;

namespace Shapes.Handlers;

public abstract class BaseShapeHandler(
    ShapeDecoratorGroup shapesGroup,
    SelectedShapeDecoratorGroup selectedShapesGroup)
{
    protected readonly ShapeDecoratorGroup ShapesGroup = shapesGroup;
    protected readonly SelectedShapeDecoratorGroup SelectedShapesGroup = selectedShapesGroup;

    public abstract void Draw(RenderWindow window);

    public void AddShape(ShapeDecorator shape)
    {
        if (ShapesGroup.Shapes.Contains(shape))
        {
            return;
        }
        
        ShapesGroup.Add(shape);
    }

    public ShapeDecorator GetActiveShape(Vector2i point)
    {
        return SelectedShapesGroup.Shapes.LastOrDefault(s => s.GetGlobalBounds().Contains(point.X, point.Y))
            ?? ShapesGroup.Shapes.LastOrDefault(s => s.GetGlobalBounds().Contains(point.X, point.Y))!;
    }

    protected void Select(ShapeDecorator shape)
    {
        if (!ShapesGroup.Shapes.Contains(shape) || SelectedShapesGroup.Shapes.Contains(shape))
        {
            if (shape == null)
            {
                UnselectAll();
            }
            else
            {
                Unselect(shape);
            }
            return;
        }
        
        ShapesGroup.Remove(shape);
        SelectedShapesGroup.Add(shape);
    }

    private void Unselect(ShapeDecorator shape)
    {
        if (ShapesGroup.Shapes.Contains(shape) || !SelectedShapesGroup.Shapes.Contains(shape))
        {
            return;
        }
        
        SelectedShapesGroup.Remove(shape);
        ShapesGroup.Add(shape);
    }

    public void UnselectAll()
    {
        var selectedShapes = SelectedShapesGroup.Shapes.ToList();
        selectedShapes.ForEach(Unselect);
    }
}