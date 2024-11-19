using SFML.Graphics;
using Shapes.Compounds;
using Shapes.Decorators;
using Shapes.Factories;
using Shapes.Mementos;

namespace Shapes.Handlers;

public sealed class CanvasHandler : BaseShapeHandler
{
    private readonly List<ShapesMemento> _history;

    public List<ShapeDecorator> Shapes => ShapesGroup.Shapes;
    public List<ShapeDecorator> SelectedShapes => SelectedShapesGroup.Shapes;
    
    private CanvasHandler(
        ShapeDecoratorGroup shapesGroup,
        SelectedShapeDecoratorGroup selectedShapesGroup) 
        : base(shapesGroup, selectedShapesGroup)
    {
        _history = [];
    }

    public CanvasHandler()
        : this(
            ShapeDecoratorFactory.GetShapeDecoratorGroup(),
            ShapeDecoratorFactory.GetSelectedShapeDecoratorGroup())
    {
    }

    public override void Draw(RenderWindow window)
    {
        ShapesGroup.Draw(window);
        SelectedShapesGroup.Draw(window);
    }
    
    public void SelectShape(ShapeDecorator shape)
    {
        Select(shape);
    }

    public void ForceSelectShape(ShapeDecorator shape)
    {
        UnselectAll();
        Select(shape);
    }

    public void GroupShapes()
    {
        if (SelectedShapes.Count < 2)
        {
            return;
        }
        
        var group = ShapeDecoratorFactory.GetShapeDecoratorGroup();
        SelectedShapes.ForEach(s => group.Add(s));

        SelectedShapesGroup.RemoveAll();
        SelectedShapesGroup.Add(group);
    }

    public void UngroupShapes()
    {
        var groups = SelectedShapes.ToList();
        foreach (var group in groups.
                     Where(g => g.GetType() == typeof(ShapeDecoratorGroup)))
        {
            foreach (var shape in ((ShapeDecoratorGroup)group).Shapes)
            {
                SelectedShapesGroup.Add(shape);
            }
            SelectedShapesGroup.Remove(group);
        }
    }

    public void MoveShapes(int moveX, int moveY)
    {
        SelectedShapesGroup.Move(moveX, moveY);
    }

    public void SaveToHistory()
    {
        var memento = new ShapesMemento(Shapes, SelectedShapes);
        _history.Add(memento);
    }

    public void RollBackHistory()
    {
        if (_history.Count == 0)
        {
            return;
        }
        
        var memento = _history.Last();
        _history.RemoveAt(_history.Count - 1);
        
        ShapesGroup.RemoveAll();
        SelectedShapesGroup.RemoveAll();

        memento.Shapes.ForEach(ShapesGroup.Add);
        memento.SelectedShapes.ForEach(SelectedShapesGroup.Add);
    }

    public void Clear()
    {
        ShapesGroup.RemoveAll();
        SelectedShapesGroup.RemoveAll();
        _history.Clear();
    }
}