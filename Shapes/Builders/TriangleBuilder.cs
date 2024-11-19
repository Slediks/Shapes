using SFML.Graphics;
using SFML.System;
using Shapes.Decorators;

namespace Shapes.Builders;

public sealed class TriangleBuilder : ShapeBuilder
{
    private readonly List<int> _parameters;
    private readonly List<uint> _visuals;
    private readonly Vector2f _position;
    private readonly TriangleDecorator _triangle;

    public TriangleBuilder(List<string> parameters)
    {
        if (parameters.Count == 6) parameters.AddRange(DefaultVisualValues);
        
        if (parameters.Count != 9)
        {
            throw new ArgumentException($"Required parameter count for triangle must be 11 but was {parameters.Count}");
        }

        _parameters = [];
        _visuals = [];
        
        for (var i = 0; i < 6; i++) _parameters.Add(int.Parse(parameters[i])); // 0-5
        for (var i = 6; i < 9; i++) _visuals.Add(uint.Parse(parameters[i])); // 6-8

        _position = new Vector2f(
            Math.Min(_parameters[0], Math.Min(_parameters[2], _parameters[4])),
            Math.Min(_parameters[1], Math.Min(_parameters[3], _parameters[5])));

        for (var i = 0; i < 3; i++)
        {
            _parameters[i * 2] -= (int)_position.X;
            _parameters[i * 2 + 1] -= (int)_position.Y;
        }

        _triangle = new TriangleDecorator(new ConvexShape());
    }
    
    public override void Build()
    {
        _triangle.SetPointCount(3);
        for (var i = 0; i < _triangle.GetPointCount(); i++)
        {
            _triangle.SetPoint((uint)i, new Vector2f(_parameters[i * 2], _parameters[i * 2 + 1]));
        }
        _triangle.SetPosition(_position);
        _triangle.SetFillColor(new Color(_visuals[0]));
        _triangle.SetOutlineColor(new Color(_visuals[1]));
        _triangle.SetOutlineThickness((int)_visuals[2]);
    }

    public override TriangleDecorator GetResult() => _triangle;
}