using Mapbox.Api.Exceptions;
using Mapbox.Api.Test.Config;
using Neovolve.Logging.Xunit;
using System;
using System.IO;
using System.Text.Json;
using Xunit;

namespace Mapbox.Api.Test;

public class MapboxClientTest(ITestOutputHelper iTestOutputHelper)
{
	/// <summary>
	/// Environment variable used to supply the access token where no appsettings.json exists,
	/// such as on a CI runner.
	/// </summary>
	internal const string AccessTokenEnvironmentVariable = "MAPBOX_ACCESS_TOKEN";

	private Configuration? _configuration;

	protected ICacheLogger Logger { get; } = iTestOutputHelper.BuildLogger();

	protected Configuration GetConfiguration()
	{
		// Have we already created this?
		if (_configuration != null)
		{
			// Yes - return that one
			return _configuration;
		}
		// No - we need to create one

		// Load config from file
		var fileInfo = new FileInfo("../../../appsettings.json");

		// Does the config file exist?
		if (fileInfo.Exists)
		{
			// Yes - load in the config
			_configuration = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(fileInfo.FullName))
				?? throw new FormatException("Invalid configuration format.");
		}
		else
		{
			// No - fall back to the environment, so that a CI runner can supply the token as a secret
			var accessToken = Environment.GetEnvironmentVariable(AccessTokenEnvironmentVariable);

			// Is a token available there?
			if (string.IsNullOrWhiteSpace(accessToken))
			{
				// No - hint to the user what to do
				throw new ConfigurationException($"Missing appsettings.json.  Please copy the appsettings.example.json in the project root folder and set the various values appropriately, or set the {AccessTokenEnvironmentVariable} environment variable.");
			}
			// Yes

			_configuration = new Configuration
			{
				MapboxClientOptions = new MapboxClientOptions { AccessToken = accessToken }
			};
		}

		_configuration.Validate();
		return _configuration;
	}

	protected MapboxClient MapboxClient
		=> field ??= new MapboxClient(GetConfiguration().MapboxClientOptions, Logger);
}
