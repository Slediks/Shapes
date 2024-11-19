using Shapes.Applications;

namespace Shapes;

public static class Program
{
    static void Main()
    {
        try
        {
            var application = Application.GetInstance();

            application.ReadDefaultShapes();
            application.ProcessWindow();
            application.PrintShapesInfo();
        }
        catch (Exception e)
        {
            throw new Exception("An exception occurred: " + e.Message);
        }
    }
}