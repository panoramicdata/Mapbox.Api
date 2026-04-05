using Mapbox.Api.Exceptions;

namespace Mapbox.Api.Test.Config;

/// <summary>
/// Test configuration
/// </summary>
public class Configuration
{
	/// <summary>
	/// Mapbox client options
	/// </summary>
	public MapboxClientOptions MapboxClientOptions { get; set; } = null!;

	public void Validate()
	{
		// MerakiClientOptions should be present
		if (MapboxClientOptions is null)
		{
			throw new ConfigurationException($"Missing {nameof(MapboxClientOptions)}");
		}
		MapboxClientOptions.Validate();
	}
}
