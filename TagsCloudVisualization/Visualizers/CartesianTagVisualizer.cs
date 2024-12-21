using System.Drawing;
using TagsCloudVisualization.ColorFactories;
using TagsCloudVisualization.Models;

namespace TagsCloudVisualization.Visualizers;

public class CartesianTagVisualizer(IColorFactory colorFactory, Size imageSize) : ITagVisualizer
{
    private readonly Point _centerOffset = new Point(imageSize.Width / 2, imageSize.Height / 2);
    private readonly Size _imageSize = imageSize;
    
    public Bitmap Visualize(IEnumerable<Tag> tags)
    {
        var bitmap = new Bitmap(_imageSize.Width, _imageSize.Height);
        using var graphics = Graphics.FromImage(bitmap);

        foreach (var tag in tags)
        {
            var brush = new SolidBrush(colorFactory.GetColor());
            var font = new Font(tag.FontFamily, tag.FontSize);
            var rectangle = tag.Rectangle;
            rectangle.Offset(_centerOffset);
            
            graphics.DrawString(tag.Content, font, brush, tag.Rectangle);
        }
        
        return bitmap;
    }
}