using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mapbox.Api.Data;

/// <summary>
/// A geocoding feature result.
/// </summary>
public class Feature
{
	/// <summary>
	/// The feature identifier.
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; set; } = null!;

	/// <summary>
	/// The GeoJSON type.
	/// </summary>
	[JsonPropertyName("type")]
	public string Type { get; set; } = null!;

	/// <summary>
	/// The place type(s).
	/// </summary>
	[JsonPropertyName("place_type")]
	public IList<string> PlaceType { get; set; } = null!;

	/// <summary>
	/// The relevance score.
	/// </summary>
	[JsonPropertyName("relevance")]
	public double Relevance { get; set; }

	/// <summary>
	/// The feature properties.
	/// </summary>
	[JsonPropertyName("properties")]
	public Properties Properties { get; set; } = null!;

	/// <summary>
	/// The feature text.
	/// </summary>
	[JsonPropertyName("text")]
	public string Text { get; set; } = null!;

	/// <summary>
	/// The full place name.
	/// </summary>
	[JsonPropertyName("place_name")]
	public string PlaceName { get; set; } = null!;

	/// <summary>
	/// The matching text.
	/// </summary>
	[JsonPropertyName("matching_text")]
	public string MatchingText { get; set; } = null!;

	/// <summary>
	/// The matching place name.
	/// </summary>
	[JsonPropertyName("matching_place_name")]
	public string MatchingPlaceName { get; set; } = null!;

	/// <summary>
	/// The center coordinates.
	/// </summary>
	[JsonPropertyName("center")]
	public IList<double> Center { get; set; } = null!;

	/// <summary>
	/// The feature geometry.
	/// </summary>
	[JsonPropertyName("geometry")]
	public Geometry Geometry { get; set; } = null!;

	/// <summary>
	/// The address.
	/// </summary>
	[JsonPropertyName("address")]
	public string Address { get; set; } = null!;

	/// <summary>
	/// The context hierarchy.
	/// </summary>
	[JsonPropertyName("context")]
	public IList<Context> Context { get; set; } = null!;

	/// <summary>
	/// The bounding box.
	/// </summary>
	[JsonPropertyName("bbox")]
	public IList<double> Bbox { get; set; } = null!;
}