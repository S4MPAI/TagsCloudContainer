using System.Drawing;
using Autofac;
using DeepMorphy;
using TagsCloudVisualization.ColorFactories;
using TagsCloudVisualization.ImageSavers;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Options;
using TagsCloudVisualization.TagsCloudImageCreators;
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
        builder.RegisterType<MorphAnalyzer>().AsSelf().SingleInstance();

        return builder;
    }

    public static ContainerBuilder RegisterTextReaders(this ContainerBuilder builder)
    {
        builder.RegisterType<TxtReader>().As<ITextReader>();

        return builder;
    }

    public static ContainerBuilder RegisterImageSavers(this ContainerBuilder builder, TagsCloudVisualizationOptions options)
    {
        builder.RegisterType<PngSaver>().WithParameter("path", options.OutputFilePath).As<IImageSaver>();

        return builder;
    }

    public static ContainerBuilder RegisterColorFactory(this ContainerBuilder builder, TagsCloudVisualizationOptions options)
    {
        if (options.ColorName == "random")
            builder.RegisterType<RandomColorFactory>().As<IColorFactory>();
        else
            builder.RegisterType<DefaultColorFactory>().As<IColorFactory>().WithParameter("colorName", options.ColorName);

        return builder;
    }

    public static ContainerBuilder RegisterWordHandlers(this ContainerBuilder builder)
    {
        builder.RegisterType<WordsInLowerCaseHandler>().As<IWordHandler>();
        builder.RegisterType<BoringWordsHandler>().As<IWordHandler>();
        builder.RegisterType<StemmingWordsHandler>().As<IWordHandler>();

        return builder;
    }

    public static ContainerBuilder RegisterCloudLayouter(this ContainerBuilder builder, TagsCloudVisualizationOptions options)
    {
        builder
            .RegisterType<CircularCloudLayouter>()
            .WithParameters([
                new NamedParameter("center", new Point(options.ImageWidth / 2, options.ImageHeight / 2)),
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

    public static ContainerBuilder RegisterTagVisualizer(this ContainerBuilder builder, TagsCloudVisualizationOptions options)
    {
        builder
            .RegisterType<TagVisualizer>()
            .As<ITagVisualizer>()
            .WithParameter("imageSize", new Size(options.ImageWidth, options.ImageHeight));

        return builder;
    }

    public static ContainerBuilder RegisterTagsCloudImageCreator(this ContainerBuilder builder)
    {
        builder.RegisterType<TagsCloudImageCreator>().AsSelf();

        return builder;
    }
}