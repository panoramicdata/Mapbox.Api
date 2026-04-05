using System.Text.Json.Serialization;

namespace Mapbox.Api.Data;

/// <summary>
/// A context element providing hierarchy information for a geocoding result.
/// </summary>
public class Context
{
	/// <summary>
	/// The context identifier.
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; set; } = null!;

	/// <summary>
	/// The context text.
	/// </summary>
	[JsonPropertyName("text")]
	public string Text { get; set; } = null!;

	/// <summary>
	/// The Wikidata identifier.
	/// </summary>
	[JsonPropertyName("wikidata")]
	public string Wikidata { get; set; } = null!;

	/// <summary>
	/// The short code (e.g. region or country code).
	/// </summary>
	[JsonPropertyName("short_code")]
	public string ShortCode { get; set; } = null!;
}