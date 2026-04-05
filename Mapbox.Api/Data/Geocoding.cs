using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mapbox.Api.Data;

/// <summary>
/// A geocoding response from the Mapbox API.
/// </summary>
public class Geocoding
{
	/// <summary>
	/// The GeoJSON type.
	/// </summary>
	[JsonPropertyName("type")]
	public string Type { get; set; } = null!;

	/// <summary>
	/// The query terms.
	/// </summary>
	[JsonPropertyName("query")]
	public IList<string> Query { get; set; } = null!;

	/// <summary>
	/// The returned features.
	/// </summary>
	[JsonPropertyName("features")]
	public IList<Feature> Features { get; set; } = null!;

	/// <summary>
	/// The attribution string.
	/// </summary>
	[JsonPropertyName("attribution")]
	public string Attribution { get; set; } = null!;
}