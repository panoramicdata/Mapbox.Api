using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mapbox.Api.Data
{
	[DataContract]
	public class Geocoding
	{
		[DataMember(Name = "type")]
		public string Type { get; set; } = null!;

		[DataMember(Name = "query")]
		public IList<string> Query { get; set; } = null!;

		[DataMember(Name = "features")]
		public IList<Feature> Features { get; set; } = null!;

		[DataMember(Name = "attribution")]
		public string Attribution { get; set; } = null!;
	}
}