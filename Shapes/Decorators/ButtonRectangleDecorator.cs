using SFML.Graphics;
using SFML.System;
using Shapes.Types;

namespace Shapes.Decorators;

public sealed class ButtonRectangleDecorator : RectangleDecorator
{
    private readonly Action _command;
    private readonly Text _text;
    
    private const int ButtonHeight = 48;
    private const int VerticalMarginOut = 1;
    private const int HorizontalMarginIn = 7;
    private const int HorizontalMarginOut = 1;
    
    public ToolbarButtonType Type { get; }
    
    public ButtonRectangleDecorator(ToolbarButtonType type, Action command, string text)
        : base(new RectangleShape())
    {
        Type = type;
        _command = command;
        _text = new Text(text, DefaultFiles.Font, 18);
        Build();
    }
    
    public void Press() => _command();

    public override void Draw(RenderWindow window)
    {
        base.Draw(window);
        window.Draw(_text);
    }

    public override void Move(int moveX, int moveY)
    {
        base.Move(moveX, moveY);
        MoveText(moveX, moveY);
    }

    private void MoveText(int moveX, int moveY)
    {
        var currentPosition = _text.Position;
        _text.Position = new Vector2f(currentPosition.X + moveX, currentPosition.Y + moveY);
    }

    private void Build()
    {
        SetPosition(new Vector2f(HorizontalMarginOut, VerticalMarginOut));
        SetSize(new Vector2f(
            _text.GetGlobalBounds().Width + HorizontalMarginIn * 2,
            ButtonHeight));
        SetFillColor(DefaultColors.DeepDark);

        var textHeight = (int)_text.GetGlobalBounds().Height;
        MoveText(HorizontalMarginIn + HorizontalMarginOut, 
            VerticalMarginOut + (ButtonHeight - textHeight) / 3);
    }
}