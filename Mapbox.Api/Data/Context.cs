using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mapbox.Api.Data;

[DataContract]
public class Context
{
	[DataMember(Name = "id")]
	public string Id { get; set; } = null!;

	[DataMember(Name = "text")]
	public string Text { get; set; } = null!;

	[DataMember(Name = "wikidata")]
	public string Wikidata { get; set; } = null!;

	[DataMember(Name = "short_code")]
	public string ShortCode { get; set; } = null!;
}