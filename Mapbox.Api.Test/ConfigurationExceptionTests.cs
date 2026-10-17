using AwesomeAssertions;
using Mapbox.Api.Exceptions;
using System;
using Xunit;

namespace Mapbox.Api.Test;

/// <summary>
/// Tests for <see cref="ConfigurationException"/> that need no access token, so they run everywhere.
/// </summary>
public class ConfigurationExceptionTests
{
	[Fact]
	public void Ctor_WithNoArguments_CreatesException()
	{
		var exception = new ConfigurationException();

		exception.Should().BeAssignableTo<Exception>();
	}

	[Fact]
	public void Ctor_WithMessage_SetsMessage()
	{
		var exception = new ConfigurationException("a message");

		exception.Message.Should().Be("a message");
	}

	[Fact]
	public void Ctor_WithMessageAndInnerException_SetsBoth()
	{
		var inner = new InvalidOperationException("inner");
		var exception = new ConfigurationException("a message", inner);

		exception.Message.Should().Be("a message");
		exception.InnerException.Should().BeSameAs(inner);
	}
}
