using Mapbox.Api.Exceptions;
using System;

namespace Mapbox.Api
{
	/// <summary>
	/// MapboxClient options
	/// </summary>
	public class MapboxClientOptions
	{
		/// <summary>
		/// The Access token
		/// </summary>
		public string AccessToken { get; set; } = string.Empty;

		/// <summary>
		/// When a 429 HttpStatus code is sent, the back-off duration doubles on each attempt.
		/// This option sets the maximum backoff duration.
		/// </summary>
		public TimeSpan MaxBackOffDelay { get; set; } = TimeSpan.FromSeconds(5);

		/// <summary>
		/// When retrying
		/// </summary>
		public int MaxAttemptCount { get; set; } = 5;

		public void Validate()
		{
			// AccessToken
			if (string.IsNullOrWhiteSpace(AccessToken))
			{
				throw new ConfigurationException($"Missing {nameof(AccessToken)}.");
			}

			// MaxBackoffDelay
			if (MaxBackOffDelay < TimeSpan.Zero)
			{
				throw new ConfigurationException($"{nameof(MaxBackOffDelay)} should not be less than zero.");
			}
		}
	}
}