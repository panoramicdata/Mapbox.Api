using AwesomeAssertions;
using Mapbox.Api.Exceptions;
using System;
using Xunit;

namespace Mapbox.Api.Test;

/// <summary>
/// Tests for <see cref="MapboxClientOptions"/> that need no access token, so they run everywhere.
/// </summary>
public class MapboxClientOptionsTests
{
	[Fact]
	public void Validate_WithAccessToken_Succeeds()
	{
		var options = new MapboxClientOptions { AccessToken = "an-access-token" };

		var validate = () => options.Validate();

		validate.Should().NotThrow();
	}

	[Theory]
	[InlineData("")]
	[InlineData("   ")]
	public void Validate_WithoutAccessToken_Throws(string accessToken)
	{
		var options = new MapboxClientOptions { AccessToken = accessToken };

		var validate = () => options.Validate();

		validate.Should()
			.Throw<ConfigurationException>()
			.WithMessage($"*{nameof(MapboxClientOptions.AccessToken)}*");
	}

	[Fact]
	public void Validate_WithNegativeMaxBackOffDelay_Throws()
	{
		var options = new MapboxClientOptions
		{
			AccessToken = "an-access-token",
			MaxBackOffDelay = TimeSpan.FromSeconds(-1)
		};

		var validate = () => options.Validate();

		validate.Should()
			.Throw<ConfigurationException>()
			.WithMessage($"*{nameof(MapboxClientOptions.MaxBackOffDelay)}*");
	}

	[Fact]
	public void Defaults_AreSensible()
	{
		var options = new MapboxClientOptions();

		options.AccessToken.Should().BeEmpty();
		options.MaxBackOffDelay.Should().BePositive();
		options.MaxAttemptCount.Should().BePositive();
	}
}
