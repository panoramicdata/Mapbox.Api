using AwesomeAssertions;
using Mapbox.Api.Data;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace Mapbox.Api.Test;

/// <summary>
/// Tests for the <see cref="Geocoding"/> model that need no access token, so they run everywhere.
/// </summary>
public class GeocodingModelTests
{
	[Fact]
	public void Geocoding_RoundTripsThroughJson()
	{
		var geocoding = new Geocoding
		{
			Type = "FeatureCollection",
			Query = new List<string> { "London" },
			Features = new List<Feature> { new() { Id = "f1", Text = "London" } },
			Attribution = "© Mapbox"
		};

		var json = JsonSerializer.Serialize(geocoding);
		var deserialized = JsonSerializer.Deserialize<Geocoding>(json);

		deserialized.Should().NotBeNull();
		deserialized!.Type.Should().Be(geocoding.Type);
		deserialized.Query.Should().Equal(geocoding.Query);
		deserialized.Attribution.Should().Be(geocoding.Attribution);
		deserialized.Features.Should().HaveCount(1);
		deserialized.Features[0].Id.Should().Be("f1");
		deserialized.Features[0].Text.Should().Be("London");
	}
}
