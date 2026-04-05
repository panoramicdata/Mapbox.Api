using System.Text.Json.Serialization;

namespace Mapbox.Api.Data;

/// <summary>
/// Properties associated with a geocoding feature.
/// </summary>
public class Properties
{
	/// <summary>
	/// The accuracy of the geocoding result.
	/// </summary>
	[JsonPropertyName("accuracy")]
	public string Accuracy { get; set; } = null!;
}