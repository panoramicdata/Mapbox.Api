[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

# Mapbox.Api

[![Nuget](https://img.shields.io/nuget/v/Mapbox.Api)](https://www.nuget.org/packages/Mapbox.Api/)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/d3b9e33de5f2461cabe69723352223ed)](https://app.codacy.com/gh/panoramicdata/Mapbox.Api/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)

To use the Mapbox API nuget package:

## Visual Studio

1.	Open your project in Visual Studio
2.  Right-click on the project and click "Manage Nuget packages"
3.  Find the package "Mapbox.Api" - install the latest version

## Example code (C# 8.0)

``` C#
using Mapbox.Api;
using System;
using System.Threading.Tasks;

namespace My.Project
{
	public static class Program
	{
		public static async Task Main()
		{
			var MapboxClient = new MapboxClient(new MapboxClientOptions
			{
				AccessToken = "0123456789abcdef0123456789abcdef01234567"
			});

			var geocoding = await MapboxClient
				.Geocoding
				.GetForwardsAsync("Greenwich Observatory, Greenwich, London")
				.ConfigureAwait(false);

			Console.WriteLine($"Location: {geolocation}");
		}
	}
}
````

## API Documentation

The Mapbox API documentation can be found here:

-	[Mapbox API Documentation](https://docs.mapbox.com/api)
