using AwesomeAssertions;
using Mapbox.Api.Data;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace Mapbox.Api.Test;

/// <summary>
/// Tests for <see cref="Feature"/> that need no access token, so they run everywhere.
/// </summary>
public class FeatureTests
{
	private static Feature CreateFeature()
		=> new()
		{
			Id = "an-id",
			Type = "Feature",
			PlaceType = new List<string> { "country" },
			Relevance = 0.5,
			Properties = new Properties { Accuracy = "point" },
			Text = "London",
			PlaceName = "London, England",
			MatchingText = "London",
			MatchingPlaceName = "London",
			Center = new List<double> { -0.1, 51.5 },
			Geometry = new Geometry { Type = "Point", Coordinates = new List<double> { -0.1, 51.5 } },
			Address = "London, England",
			Context = new List<Context> { new() { Id = "a-context", Text = "England", Wikidata = "Q1856", ShortCode = "ENG" } },
			Bbox = new List<double> { -0.5, 51.0, 0.5, 52.0 }
		};

	[Fact]
	public void Properties_RoundTripThroughJson()
	{
		var feature = CreateFeature();

		var json = JsonSerializer.Serialize(feature);
		var deserialized = JsonSerializer.Deserialize<Feature>(json);

		deserialized.Should().NotBeNull();
		deserialized!.Id.Should().Be(feature.Id);
		deserialized.Type.Should().Be(feature.Type);
		deserialized.PlaceType.Should().Equal(feature.PlaceType);
		deserialized.Relevance.Should().Be(feature.Relevance);
		deserialized.Properties.Accuracy.Should().Be(feature.Properties.Accuracy);
		deserialized.Text.Should().Be(feature.Text);
		deserialized.PlaceName.Should().Be(feature.PlaceName);
		deserialized.MatchingText.Should().Be(feature.MatchingText);
		deserialized.MatchingPlaceName.Should().Be(feature.MatchingPlaceName);
		deserialized.Center.Should().Equal(feature.Center);
		deserialized.Geometry.Type.Should().Be(feature.Geometry.Type);
		deserialized.Geometry.Coordinates.Should().Equal(feature.Geometry.Coordinates);
		deserialized.Address.Should().Be(feature.Address);
		deserialized.Bbox.Should().Equal(feature.Bbox);
		deserialized.Context.Should().HaveCount(1);
		deserialized.Context[0].Id.Should().Be("a-context");
		deserialized.Context[0].Text.Should().Be("England");
		deserialized.Context[0].Wikidata.Should().Be("Q1856");
		deserialized.Context[0].ShortCode.Should().Be("ENG");
	}
}
