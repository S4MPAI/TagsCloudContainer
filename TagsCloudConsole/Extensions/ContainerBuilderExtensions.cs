using Autofac;
using TagsCloudVisualization;
using TagsCloudVisualization.ColorFactories;
using TagsCloudVisualization.ImageSavers;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.TextReaders;
using TagsCloudVisualization.Visualizers;
using TagsCloudVisualization.WordsHandlers;
using WeCantSpell.Hunspell;

namespace TagsCloudConsole.Extensions;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder RegisterWordAnalytics(this ContainerBuilder builder)
    {
        builder.RegisterInstance(WordList.CreateFromFiles("Dictionaries/ru/ru.dic"));
        
        return builder;
    }
    
    public static ContainerBuilder RegisterTextReaders(this ContainerBuilder builder)
    {
        builder.RegisterType<TxtReader>().As<ITextReader>();
        
        return builder;
    }
    
    public static ContainerBuilder RegisterImageSavers(this ContainerBuilder builder, TagsCloudVisualizationOptions options)
    {
        builder.RegisterType<PngSaver>().WithParameter("path", options.PathToSave).As<IImageSaver>();
        
        return builder;
    }
    
    public static ContainerBuilder RegisterColorFactory(this ContainerBuilder builder, TagsCloudVisualizationOptions options)
    {
        builder.RegisterType<DefaultColorFactory>().As<IColorFactory>().WithParameter("colorName", options.ColorName);
        
        return builder;
    }
    
    public static ContainerBuilder RegisterWordHandlers(this ContainerBuilder builder)
    {
        builder.RegisterType<WordsInLowerCaseHandler>().As<IColorFactory>();
        builder.RegisterType<BoringWordsHandler>().As<IColorFactory>();
        builder.RegisterType<StemmingWordsHandler>().As<IColorFactory>();
        
        return builder;
    }
    
    public static ContainerBuilder RegisterCloudLayouter(this ContainerBuilder builder, TagsCloudVisualizationOptions options)
    {
        builder
            .RegisterType<CircularCloudLayouter>()
            .WithParameters([
                new NamedParameter("radius", options.LayoutRadius),
                new NamedParameter("angleOffset", options.LayoutAngleOffset)
            ])
            .As<ICloudLayouter>();
        
        return builder;
    }
    
    public static ContainerBuilder RegisterTagLayouter(this ContainerBuilder builder, TagsCloudVisualizationOptions options)
    {
        builder.RegisterType<TagLayouterOptions>().WithParameters([
            new NamedParameter("minFontSize", options.MinFontSize),
            new NamedParameter("maxFontSize", options.MaxFontSize),
            new NamedParameter("fontName", options.FontFamily)
        ])
            .AsSelf();
        builder
            .RegisterType<TagLayouter>()
            .As<ITagLayouter>();
        
        return builder;
    }
    
    public static ContainerBuilder RegisterTagVisualizer(this ContainerBuilder builder)
    {
        builder.RegisterType<CartesianTagVisualizer>().As<ITagVisualizer>();
        
        return builder;
    }
    
    public static ContainerBuilder RegisterTagsCloudImageCreator(this ContainerBuilder builder)
    {
        builder.RegisterType<TagsCloudImageCreator>().AsSelf();
        
        return builder;
    }
}