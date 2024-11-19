using SFML.System;

namespace Shapes.States;

public interface IState
{
    void Click(Vector2i point);
    void PressedKeyG();
    void PressedKeyU();
    void MouseMoved(Vector2i point);
    void MouseReleased();
}