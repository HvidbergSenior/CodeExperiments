namespace Insight.BuildingBlocks.Infrastructure.Marten
{
    public class MartenConfig : IMartenConfig
    {
        public const string DefaultConfigKey = "Marten";
        public const string DefaultSchema = "public";
        public const bool DefaultRecreateDatabase = false;
        public const string DefaultFullTextSearchLanguage = "danish";
        public const bool DefaultPopulateWithDemoData = true;
        public const bool DefaultAddFullTextExtension = false;
        
        public string ConnectionString { get; set; } = string.Empty;
        public string SchemaName { get; set; } = DefaultSchema;
        public bool ShouldRecreateDatabase { get; set; } = DefaultRecreateDatabase;
        public string FullTextSearchLanguage { get; set; } = DefaultFullTextSearchLanguage;
        public bool PopulateWithDemoData { get; set; } = DefaultPopulateWithDemoData;
        public bool AddFullTextExtension { get; set; } = DefaultAddFullTextExtension;
    }

    public interface IMartenConfig
    {
        string FullTextSearchLanguage { get; }
        
        bool PopulateWithDemoData { get; }
        
        bool ShouldRecreateDatabase { get; }

        bool AddFullTextExtension { get; }
    }
}