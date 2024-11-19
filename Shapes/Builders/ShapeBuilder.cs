using Shapes.Decorators;
using Shapes.Types;

namespace Shapes.Builders;

public abstract class ShapeBuilder
{
    protected List<string> DefaultVisualValues =
        [DefaultColors.Blue.ToInteger().ToString(), DefaultColors.Blue.ToInteger().ToString(), "0"];
    public abstract void Build();
    public abstract ShapeDecorator GetResult();
}