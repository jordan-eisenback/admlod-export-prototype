using Xunit;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

public class AppSettingsTests
{
    [Fact]
    public void ShouldLoadAppSettingsFromConfiguration()
    {
        var configData = new Dictionary<string, string>
        {
            {"Paths:ExportFile", "MockFiles/adm_export.txt"},
            {"Paths:ResponseSuccessFile", "MockFiles/adm_response_success.txt"},
            {"Paths:ResponseErrorFile", "MockFiles/adm_response_error.txt"},
            {"Paths:LogFile", "MockFiles/export.log"},
            {"Logging:EnableFileLogging", "true"},
            {"Logging:LogLevel", "INFO"},
            {"FeatureFlags:SimulateFtpFailure", "false"},
            {"FeatureFlags:EnableDynamicResponse", "true"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        var appSettings = new AppSettings();
        configuration.Bind(appSettings);

        Assert.Equal("MockFiles/adm_export.txt", appSettings.Paths.ExportFile);
        Assert.True(appSettings.Logging.EnableFileLogging);
        Assert.Equal("INFO", appSettings.Logging.LogLevel);
        Assert.False(appSettings.FeatureFlags.SimulateFtpFailure);
        Assert.True(appSettings.FeatureFlags.EnableDynamicResponse);
    }
}