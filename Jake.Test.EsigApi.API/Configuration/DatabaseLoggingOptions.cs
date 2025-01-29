namespace Jake.Test.EsigApi.API.Configuration;

public class DatabaseLoggingOptions
{
    public const string SectionName = "DatabaseLogging";
    
    public string ConnectionString { get; set; } = string.Empty;
    public bool EnableRequestLogging { get; set; } = true;
    public bool EnableExceptionLogging { get; set; } = true;
    public int RetentionDays { get; set; } = 30;
} 