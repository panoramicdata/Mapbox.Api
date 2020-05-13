using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mapbox.Api.Data
{
	[DataContract]
	public class Feature
	{
		[DataMember(Name = "id")]
		public string Id { get; set; } = null!;

		[DataMember(Name = "type")]
		public string Type { get; set; } = null!;

		[DataMember(Name = "place_type")]
		public IList<string> PlaceType { get; set; } = null!;

		[DataMember(Name = "relevance")]
		public double Relevance { get; set; }

		[DataMember(Name = "properties")]
		public Properties Properties { get; set; } = null!;

		[DataMember(Name = "text")]
		public string Text { get; set; } = null!;

		[DataMember(Name = "place_name")]
		public string PlaceName { get; set; } = null!;

		[DataMember(Name = "matching_text")]
		public string MatchingText { get; set; } = null!;

		[DataMember(Name = "matching_place_name")]
		public string MatchingPlaceName { get; set; } = null!;

		[DataMember(Name = "center")]
		public IList<double> Center { get; set; } = null!;

		[DataMember(Name = "geometry")]
		public Geometry Geometry { get; set; } = null!;

		[DataMember(Name = "address")]
		public string Address { get; set; } = null!;

		[DataMember(Name = "context")]
		public IList<Context> Context { get; set; } = null!;

		[DataMember(Name = "bbox")]
		public IList<double> Bbox { get; set; } = null!;
	}
}