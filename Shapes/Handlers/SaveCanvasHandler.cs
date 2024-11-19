using Shapes.Strategies;

namespace Shapes.Handlers;

public sealed class SaveCanvasHandler(CanvasHandler canvasHandler)
{
   private readonly CanvasHandler _canvasHandler = canvasHandler;

   private static readonly SaveShapesToTextFileStrategy SaveTextStrategy = new();
   private static readonly SaveShapesToBinaryFileStrategy SaveBinaryStrategy = new();
   private SaveShapesStrategy _saveStrategy = SaveTextStrategy;

   private static readonly OpenShapesFromTextFileStrategy OpenTextStrategy = new();
   private static readonly OpenShapesFromBinaryFileStrategy OpenBinaryStrategy = new();
   private OpenShapesStrategy _openStrategy = OpenTextStrategy;

   private void Save(string filename)
   {
      _canvasHandler.UnselectAll();
      
      _saveStrategy.Save(filename, _canvasHandler.Shapes);
   }

   private void Open(string filename)
   {
      _canvasHandler.Clear();
      
      var shapes = _openStrategy.Open(filename);
      shapes.ForEach(_canvasHandler.AddShape);
   }

   public void SaveToTextFile(string filename)
   {
      _saveStrategy = SaveTextStrategy;
      Save(filename);
   }

   public void SaveToBinaryFile(string filename)
   {
      _saveStrategy = SaveBinaryStrategy;
      Save(filename);
   }

   public void OpenFromTextFile(string filename)
   {
      _openStrategy = OpenTextStrategy;
      Open(filename);
   }

   public void OpenFromBinaryFile(string filename)
   {
      _openStrategy = OpenBinaryStrategy;
      Open(filename);
   }
}