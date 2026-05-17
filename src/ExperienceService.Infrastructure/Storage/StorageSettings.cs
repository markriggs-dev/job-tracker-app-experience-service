namespace ExperienceService.Infrastructure.Storage;

public class StorageSettings
{
    public string Endpoint { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = "experience-profiles";
    public bool UseSSL { get; set; } = false;
}
