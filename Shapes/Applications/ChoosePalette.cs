using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Shapes.Decorators;
using Shapes.Factories;
using Shapes.Handlers;
using Shapes.Iterators;
using Shapes.Types;

namespace Shapes.Applications;

public sealed class ChoosePalette
{
    private readonly int _margin;
    private const int Size = 64;
    private const int Columns = 6;
    private const int TextHeight = 18;
    
    private readonly DefaultsListIterator<Color> _fillColorList;
    private readonly DefaultsListIterator<Color> _outlineColorList;
    private readonly DefaultsListIterator<int> _outlineThicknessList;
    private readonly List<CanvasHandler> _canvasHandlers;
    private readonly RenderWindow _window;
    
    private List<Text> _texts = [];
    
    private ChoosePalette(
        DefaultsListIterator<Color> fillColorList,
        DefaultsListIterator<Color> outlineColorList,
        DefaultsListIterator<int> outlineThicknessList)
    {
        _fillColorList = fillColorList;
        _outlineColorList = outlineColorList;
        _outlineThicknessList = outlineThicknessList;
        _margin = outlineThicknessList.ToList().Max() * 2;
        _canvasHandlers = CreateCanvasHandlers();
        _window = CreateWindow();
        _window.Closed += WindowClosed;
        _window.MouseButtonPressed += WindowMousePressed;

        ProcessWindow();
    }

    public static void Show(
        DefaultsListIterator<Color> fillColorList,
        DefaultsListIterator<Color> outlineColorList,
        DefaultsListIterator<int> outlineThicknessList)
    {
        new ChoosePalette(fillColorList, outlineColorList, outlineThicknessList);
    }
    
    private void ProcessWindow()
    {
        while (_window.IsOpen)
        {
            _window.DispatchEvents();
            _window.Clear(DefaultColors.DeepDark);
            
            _texts.ForEach(t => _window.Draw(t));
            _canvasHandlers.ForEach(c => c.Draw(_window));
            
            _window.Display();
        }
    }
    
    private RenderWindow CreateWindow()
    {
        var maxCanvasWidth = _canvasHandlers.Select(c => GetCanvasBounds(c).Width).Prepend(0).Max();
        var width = maxCanvasWidth + 2 * _margin;
        
        var lastCanvasBounds = GetCanvasBounds(_canvasHandlers.Last());
        var height = lastCanvasBounds.Top + lastCanvasBounds.Height + _margin;
        
        return new RenderWindow(new VideoMode(
                (uint)width,
                (uint)height),
            "ChoosePalette");
    }

    private List<CanvasHandler> CreateCanvasHandlers()
    {
        var canvasHandlers = new List<CanvasHandler>();

        // fill color
        var fillColorCanvas = new CanvasHandler();
        _texts.Add(new Text("Fill Color", DefaultFiles.Font, TextHeight));
        _texts.Last().Position = new Vector2f(_margin, _margin);

        foreach (var color in _fillColorList.ToList())
        {
            var newShape = CreatePaletteCell(fillColorCanvas, 0);
            newShape.SetFillColor(color);
            fillColorCanvas.AddShape(newShape);
        }
        fillColorCanvas.ForceSelectShape(fillColorCanvas.Shapes.First(s => s.FillColor == _fillColorList.GetCurrent()));
        canvasHandlers.Add(fillColorCanvas);
        
        // outline color
        var outlineColorCanvas = new CanvasHandler();
        var lastCanvasBounds = GetCanvasBounds(fillColorCanvas);
        _texts.Add(new Text("Outline Color", DefaultFiles.Font, TextHeight));
        _texts.Last().Position = new Vector2f(_margin, lastCanvasBounds.Top + lastCanvasBounds.Height + _margin);

        foreach (var color in _outlineColorList.ToList())
        {
            var newShape = CreatePaletteCell(outlineColorCanvas, lastCanvasBounds.Top + lastCanvasBounds.Height);
            newShape.SetFillColor(DefaultColors.Dark);
            newShape.SetOutlineColor(color);
            newShape.SetOutlineThickness(2);
            outlineColorCanvas.AddShape(newShape);
        }
        outlineColorCanvas.ForceSelectShape(outlineColorCanvas.Shapes.First(s => s.OutlineColor == _outlineColorList.GetCurrent()));
        canvasHandlers.Add(outlineColorCanvas);
        
        // outline thickness
        var outlineThicknessCanvas = new CanvasHandler();
        lastCanvasBounds = GetCanvasBounds(outlineColorCanvas);
        _texts.Add(new Text("Outline Thickness", DefaultFiles.Font, TextHeight));
        _texts.Last().Position = new Vector2f(_margin, lastCanvasBounds.Top + lastCanvasBounds.Height + _margin);

        foreach (var thickness in _outlineThicknessList.ToList())
        {
            var newShape = CreatePaletteCell(outlineThicknessCanvas, lastCanvasBounds.Top + lastCanvasBounds.Height);
            newShape.SetFillColor(DefaultColors.Dark);
            newShape.SetOutlineColor(DefaultColors.Orange);
            newShape.SetOutlineThickness(thickness);
            outlineThicknessCanvas.AddShape(newShape);
        }
        outlineThicknessCanvas.ForceSelectShape(outlineThicknessCanvas.Shapes.First(s => s.OutlineThickness == _outlineThicknessList.GetCurrent()));
        canvasHandlers.Add(outlineThicknessCanvas);

        return canvasHandlers;
    }

    private RectangleDecorator CreatePaletteCell(CanvasHandler canvasHandler, int canvasStartY)
    {
        var newShape = ShapeDecoratorFactory.GetRectangleDecorator(["0", "0", Size.ToString(), Size.ToString()]);
        newShape.SetPosition(new Vector2f(
            (_margin + Size) * (canvasHandler.Shapes.Count % Columns) + _margin,
            (_margin + Size) * (float)Math.Floor((double)canvasHandler.Shapes.Count / Columns) + 2 * _margin + TextHeight + canvasStartY));
        return newShape;
    }
    
    private void WindowClosed(object? sender, EventArgs e) => _window.Close();

    private void WindowMousePressed(object? sender, MouseButtonEventArgs e)
    {
        if (e.Button != Mouse.Button.Left) return;
        
        var activeCanvas = _canvasHandlers.SingleOrDefault(c => GetCanvasBounds(c).Contains(e.X, e.Y));
        if (activeCanvas == null) return;
        
        var activeShape = activeCanvas.GetActiveShape(new Vector2i(e.X, e.Y));
        if (!activeCanvas.Shapes.Contains(activeShape)) return;

        if (activeCanvas == _canvasHandlers[0]) _fillColorList.SetCurrent(activeShape.FillColor);
        if (activeCanvas == _canvasHandlers[1]) _outlineColorList.SetCurrent(activeShape.OutlineColor);
        if (activeCanvas == _canvasHandlers[2]) _outlineThicknessList.SetCurrent(activeShape.OutlineThickness);
        
        activeCanvas.ForceSelectShape(activeShape);
    }

    private static IntRect GetCanvasBounds(CanvasHandler canvasHandler)
    {
        var shapes = canvasHandler.Shapes.Concat(canvasHandler.SelectedShapes).ToList();
        var firstShapeBounds = shapes[0].GetGlobalBounds();
        var bounds = (new Vector2i(firstShapeBounds.Left, firstShapeBounds.Top),
            new Vector2i(
                firstShapeBounds.Left + firstShapeBounds.Width,
                firstShapeBounds.Top + firstShapeBounds.Height));

        foreach (var shapeBounds in shapes.Select(s => s.GetGlobalBounds()))
        {
            if (shapeBounds.Left < bounds.Item1.X)
            {
                bounds.Item1.X = shapeBounds.Left;
            }

            if (shapeBounds.Top < bounds.Item1.Y)
            {
                bounds.Item1.Y = shapeBounds.Top;
            }

            if (shapeBounds.Left + shapeBounds.Width > bounds.Item2.X)
            {
                bounds.Item2.X = shapeBounds.Left + shapeBounds.Width;
            }

            if (shapeBounds.Top + shapeBounds.Height > bounds.Item2.Y)
            {
                bounds.Item2.Y = shapeBounds.Top + shapeBounds.Height;
            }
        }

        return new IntRect(
            bounds.Item1.X,
            bounds.Item1.Y,
            bounds.Item2.X - bounds.Item1.X,
            bounds.Item2.Y - bounds.Item1.Y);
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}