using SFML.Graphics;
using SFML.System;
using Shapes.Decorators;

namespace Shapes.Builders;

public sealed class RectangleBuilder : ShapeBuilder
{
    private readonly List<int> _parameters;
    private readonly List<uint> _visuals;
    private readonly RectangleDecorator _rectangle;

    public RectangleBuilder(List<string> parameters)
    {
        if (parameters.Count == 4) parameters.AddRange(DefaultVisualValues);
        
        if (parameters.Count != 7)
        {
            throw new ArgumentException($"Required parameter count for rectangle must be 7 but was {parameters.Count}");
        }

        _parameters = [];
        _visuals = [];

        for (var i = 0; i < 4; i++) _parameters.Add(int.Parse(parameters[i]));
        for (var i = 4; i < 7; i++) _visuals.Add(uint.Parse(parameters[i]));

        _rectangle = new RectangleDecorator(new RectangleShape());
    }
    
    public override void Build()
    {
        _rectangle.SetPosition(new Vector2f(_parameters[0], _parameters[1]));
        _rectangle.SetSize(new Vector2f(
            _parameters[2] - _parameters[0],
            _parameters[3] - _parameters[1]));
        _rectangle.SetFillColor(new Color(_visuals[0]));
        _rectangle.SetOutlineColor(new Color(_visuals[1]));
        _rectangle.SetOutlineThickness((int)_visuals[2]);
    }

    public override RectangleDecorator GetResult() => _rectangle;
}