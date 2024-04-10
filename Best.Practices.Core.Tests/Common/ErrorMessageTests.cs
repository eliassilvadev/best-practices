using Best.Practices.Core.Common;
using FluentAssertions;
using Xunit;

namespace Best.Practices.Core.Tests.Common
{
    public class ErrorMessageTests
    {
        public ErrorMessageTests()
        {
        }

        [Fact]
        public void Constructor_GivenAStringMessageWithoutCode_ReturnsErrorMessage()
        {
            //Act
            var exception = new ErrorMessage("Error message test");

            // Assert
            exception.Code.Should().Be(CommonConstants.ErrorCodes.DefaulErrorCode);
            exception.Message.Should().Be("Error message test");
        }

        [Fact]
        public void Constructor_GivenAStringMessageWithoutSeparatorAndCode_ReturnsErrorMessage()
        {
            //Act
            var exception = new ErrorMessage($"999{CommonConstants.ErroMessageSeparator}Error message test");

            // Assert
            exception.Code.Should().Be("999");
            exception.Message.Should().Be("Error message test");
        }

        [Fact]
        public void Constructor_GivenCodeAndMessage_ReturnsErrorMessage()
        {
            //Act
            var exception = new ErrorMessage("999", "Error message test");

            // Assert
            exception.Code.Should().Be("999");
            exception.Message.Should().Be("Error message test");
        }

        [Fact]
        public void GetErrorCodeFromMessage_GivenMessageWithCodeAndSeparator_ReturnsErrorCode()
        {
            //Act
            var errorCode = ErrorMessage.GetErrorCodeFromMessage("999;Error message test");

            // Assert
            errorCode.Should().Be("999");
        }
    }
}