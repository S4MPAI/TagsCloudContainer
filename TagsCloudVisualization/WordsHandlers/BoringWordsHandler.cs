using DeepMorphy;

namespace TagsCloudVisualization.WordsHandlers;

public class BoringWordsHandler : IWordHandler
{
    private static readonly MorphAnalyzer Analyzer = new();
    private static readonly HashSet<string> CorrectSpeechParts = ["сущ", "инф_гл", "прил", "деепр"];
    
    public IEnumerable<string> Handle(IEnumerable<string> words)
    {
        return Analyzer
            .Parse(words)
            .Where(x => CorrectSpeechParts.Contains(x["чр"].BestGramKey))
            .Select(x => x.Text);
    }
}