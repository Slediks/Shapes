using SFML.Graphics;
using SFML.System;
using Shapes.Decorators;

namespace Shapes.Compounds;

public sealed class SelectedShapeDecoratorGroup : ShapeDecoratorGroup
{
    public override void Draw(RenderWindow window)
    {
        if (Shapes.Count == 0)
        {
            return;
        }

        var bounds = GetGlobalBounds();
        var frame = new RectangleDecorator(new RectangleShape());

        frame.SetPosition(new Vector2f(bounds.Left, bounds.Top));
        frame.SetSize(new Vector2f(bounds.Width, bounds.Height));
        frame.SetFillColor(new Color(0, 0, 0, 0));
        frame.SetOutlineColor(Color.White);
        frame.SetOutlineThickness(1);
        
        base.Draw(window);
        frame.Draw(window);
    }
}