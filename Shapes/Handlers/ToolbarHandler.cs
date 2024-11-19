using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Shapes.Applications;
using Shapes.Compounds;
using Shapes.Decorators;
using Shapes.Factories;
using Shapes.Iterators;
using Shapes.Types;

namespace Shapes.Handlers;

public sealed class ToolbarHandler : BaseShapeHandler
{
    private readonly Action _setDragAndDropStateAction;
    private readonly Action _setAddShapeStateAction;
    private readonly Action _setChangeFillColorStateAction;
    private readonly Action _setChangeOutlineColorStateAction;
    private readonly Action _setChangeOutlineThicknessStateAction;
    private readonly Action _undoAction;

    private readonly DefaultsListIterator<Color> _outlineColors
        = new([
            DefaultColors.Blue,
            DefaultColors.Red,
            DefaultColors.Green,
            DefaultColors.Orange,
            DefaultColors.Violet,
            DefaultColors.BlueDark
        ]);

    private readonly DefaultsListIterator<Color> _fillColors
        = new([
            DefaultColors.Blue,
            DefaultColors.Red,
            DefaultColors.Green,
            DefaultColors.Orange,
            DefaultColors.Violet,
            DefaultColors.BlueDark
        ]);

    private readonly DefaultsListIterator<int> _outlineThicknesses
        = new([
            0,
            1,
            2,
            3,
            4,
            5
        ]);

    private readonly DefaultsListIterator<int> _addShapeType
        = new([
            (int)ShapeType.Triangle,
            (int)ShapeType.Rectangle,
            (int)ShapeType.Circle
        ]);

    private const int Height = 50;

    private ShapeDecorator _currentPalette = null!;

    public Color CurrentFillColor => _fillColors.GetCurrent();
    public Color CurrentOutlineColor => _outlineColors.GetCurrent();
    public int CurrentOutlineThickness => _outlineThicknesses.GetCurrent();
    public ShapeType CurrentAddShapeType => (ShapeType)_addShapeType.GetCurrent();
    public void ShowPalette() => ChoosePalette.Show(_fillColors, _outlineColors, _outlineThicknesses);

    private ToolbarHandler(
        ShapeDecoratorGroup shapesGroup,
        SelectedShapeDecoratorGroup selectedShapesGroup,
        Action setDragAndDropStateAction,
        Action setAddShapeStateAction,
        Action setChangeFillColorStateAction,
        Action setChangeOutlineColorStateAction,
        Action setChangeOutlineThicknessStateAction,
        Action undoAction)
        : base(shapesGroup, selectedShapesGroup)
    {
        _setDragAndDropStateAction = setDragAndDropStateAction;
        _setAddShapeStateAction = setAddShapeStateAction;
        _setChangeFillColorStateAction = setChangeFillColorStateAction;
        _setChangeOutlineColorStateAction = setChangeOutlineColorStateAction;
        _setChangeOutlineThicknessStateAction = setChangeOutlineThicknessStateAction;
        _undoAction = undoAction;

        foreach (var type in Enum.GetValues<ToolbarButtonType>())
        {
            var newButton = new ButtonRectangleDecorator(type, GetButtonCommand(type), GetButtonText(type));

            var lastButton = (ButtonRectangleDecorator?)ShapesGroup.Shapes.LastOrDefault();
            if (lastButton != null)
            {
                var moveX = (int)(lastButton.Position.X + lastButton.Size.X);
                newButton.Move(moveX, 0);
            }
            
            AddShape(newButton);
        }
        
        var firstButton = (ButtonRectangleDecorator?)ShapesGroup.Shapes.FirstOrDefault();
        if (firstButton == null) return;
        
        SelectButton(firstButton);
        firstButton.Press();
    }

    public ToolbarHandler(
        Action setDragAndDropStateAction,
        Action setAddShapeStateAction,
        Action setChangeFillColorStateAction,
        Action setChangeOutlineColorStateAction,
        Action setChangeOutlineThicknessStateAction,
        Action undoAction)
        : this(
            ShapeDecoratorFactory.GetShapeDecoratorGroup(),
            ShapeDecoratorFactory.GetSelectedShapeDecoratorGroup(),
            setDragAndDropStateAction,
            setAddShapeStateAction,
            setChangeFillColorStateAction,
            setChangeOutlineColorStateAction,
            setChangeOutlineThicknessStateAction,
            undoAction)
    {
    }

    public bool HandleClick(Vector2i point)
    {
        if (point.Y > Height)
        {
            return false;
        }

        // Palette
        if (_currentPalette.GetGlobalBounds().Contains(point))
        {
            ShowPalette();
            return true;
        }
        
        var activeButton = (ButtonRectangleDecorator?)GetActiveShape(point);
        if (activeButton == null) return true;
        
        if (activeButton.Type == ToolbarButtonType.Undo)
        {
            activeButton.Press();
            return true;
        }

        if (SelectedShapesGroup.Shapes.Contains(activeButton))
        {
            ChangeToolbarParams(activeButton.Type);
        }
        else
        {
            SelectButton(activeButton);
            activeButton.Press();
        }

        return true;
    }

    public override void Draw(RenderWindow window)
    {
        var background = new RectangleDecorator(new RectangleShape());
        background.SetPosition(new Vector2f(0, 0));
        background.SetSize(new Vector2f(window.Size.X, Height));
        background.SetFillColor(DefaultColors.Dark);
        background.Draw(window);

        _currentPalette = CurrentAddShapeType switch
        {
            ShapeType.Triangle => ShapeDecoratorFactory.GetTriangleDecorator(["16", "0", "0", "32", "32", "32"]),
            ShapeType.Rectangle => ShapeDecoratorFactory.GetRectangleDecorator(["0", "0", "32", "32"]),
            ShapeType.Circle => ShapeDecoratorFactory.GetCircleDecorator(["0", "0", "16"]),
            _ => throw new ArgumentOutOfRangeException()
        };
        _currentPalette.SetPosition(new Vector2f(window.Size.X - 50, 9));
        _currentPalette.SetFillColor(CurrentFillColor);
        _currentPalette.SetOutlineColor(CurrentOutlineColor);
        _currentPalette.SetOutlineThickness(CurrentOutlineThickness);
        _currentPalette.Draw(window);
        
        ShapesGroup.Draw(window);
        SelectedShapesGroup.Draw(window);
    }

    private void SelectButton(ButtonRectangleDecorator button)
    {
        UnselectAll();
        Select(button);
    }

    private void ChangeToolbarParams(ToolbarButtonType type)
    {
        switch (type)
        {
            case ToolbarButtonType.AddShape:
                _addShapeType.MoveNext();
                break;
            case ToolbarButtonType.ChangeFillColor:
                _fillColors.MoveNext();
                break;
            case ToolbarButtonType.ChangeOutlineColor:
                _outlineColors.MoveNext();
                break;
            case ToolbarButtonType.ChangeOutlineThickness:
                _outlineThicknesses.MoveNext();
                break;
            default:
                return;
        }
    }

    private Action GetButtonCommand(ToolbarButtonType type)
    {
        return type switch
        {
            ToolbarButtonType.DragAndDrop => _setDragAndDropStateAction,
            ToolbarButtonType.AddShape => _setAddShapeStateAction,
            ToolbarButtonType.ChangeFillColor => _setChangeFillColorStateAction,
            ToolbarButtonType.ChangeOutlineColor => _setChangeOutlineColorStateAction,
            ToolbarButtonType.ChangeOutlineThickness => _setChangeOutlineThicknessStateAction,
            ToolbarButtonType.Undo => _undoAction,
            _ => throw new InvalidOperationException($"Unknown toolbar button type: {type}")
        };
    }

    private static string GetButtonText(ToolbarButtonType type)
    {
        return type switch
        {
            ToolbarButtonType.DragAndDrop => "Drag&Drop",
            ToolbarButtonType.AddShape => "Add",
            ToolbarButtonType.ChangeFillColor => "Fill Color",
            ToolbarButtonType.ChangeOutlineColor => "Outline Color",
            ToolbarButtonType.ChangeOutlineThickness => "Outline Thickness",
            ToolbarButtonType.Undo => "Undo",
            _ => throw new InvalidOperationException($"Unknown toolbar button type: {type}")
        };
    }
}