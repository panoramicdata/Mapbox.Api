using AwesomeAssertions;
using Mapbox.Api.Data;
using System.Threading.Tasks;
using Xunit;

namespace Mapbox.Api.Test;

/// <summary>
/// Tests that call the live Mapbox API, so they need an access token.
/// They are excluded from CI runs where no token is available.
/// </summary>
[Trait("Category", "Integration")]
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
