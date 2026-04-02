using Mapbox.Api.Exceptions;
using Mapbox.Api.Test.Config;
using Neovolve.Logging.Xunit;
using Newtonsoft.Json;
using System;
using System.IO;
using Xunit;

namespace Mapbox.Api.Test;

public class MapboxClientTest(ITestOutputHelper iTestOutputHelper)
{
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
		if (!fileInfo.Exists)
		{
			// No - hint to the user what to do
			throw new ConfigurationException("Missing appsettings.json.  Please copy the appsettings.example.json in the project root folder and set the various values appropriately.");
		}
		// Yes

		// Load in the config
		_configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(fileInfo.FullName))
			?? throw new FormatException("Invalid configuration format.");

		_configuration.Validate();
		return _configuration;
	}

	protected MapboxClient MapboxClient
		=> field ??= new MapboxClient(GetConfiguration().MapboxClientOptions, Logger);
}