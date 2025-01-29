namespace Jake.Test.EsigApi.API.Configuration;

public class SwaggerOptions
{
    public const string SectionName = "Swagger";
    
    public string Title { get; set; } = "E-Signature API";
    public string Version { get; set; } = "v1";
    public string Description { get; set; } = "An API for managing electronic signatures";
    public ContactInfo Contact { get; set; } = new();
    public LicenseInfo License { get; set; } = new();
}

public class ContactInfo
{
    public string Name { get; set; } = "API Support";
    public string Email { get; set; } = "support@example.com";
}

public class LicenseInfo
{
    public string Name { get; set; } = "MIT";
    public string Url { get; set; } = "https://opensource.org/licenses/MIT";
} 