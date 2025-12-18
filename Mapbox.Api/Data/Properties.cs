using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mapbox.Api.Data;

[DataContract]
public class Properties
{
	[DataMember(Name = "accuracy")]
	public string Accuracy { get; set; } = null!;
}