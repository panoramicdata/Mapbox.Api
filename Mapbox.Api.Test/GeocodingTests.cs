using FluentAssertions;
using Mapbox.Api.Data;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Mapbox.Api.Test
{
	public class GeocodingTests : MapboxClientTest
	{
		public GeocodingTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetAllAsync_Succeeds()
		{
			var geocoding = await MapboxClient
				.Geocoding
				.GetForwardsAsync("Greenwich Observatory, Greenwich, London")
				.ConfigureAwait(false);
			geocoding.Should().BeOfType<Geocoding>();
			geocoding.Should().NotBeNull();
		}
	}
}
