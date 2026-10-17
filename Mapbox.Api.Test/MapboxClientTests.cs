using AwesomeAssertions;
using Mapbox.Api.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using Xunit;

namespace Mapbox.Api.Test;

/// <summary>
/// Tests for <see cref="MapboxClient"/> that need no access token, so they run everywhere.
/// </summary>
public class MapboxClientTests
{
	private static MapboxClientOptions CreateOptions()
		=> new() { AccessToken = "an-access-token" };

	[Fact]
	public void Ctor_WithOptions_CreatesClient()
	{
		using var client = new MapboxClient(CreateOptions());

		client.Should().NotBeNull();
		client.Geocoding.Should().NotBeNull();
	}

	[Fact]
	public void Ctor_WithOptionsAndLogger_CreatesClient()
	{
		using var client = new MapboxClient(CreateOptions(), NullLogger.Instance);

		client.Should().NotBeNull();
		client.Geocoding.Should().NotBeNull();
	}

	[Fact]
	public void Ctor_WithNullOptions_Throws()
	{
		var ctor = () => new MapboxClient(null!);

		ctor.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public void Dispose_CanBeCalledMoreThanOnce()
	{
		var client = new MapboxClient(CreateOptions());

		client.Dispose();
		client.Dispose();

		client.Should().NotBeNull();
	}
}
