using Mapbox.Api.Exceptions;
using Mapbox.Api.Test.Config;
using Neovolve.Logging.Xunit;
using Newtonsoft.Json;
using System.IO;
using Xunit.Abstractions;

namespace Mapbox.Api.Test
{
	public class MapboxClientTest
	{
		private MapboxClient? _mapboxClient;

		private Configuration? _configuration;

		protected ICacheLogger Logger { get; }

		public MapboxClientTest(ITestOutputHelper iTestOutputHelper)
		{
			Logger = iTestOutputHelper.BuildLogger();
		}

		public Configuration Configuration
		{
			get
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
				_configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(fileInfo.FullName));
				_configuration.Validate();
				return _configuration;
			}
		}

		protected MapboxClient MapboxClient
			=> _mapboxClient ??= new MapboxClient(Configuration.MapboxClientOptions, Logger);
	}
}