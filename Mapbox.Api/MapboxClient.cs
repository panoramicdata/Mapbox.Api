using Mapbox.Api.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Refit;
using System;
using System.Net.Http;

namespace Mapbox.Api
{
	/// <summary>
	/// A Mapbox Dashboard API client
	/// </summary>
	public class MapboxClient : IDisposable
	{
		private readonly ILogger _logger;
		private readonly HttpClient _httpClient;
		private readonly AuthenticatedBackingOffHttpClientHandler _httpClientHandler;

		/// <summary>
		/// A Meraki portal client
		/// </summary>
		/// <param name="options"></param>
		/// <param name="logger"></param>

		public MapboxClient(MapboxClientOptions options) : this(options, default) { }

		public MapboxClient(MapboxClientOptions options, ILogger? logger)
		{
			_logger = logger ?? NullLogger.Instance;
			_httpClientHandler = new AuthenticatedBackingOffHttpClientHandler(options ?? throw new ArgumentNullException(nameof(options)), _logger);
			_httpClient = new HttpClient(_httpClientHandler) { BaseAddress = new Uri("https://api.mapbox.com/") };

			var refitSettings = new RefitSettings
			{
				ContentSerializer = new NewtonsoftJsonContentSerializer(
				new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore
				})
			};

			Geocoding = RestService.For<IGeocoding>(_httpClient, refitSettings);
		}

		/// <summary>
		/// Action batches
		/// </summary>
		public IGeocoding Geocoding { get; }

		#region IDisposable Support
		private bool _disposedValue;

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					_logger.LogDebug(Resources.Disposing);
					_httpClient.Dispose();
					_httpClientHandler.Dispose();
					_logger.LogDebug(Resources.Disposed);
				}

				_disposedValue = true;
			}
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);

			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
