using SFML.Graphics;
using SFML.System;
using Shapes.Decorators;

namespace Shapes.Builders;

public sealed class CircleBuilder : ShapeBuilder
{
    private readonly List<int> _parameters;
    private readonly List<uint> _visuals;
    private readonly CircleDecorator _circle;

    public CircleBuilder(List<string> parameters)
    {
        if (parameters.Count == 3) parameters.AddRange(DefaultVisualValues);
        
        if (parameters.Count != 6)
        {
            throw new ArgumentException($"Required parameter count for circle must be 6 but was {parameters.Count}");
        }

        _parameters = [];
        _visuals = [];
        
        for (var i = 0; i < 3; i++) _parameters.Add(int.Parse(parameters[i]));
        for (var i = 3; i < 6; i++) _visuals.Add(uint.Parse(parameters[i]));

        _circle = new CircleDecorator(new CircleShape());
    }
    
    public override void Build()
    {
        _circle.SetPosition(new Vector2f(
            _parameters[0] - _parameters[2],
            _parameters[1] - _parameters[2]));
        _circle.SetRadius(_parameters[2]);
        _circle.SetFillColor(new Color(_visuals[0]));
        _circle.SetOutlineColor(new Color(_visuals[1]));
        _circle.SetOutlineThickness((int)_visuals[2]);
    }

    public override CircleDecorator GetResult() => _circle;
}