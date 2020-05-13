﻿using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mapbox.Api
{
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
			// The API Key is set

			request.RequestUri = new Uri($"{request.RequestUri}?access_token={_options.AccessToken}&cachebuster={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds():F0}");

			var logPrefix = $"Request {Guid.NewGuid()}: ";

			var attemptCount = 0;
			while (true)
			{
				attemptCount++;
				cancellationToken.ThrowIfCancellationRequested();

				// Only do diagnostic logging if we're at the level we want to enable for as this is more efficient
				if (_logger.IsEnabled(_levelToLogAt))
				{
					_logger.Log(_levelToLogAt, $"{logPrefix}Request\r\n{request}");
					if (request.Content != null)
					{
						_logger.Log(_levelToLogAt, $"{logPrefix}RequestContent\r\n" + await request.Content.ReadAsStringAsync().ConfigureAwait(false));
					}
				}

				// Complete the action
				var httpResponseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

				// Only do diagnostic logging if we're at the level we want to enable for as this is more efficient
				if (_logger.IsEnabled(_levelToLogAt))
				{
					_logger.Log(_levelToLogAt, $"{logPrefix}Response\r\n{httpResponseMessage}");
					if (httpResponseMessage.Content != null)
					{
						_logger.Log(_levelToLogAt, $"{logPrefix}ResponseContent\r\n" + await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false));
					}
				}

				TimeSpan delay;
				// As long as we were not given a back-off request then we'll return the response and any further StatusCode handling is up to the caller
				var statusCodeInt = (int)httpResponseMessage.StatusCode;
				switch (statusCodeInt)
				{
					case 429:
						// Back off by the requested amount.
						var headers = httpResponseMessage.Headers;
						var foundHeader = headers.TryGetValues("Retry-After", out var retryAfterHeaders);
						var retryAfterSecondsString = foundHeader
							? retryAfterHeaders.FirstOrDefault() ?? "1"
							: "1";
						if (!int.TryParse(retryAfterSecondsString, out var retryAfterSeconds))
						{
							retryAfterSeconds = 1;
						}
						delay = TimeSpan.FromSeconds(1.1 * retryAfterSeconds);
						_logger.LogDebug($"{logPrefix}Received {statusCodeInt} on attempt {attemptCount}/{_options.MaxAttemptCount}.");
						break;
					case 502:
						_logger.LogDebug($"{logPrefix}Received {statusCodeInt} on attempt {attemptCount}/{_options.MaxAttemptCount}.");
						delay = TimeSpan.FromSeconds(5);
						break;
					default:
						if (attemptCount > 1)
						{
							_logger.LogDebug($"{logPrefix}Received {statusCodeInt} on attempt {attemptCount}/{_options.MaxAttemptCount}.");
						}
						return httpResponseMessage;
				}
				// Try up to the maximum retry count.
				if (attemptCount >= _options.MaxAttemptCount)
				{
					_logger.LogDebug($"{logPrefix}Giving up retrying.  Returning {statusCodeInt} on attempt {attemptCount}/{_options.MaxAttemptCount}.");
					return httpResponseMessage;
				}

				_logger.LogDebug($"{logPrefix}Waiting {delay.TotalSeconds:N2}s.");
				await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
			}
		}
	}
}
