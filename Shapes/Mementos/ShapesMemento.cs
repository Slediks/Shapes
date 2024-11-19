using Shapes.Decorators;

namespace Shapes.Mementos;

public sealed class ShapesMemento
{
    public List<ShapeDecorator> Shapes { get; }
    public List<ShapeDecorator> SelectedShapes { get; }

    public ShapesMemento(
        List<ShapeDecorator> shapes,
        List<ShapeDecorator> selectedShapes)
    {
        Shapes = [];
        SelectedShapes = [];
        shapes.ForEach(s => Shapes.Add(s.Clone()));
        selectedShapes.ForEach(s => SelectedShapes.Add(s.Clone()));
    }
}