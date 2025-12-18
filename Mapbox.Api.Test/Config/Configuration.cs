using Mapbox.Api.Exceptions;
using System.Runtime.Serialization;

namespace Mapbox.Api.Test.Config;

/// <summary>
/// Test configuration
/// </summary>
[DataContract]
public class Configuration
{
	/// <summary>
	/// Mapbox client options
	/// </summary>
	[DataMember(Name = "MapboxClientOptions")]
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
