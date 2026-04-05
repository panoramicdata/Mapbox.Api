using Mapbox.Api.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Refit;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mapbox.Api;

/// <summary>
/// A Mapbox Dashboard API client
/// </summary>
public class MapboxClient : IDisposable
{
	private readonly HttpClient _httpClient;
	private readonly AuthenticatedBackingOffHttpClientHandler _httpClientHandler;

	/// <summary>
	/// Initializes a new instance of the <see cref="MapboxClient"/> class.
	/// </summary>
	/// <param name="options">The client options.</param>
	public MapboxClient(MapboxClientOptions options) : this(options, default) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="MapboxClient"/> class.
	/// </summary>
	/// <param name="options">The client options.</param>
	/// <param name="logger">The logger.</param>
	public MapboxClient(MapboxClientOptions options, ILogger? logger)
	{
		var resolvedLogger = logger ?? NullLogger.Instance;
		_httpClientHandler = new AuthenticatedBackingOffHttpClientHandler(options ?? throw new ArgumentNullException(nameof(options)), resolvedLogger);
		_httpClient = new HttpClient(_httpClientHandler) { BaseAddress = new Uri("https://api.mapbox.com/") };

		var refitSettings = new RefitSettings
		{
			ContentSerializer = new SystemTextJsonContentSerializer(
			new JsonSerializerOptions
			{
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
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

	/// <summary>
	/// Releases unmanaged and optionally managed resources.
	/// </summary>
	/// <param name="disposing">Whether to release managed resources.</param>
	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				_httpClient.Dispose();
				_httpClientHandler.Dispose();
			}

			_disposedValue = true;
		}
	}

	// This code added to correctly implement the disposable pattern.
	/// <inheritdoc />
	public void Dispose()
	{
		// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		Dispose(true);

		GC.SuppressFinalize(this);
	}
	#endregion
}
