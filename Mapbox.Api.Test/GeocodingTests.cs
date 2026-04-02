using AwesomeAssertions;
using Mapbox.Api.Data;
using System.Threading.Tasks;
using Xunit;

namespace Mapbox.Api.Test;

public class GeocodingTests(ITestOutputHelper iTestOutputHelper) : MapboxClientTest(iTestOutputHelper)
{
	[Fact]
	public async Task GetAllAsync_Succeeds()
	{
		var geocoding = await MapboxClient
			.Geocoding
			.GetForwardsAsync("Greenwich Observatory, Greenwich, London", TestContext.Current.CancellationToken);
		geocoding.Should().BeOfType<Geocoding>();
		geocoding.Should().NotBeNull();
	}
}
