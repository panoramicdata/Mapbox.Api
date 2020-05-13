using Mapbox.Api.Data;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Mapbox.Api.Interfaces
{
	/// <summary>
	/// Represents a collection of functions to interact with the API endpoints
	/// </summary>
	public interface IGeocoding
	{
		/// <summary>
		/// Gets forward geocoding.
		/// </summary>
		/// <param name="location">The location to look up</param>
		/// <returns>Task of Object</returns>
		[Get("/geocoding/v5/mapbox.places/{location}.json")]
		Task<Geocoding> GetForwardsAsync(
			[AliasAs("location")]string location,
			CancellationToken cancellationToken = default);
	}
}