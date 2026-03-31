using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mapbox.Api;

internal class AuthenticatedBackingOffHttpClientHandler : HttpClientHandler
{
	private readonly MapboxClientOptions _options;
	private readonly ILogger _logger;
	private readonly LogLevel _levelToLogAt = LogLevel.Trace;

	public AuthenticatedBackingOffHttpClientHandler(MapboxClientOptions options, ILogger logger)
	{
		_options = options;
		_logger = logger;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		// Ensure the API key is set
		if (_options.AccessToken?.Length == 0)
		{
			throw new InvalidOperationException(Resources.ApiKeyIsNotSet);
		}

		request.RequestUri = new Uri($"{request.RequestUri}?access_token={_options.AccessToken}&cachebuster={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds():F0}");

		var logPrefix = $"Request {Guid.NewGuid()}: ";

		var attemptCount = 0;
		while (true)
		{
			attemptCount++;
			cancellationToken.ThrowIfCancellationRequested();

			await LogRequestAsync(request, logPrefix).ConfigureAwait(false);

			var httpResponseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

			await LogResponseAsync(httpResponseMessage, logPrefix).ConfigureAwait(false);

			var statusCodeInt = (int)httpResponseMessage.StatusCode;
			var delay = GetRetryDelay(httpResponseMessage, statusCodeInt, logPrefix, attemptCount);

			if (delay is null)
			{
				if (attemptCount > 1)
				{
					_logger.LogDebug($"{logPrefix}Received {statusCodeInt} on attempt {attemptCount}/{_options.MaxAttemptCount}.");
				}

				return httpResponseMessage;
			}

			if (attemptCount >= _options.MaxAttemptCount)
			{
				_logger.LogDebug($"{logPrefix}Giving up retrying.  Returning {statusCodeInt} on attempt {attemptCount}/{_options.MaxAttemptCount}.");
				return httpResponseMessage;
			}

			_logger.LogDebug($"{logPrefix}Waiting {delay.Value.TotalSeconds:N2}s.");
			await Task.Delay(delay.Value, cancellationToken).ConfigureAwait(false);
		}
	}

	private async Task LogRequestAsync(HttpRequestMessage request, string logPrefix)
	{
		if (!_logger.IsEnabled(_levelToLogAt))
		{
			return;
		}

		_logger.Log(_levelToLogAt, $"{logPrefix}Request\r\n{request}");
		if (request.Content != null)
		{
			_logger.Log(_levelToLogAt, $"{logPrefix}RequestContent\r\n" + await request.Content.ReadAsStringAsync().ConfigureAwait(false));
		}
	}

	private async Task LogResponseAsync(HttpResponseMessage response, string logPrefix)
	{
		if (!_logger.IsEnabled(_levelToLogAt))
		{
			return;
		}

		_logger.Log(_levelToLogAt, $"{logPrefix}Response\r\n{response}");
		if (response.Content != null)
		{
			_logger.Log(_levelToLogAt, $"{logPrefix}ResponseContent\r\n" + await response.Content.ReadAsStringAsync().ConfigureAwait(false));
		}
	}

	private TimeSpan? GetRetryDelay(HttpResponseMessage response, int statusCodeInt, string logPrefix, int attemptCount)
	{
		switch (statusCodeInt)
		{
			case 429:
				var headers = response.Headers;
				var foundHeader = headers.TryGetValues("Retry-After", out var retryAfterHeaders);
				var retryAfterSecondsString = foundHeader
					? retryAfterHeaders.FirstOrDefault() ?? "1"
					: "1";
				if (!int.TryParse(retryAfterSecondsString, out var retryAfterSeconds))
				{
					retryAfterSeconds = 1;
				}

				_logger.LogDebug($"{logPrefix}Received {statusCodeInt} on attempt {attemptCount}/{_options.MaxAttemptCount}.");
				return TimeSpan.FromSeconds(1.1 * retryAfterSeconds);
			case 502:
				_logger.LogDebug($"{logPrefix}Received {statusCodeInt} on attempt {attemptCount}/{_options.MaxAttemptCount}.");
				return TimeSpan.FromSeconds(5);
			default:
				return null;
		}
	}
}
