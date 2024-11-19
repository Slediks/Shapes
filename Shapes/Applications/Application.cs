using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Shapes.Handlers;
using Shapes.States;
using Shapes.Types;
using Shapes.Visitors;

namespace Shapes.Applications;

public sealed class Application
{
    private static readonly Application Instance = new(new RenderWindow(new VideoMode(1000, 1000), "Shapes"), DefaultFiles.Input, DefaultFiles.Output);
    
    private readonly string _input;
    private readonly string _output;
    
    private readonly SaveCanvasHandler _saveCanvasHandler;
    
    private readonly RenderWindow _window;

    private readonly DragAndDropState _dragAndDropState;
    private readonly AddShapeState _addShapeState;
    private readonly ChangeFillColorState _changeFillColorState;
    private readonly ChangeOutlineColorState _changeOutlineColorState;
    private readonly ChangeOutlineThicknessState _changeOutlineThicknessState;

    private IState _state = null!;

    public ToolbarHandler ToolbarHandler { get; }
    public CanvasHandler CanvasHandler { get; }
    public Vector2i ClickMousePosition { get; private set; }

    public bool IsSomethingMoving { get; private set; }

    private Application(RenderWindow window, string input, string output)
    {
        _input = input;
        _output = output;

        _dragAndDropState = new DragAndDropState(this);
        _addShapeState = new AddShapeState(this);
        _changeFillColorState = new ChangeFillColorState(this);
        _changeOutlineColorState = new ChangeOutlineColorState(this);
        _changeOutlineThicknessState = new ChangeOutlineThicknessState(this);

        ToolbarHandler = new ToolbarHandler(
            SetDragAndDropStateAction,
            SetAddShapeStateAction,
            SetChangeFillColorStateAction,
            SetChangeOutlineColorStateAction,
            SetChangeOutlineThicknessStateAction,
            UndoAction);

        CanvasHandler = new CanvasHandler();
        _saveCanvasHandler = new SaveCanvasHandler(CanvasHandler);

        _window = window;
        _window.Closed += WindowClosed;
        _window.MouseButtonPressed += WindowMousePressed;
        _window.MouseButtonReleased += WindowMouseReleased;
        _window.MouseMoved += WindowMouseMoved;
        _window.KeyPressed += WindowKeyPressed;
        
        ClickMousePosition = new Vector2i();
        IsSomethingMoving = false;
    }

    public static Application GetInstance() => Instance;
    
    public void ReadDefaultShapes()
    {
        _saveCanvasHandler.OpenFromTextFile(_input);
    }

    public void PrintShapesInfo()
    {
        var outputStream = new StreamWriter($"{_output}.txt");
        var descriptionVisitor = new ShapeDescriptionVisitor(outputStream);
        var shapes = CanvasHandler.Shapes
            .Concat(CanvasHandler.SelectedShapes)
            .ToList();

        foreach (var shape in shapes)
        {
            shape.Accept(descriptionVisitor);
        }
        
        outputStream.Close();
    }

    public void ProcessWindow()
    {
        while (_window.IsOpen)
        {
            _window.DispatchEvents();
            _window.Clear(DefaultColors.DeepDark);

            DrawApplication();
            
            _window.Display();
        }
    }

    private void DrawApplication()
    {
        CanvasHandler.Draw(_window);
        ToolbarHandler.Draw(_window);
    }

    private void WindowClosed(object? sender, EventArgs e)
    {
        _window.Close();
    }

    private void WindowMousePressed(object? sender, MouseButtonEventArgs e)
    {
        if (e.Button != Mouse.Button.Left) return;
        
        SetClickMousePosition(new Vector2i(e.X, e.Y));

        if (ToolbarHandler.HandleClick(ClickMousePosition)) return;
        
        _state.Click(ClickMousePosition);
    }

    private void WindowMouseReleased(object? sender, MouseButtonEventArgs e)
    {
        _state.MouseReleased();
    }

    private void WindowMouseMoved(object? sender, MouseMoveEventArgs e)
    {
        _state.MouseMoved(new Vector2i(e.X, e.Y));
    }

    private void WindowKeyPressed(object? sender, KeyEventArgs e)
    {
        if (!Keyboard.IsKeyPressed(Keyboard.Key.LControl)) return;

        switch (e.Code)
        {
            case Keyboard.Key.G:
                Console.WriteLine("Pressed Ctrl + G");
                _state.PressedKeyG();
                break;
            case Keyboard.Key.U:
                Console.WriteLine("Pressed Ctrl + U");
                _state.PressedKeyU();
                break;
            case Keyboard.Key.Z:
                Console.WriteLine("Pressed Ctrl + Z");
                UndoAction();
                break;
            case Keyboard.Key.P:
                Console.WriteLine("Pressed Ctrl + P");
                ToolbarHandler.ShowPalette();
                break;
            case Keyboard.Key.T:
                if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                {
                    Console.WriteLine("Pressed Ctrl + S + T");
                    _saveCanvasHandler.SaveToTextFile(DefaultFiles.SaveFileName);
                }
                
                if (Keyboard.IsKeyPressed(Keyboard.Key.O))
                {
                    Console.WriteLine("Pressed Ctrl + O + T");
                    _saveCanvasHandler.OpenFromTextFile(DefaultFiles.OpenFileName);
                }
                
                break;
            case Keyboard.Key.B:
                if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                {
                    Console.WriteLine("Pressed Ctrl + S + B");
                    _saveCanvasHandler.SaveToBinaryFile(DefaultFiles.SaveFileName);
                }
            
                if (Keyboard.IsKeyPressed(Keyboard.Key.O))
                {
                    Console.WriteLine("Pressed Ctrl + O + B");
                    _saveCanvasHandler.OpenFromBinaryFile(DefaultFiles.OpenFileName);
                }
            
                break;
            default:
                return;
        }
    }

    private void SetDragAndDropStateAction()
    {
        _state = _dragAndDropState;
    }

    private void SetAddShapeStateAction()
    {
        _state = _addShapeState;
        CanvasHandler.UnselectAll();
    }

    private void SetChangeFillColorStateAction()
    {
        _state = _changeFillColorState;
        CanvasHandler.UnselectAll();
    }

    private void SetChangeOutlineColorStateAction()
    {
        _state = _changeOutlineColorState;
        CanvasHandler.UnselectAll();
    }

    private void SetChangeOutlineThicknessStateAction()
    {
        _state = _changeOutlineThicknessState;
        CanvasHandler.UnselectAll();
    }

    private void UndoAction()
    {
        CanvasHandler.RollBackHistory();
    }
    
    public void SetClickMousePosition(Vector2i mousePosition) => ClickMousePosition = mousePosition;
    public void SetIsSomethingMoving(bool isSomethingMoving) => IsSomethingMoving = isSomethingMoving;
}