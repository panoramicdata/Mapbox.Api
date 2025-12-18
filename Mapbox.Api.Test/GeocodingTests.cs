using AwesomeAssertions;
using Mapbox.Api.Data;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Mapbox.Api.Test;

public class GeocodingTests(ITestOutputHelper iTestOutputHelper) : MapboxClientTest(iTestOutputHelper)
{
	[Fact]
	public async Task GetAllAsync_Succeeds()
	{
		var geocoding = await MapboxClient
			.Geocoding
			.GetForwardsAsync("Greenwich Observatory, Greenwich, London");
		geocoding.Should().BeOfType<Geocoding>();
		geocoding.Should().NotBeNull();
	}
}
