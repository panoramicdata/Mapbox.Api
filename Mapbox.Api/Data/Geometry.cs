using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mapbox.Api.Data;

/// <summary>
/// A GeoJSON geometry object.
/// </summary>
public class Geometry
{
	/// <summary>
	/// The geometry type.
	/// </summary>
	[JsonPropertyName("type")]
	public string Type { get; set; } = null!;

	/// <summary>
	/// The coordinates of the geometry.
	/// </summary>
	[JsonPropertyName("coordinates")]
	public IList<double> Coordinates { get; set; } = null!;

	/// <summary>
	/// Whether the result is interpolated.
	/// </summary>
	[JsonPropertyName("interpolated")]
	public bool? Interpolated { get; set; }
}