using Best.Practices.Core.Common;
using Best.Practices.Core.Exceptions;
using FluentAssertions;
using Xunit;

namespace Best.Practices.Core.Tests.Exceptions
{
    public class InvalidInputExceptionTests
    {
        public InvalidInputExceptionTests()
        {
        }

        [Fact]
        public void Constructor_Always_InstantiateErrorList()
        {
            //Act
            var exception = new InvalidInputException("Message Test");

            exception.Errors.Should().HaveCount(1);
            exception.Errors.Should().ContainEquivalentOf(new ErrorMessage("Message Test"));
        }

        [Fact]
        public void Constructor_GivenAErrorObjectParameter_InstantiateErrorList()
        {
            //Act
            var exception = new InvalidInputException(new ErrorMessage("Message Test"));

            exception.Errors.Should().HaveCount(1);
            exception.Errors.Should().ContainEquivalentOf(new ErrorMessage("Message Test"));
        }

        [Fact]
        public void Constructor_GivenAErrorListParameter_InstantiateErrorList()
        {
            //Arrange
            var errorList = new List<ErrorMessage>() {
                new("Message Test 1"),
                new("Message Test 2") };

            //Act
            var exception = new InvalidInputException(errorList);

            exception.Errors.Should().HaveCount(2);
            exception.Errors.Should().ContainEquivalentOf(new ErrorMessage("Message Test 1"));
            exception.Errors.Should().ContainEquivalentOf(new ErrorMessage("Message Test 1"));
        }
    }
}