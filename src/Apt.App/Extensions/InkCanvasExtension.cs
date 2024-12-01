using System.Windows.Controls;

namespace Apt.App.Extensions
{
    public static class InkCanvasExtensions
    {
        public static DrawingGroup GetDrawing(this InkCanvas inkCanvas)
        {
            DrawingGroup drawingGroup = new DrawingGroup();
            using (DrawingContext drawingContext = drawingGroup.Open())
            {
                drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(new Point(0, 0), new Size(inkCanvas.ActualWidth, inkCanvas.ActualHeight)));
                inkCanvas.Strokes.Draw(drawingContext);
            }
            return drawingGroup;
        }
    }
}
