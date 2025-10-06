public class AppSettings
{
    public PathsConfig Paths { get; set; } = new();
    public LoggingConfig Logging { get; set; } = new();
    public FeatureFlagsConfig FeatureFlags { get; set; } = new();
}

public class PathsConfig
{
    public string ExportFile { get; set; } = "MockFiles/adm_export.txt";
    public string ResponseSuccessFile { get; set; } = "MockFiles/adm_response_success.txt";
    public string ResponseErrorFile { get; set; } = "MockFiles/adm_response_error.txt";
    public string LogFile { get; set; } = "MockFiles/export.log";
}

public class LoggingConfig
{
    public bool EnableFileLogging { get; set; } = true;
    public string LogLevel { get; set; } = "INFO";
}

public class FeatureFlagsConfig
{
    public bool SimulateFtpFailure { get; set; } = false;
    public bool EnableDynamicResponse { get; set; } = true;
}