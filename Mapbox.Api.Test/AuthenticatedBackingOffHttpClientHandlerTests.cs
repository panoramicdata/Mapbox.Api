using AwesomeAssertions;
using Mapbox.Api;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Mapbox.Api.Test;

/// <summary>
/// Tests for the internal <c>AuthenticatedBackingOffHttpClientHandler</c> that need no access
/// token and no network, so they run everywhere.  The handler is internal, so we reach it
/// through reflection.
/// </summary>
public class AuthenticatedBackingOffHttpClientHandlerTests
{
	private static object CreateHandler(MapboxClientOptions options, Microsoft.Extensions.Logging.ILogger logger)
	{
		var assembly = typeof(MapboxClient).Assembly;
		var handlerType = assembly.GetType("Mapbox.Api.AuthenticatedBackingOffHttpClientHandler")
			?? throw new InvalidOperationException("Handler type not found.");

		var constructor = handlerType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			.FirstOrDefault(c => c.GetParameters().Length == 2)
			?? throw new InvalidOperationException("Constructor not found.");

		return constructor.Invoke(new object[] { options, logger })!;
	}

	private static Task<HttpResponseMessage> InvokeSendAsync(object handler, HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var sendAsync = handler.GetType().GetMethod("SendAsync", BindingFlags.Instance | BindingFlags.NonPublic)
			?? throw new InvalidOperationException("SendAsync not found.");
		return (Task<HttpResponseMessage>)sendAsync.Invoke(handler, new object[] { request, cancellationToken })!;
	}

	private static MapboxClientOptions CreateOptions(int maxAttempts = 1)
		=> new() { AccessToken = "an-access-token", MaxAttemptCount = maxAttempts };

	private static HttpRequestMessage CreateRequest()
		=> new(HttpMethod.Get, "https://api.mapbox.com/geocoding/v5/mapbox.places/test.json");

	[Fact]
	public async Task SendAsync_WithEmptyAccessToken_Throws()
	{
		var handler = CreateHandler(new MapboxClientOptions { AccessToken = string.Empty }, NullLogger.Instance);

		var act = () => InvokeSendAsync(handler, CreateRequest(), CancellationToken.None);

		(await Record.ExceptionAsync(act)).Should().BeOfType<System.InvalidOperationException>();
	}

	[Fact]
	public async Task SendAsync_AddsAccessTokenAndCacheBuster()
	{
		var handler = CreateHandler(CreateOptions(), NullLogger.Instance);
		var request = CreateRequest();

		// The request is rewritten before the network call, so the URL mutation is observable
		// even though the underlying network call fails.
		try
		{
			await InvokeSendAsync(handler, request, CancellationToken.None);
		}
		catch
		{
			// Expected: no valid access token means the real API rejects the call.
		}

		request.RequestUri!.Query.Should().Contain("access_token=an-access-token");
		request.RequestUri.Query.Should().Contain("cachebuster=");
	}

	[Fact]
	public void Ctor_WithNullOptions_DoesNotThrow()
	{
		var handler = CreateHandler(null!, NullLogger.Instance);

		handler.Should().NotBeNull();
	}
}
