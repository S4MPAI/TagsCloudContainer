using System.Drawing;
using TagsCloudVisualization.ColorFactories;
using TagsCloudVisualization.Models;

namespace TagsCloudVisualization.Visualizers;

public class TagVisualizer(IColorFactory colorFactory, Size imageSize) : ITagVisualizer
{
    public Bitmap Visualize(IEnumerable<Tag> tags)
    {
        var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
        using var graphics = Graphics.FromImage(bitmap);

        foreach (var tag in tags)
        {
            var brush = new SolidBrush(colorFactory.GetColor());
            var font = new Font(tag.FontFamily, tag.FontSize);
            
            graphics.DrawString(tag.Content, font, brush, tag.Rectangle);
        }
        
        return bitmap;
    }
}