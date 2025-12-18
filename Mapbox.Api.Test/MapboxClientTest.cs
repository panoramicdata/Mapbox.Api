using Mapbox.Api.Exceptions;
using Mapbox.Api.Test.Config;
using Neovolve.Logging.Xunit;
using Newtonsoft.Json;
using System;
using System.IO;
using Xunit.Abstractions;

namespace Mapbox.Api.Test;

public class MapboxClientTest(ITestOutputHelper iTestOutputHelper)
{
	protected ICacheLogger Logger { get; } = iTestOutputHelper.BuildLogger();

	public Configuration Configuration
	{
		get
		{
			// Have we already created this?
			if (field != null)
			{
				// Yes - return that one
				return field;
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
			field = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(fileInfo.FullName))
				?? throw new FormatException("Invalid configuration format.");

			field.Validate();
			return field;
		}
	}

	protected MapboxClient MapboxClient
		=> field ??= new MapboxClient(Configuration.MapboxClientOptions, Logger);
}