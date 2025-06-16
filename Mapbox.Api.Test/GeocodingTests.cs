using AwesomeAssertions;
using Mapbox.Api.Data;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;

namespace Mapbox.Api.Test
{
	public class GeocodingTests : MapboxClientTest
	{
		public GeocodingTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async Task GetAllAsync_Succeeds()
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
