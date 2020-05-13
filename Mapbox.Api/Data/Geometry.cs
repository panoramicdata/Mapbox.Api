using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mapbox.Api.Data
{
	[DataContract]
	public class Geometry
	{
		[DataMember(Name = "type")]
		public string Type { get; set; } = null!;

		[DataMember(Name = "coordinates")]
		public IList<double> Coordinates { get; set; } = null!;

		[DataMember(Name = "interpolated")]
		public bool? Interpolated { get; set; }
	}
}